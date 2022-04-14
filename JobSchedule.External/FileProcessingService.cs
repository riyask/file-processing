using JobSchedule.Data;
using JobSchedule.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JobSchedule.Shared.Common;
using phoneNumberParserNamespace = JobSchedule.External.PhoneNumberParser.Factory;

namespace JobSchedule.External
{
    /// <summary>
    /// class FileProcessingService
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
    /// <seealso cref="System.IDisposable" />
    public class FileProcessingService : Microsoft.Extensions.Hosting.BackgroundService, IDisposable
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        //To implement timer based event
        //private readonly int _refreshIntervalInSeconds = 3;

        /// <summary>
        /// The sorting channel
        /// </summary>
        private readonly IFileProcessingChannel _fileProcessingChannel;
        /// <summary>
        /// The application data storage
        /// </summary>
        private readonly IAppDataStorage _appDataStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hostApplicationLifetime">The host application lifetime.</param>
        /// <param name="sortingChannel">The sorting channel.</param>
        /// <param name="appDataStorage">The application data storage.</param>
        public FileProcessingService(ILogger logger, IHostApplicationLifetime hostApplicationLifetime, IFileProcessingChannel sortingChannel, IAppDataStorage appDataStorage)
        {
            _logger = logger;
            _fileProcessingChannel = sortingChannel;
            _appDataStorage = appDataStorage;
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information($"{nameof(LogEventMap.HostingService_Started) } , Host Service has started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //To Enable timer based event but we are using IAsyncEnumerable using Channel Options
                    //await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);

                    //Either read IAsyncEnumrable and process or collections and invoke parallel sorting task.
                    //Keeping iAsyncEnumerable for the sake of simplicity at the moment.                
                    var jobId = string.Empty;
                    await foreach (var jobItem in _fileProcessingChannel.ReadAllAsync(stoppingToken))
                    {
                        jobId = jobItem.Id;
                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemReceived) } , Id = {jobId}, Message = Host Service has received this item.");

                        IFormFile file = jobItem.File;
                        jobItem.File = null;
                        _appDataStorage.Add(jobItem);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemFileProcessingInProgress) } , Id = {jobId}, Message = Host Service has received this item.");

                        await Task.Run(() => ProcessFile(jobItem, file), stoppingToken);

                        _logger.Information($"{nameof(LogEventMap.HostingService_ItemFileProcessingCompleted) } , Id = {jobId}, Message = Host Service has completed this item.");

                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warning(ex, "Operation cancelled occured");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception was thrown.");
                _fileProcessingChannel.CompleteWriter(ex);
            }
            finally
            {
                _fileProcessingChannel.TryCompleteWriter();
            }
        }

        /// <summary>
        /// Processes the file.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        private async void ProcessFile(JobItem jobItem, IFormFile file)
        {
            try
            {
                var extension = System.IO.Path.GetExtension(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{Constant.PhoneNumberFileFolder}\\{jobItem.Id.ToString().Replace("-", "")}{extension}");
                using (var sr = new StreamReader(path))
                {
                    var lines = await sr.ReadToEndAsync();
                    var phoneNumbers = lines.Split('\n').ToList();
                    jobItem.PhoneNumber = ParsePhoneNumber(jobItem, phoneNumbers).Select(a=>a).Distinct().ToList();
                    jobItem.UpdateJobStatus(JobStatus.Completed);
                }
                File.Delete(path);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unhandled exception was thrown while reading file");
                jobItem.UpdateJobStatus(JobStatus.Failed);
            }
        }

        /// <summary>
        /// Parses the phone number.
        /// </summary>
        private List<string> ParsePhoneNumber(JobItem jobItem, List<string> phoneNumbers)
        {
            phoneNumberParserNamespace.IPhoneNumberParserFactory phoneNumberParserfactory = null;
            switch (jobItem.PhoneNumberParserType)
            {
                case PhoneNumberParserType.German:
                    phoneNumberParserfactory = new phoneNumberParserNamespace.GermanPhoneNumberParserFactory();
                    break;
                default:
                    break;
            }
            return phoneNumberParserfactory.ParsePhoneNumber(phoneNumbers).PhoneNumbers;
        }
    }
}
