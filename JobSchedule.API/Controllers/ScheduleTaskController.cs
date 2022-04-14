using JobSchedule.External;
using JobSchedule.API.Controllers;
using JobSchedule.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JobSchedule.API.Extensions;
using System;
using System.Linq;
using JobSchedule.Shared.Common;

namespace JobSchedule.API.Controllers
{
    /// <summary>
    ///  class ScheduledTaskController
    /// </summary>
    /// <seealso cref="JobSchedule.API.Controllers.ControllerBaseEx" />
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledTaskController : ControllerBaseEx
    {
        /// <summary>
        /// The sorting channel
        /// </summary>
        private readonly IFileProcessingChannel _fileProcessingChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sortingChannel">The sorting channel.</param>
        public ScheduledTaskController(ILogger logger, IFileProcessingChannel sortingChannel)
        : base((logger))
        {
            _fileProcessingChannel = sortingChannel;
        }

        /// <summary>
        /// Gets all scheduled job.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<JobItem> GetAllScheduledTask()
        {
            LogInfo(LogEventMap.WebApi_ScheduledTask_AllJobStatus, null, "Retreiving all jobs metadata.");
            return _fileProcessingChannel.GetAllJob();
        }

        /// <summary>
        /// Gets the scheduled job by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public JobItem GetScheduledTaskById(string id)
        {

            LogInfo(LogEventMap.WebApi_ScheduledTask_SpecificJobStatus, id, $"Querying in collections");

            var jobItem = _fileProcessingChannel.GetJobById(id);

            if (jobItem == null)
            {
                LogInfo(LogEventMap.WebApi_ScheduledTask_SpecificJob_NotFound, id, $"Item not found in collection.");
                return null;
            }

            if (jobItem?.Status == JobStatus.Completed)
            {
                return jobItem;
            }
            else
            {
                return new JobItem(jobItem);
            }
        }


        /// <summary>
        /// Gets the scheduled job by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public string DeleteScheduledTaskById(string id)
        {

            LogInfo(LogEventMap.WebApi_ScheduledTask_SpecificJobStatus, id, $"Deleting task from collections");
            var message = _fileProcessingChannel.DeleteJobById(id);
            return message;

        }

        [Route("ProcessFile")]
        [HttpPost]
        [Produces("application/json")]
        [AllowedExtensions(new string[]{".txt"})]
        public async Task<string> ProcessFileTask(IFormFile file)
        {
            try
            {
                string fileName = file.FileName.Replace(@"\\\\", @"\\");
                var extension = System.IO.Path.GetExtension(fileName);
                if (extension == ".txt")
                {
                    var jobItem = new JobItem(file);
                    jobItem.PhoneNumberParserType = PhoneNumberParserType.German;
                    if (file.Length > 0)
                    {
                        LogInfo(LogEventMap.WebApi_ScheduledTask_ItemReceived, jobItem.Id, $"File recieved {fileName}");
                        jobItem = new JobItem()
                        {
                            File = file,
                            FileName = fileName
                        };
                        var newFileName = $"{jobItem.Id.ToString().Replace("-", "")}{extension}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), $"{Constant.PhoneNumberFileFolder}\\{newFileName}");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        _fileProcessingChannel.AddItemAsync(jobItem);
                        LogInfo(LogEventMap.WebApi_ScheduledTask_ItemReceived_Scheduled, jobItem.Id, $"Item has been scheduled.");

                    }
                    return jobItem.Id;
                }
                else
                {
                    return "File should be .txt";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}