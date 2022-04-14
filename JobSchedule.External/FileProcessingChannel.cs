using JobSchedule.Data;
using JobSchedule.Shared.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JobSchedule.External
{
    /// <summary>
    /// class FileProcessingChannel
    /// </summary>
    /// <seealso cref="JobSchedule.External.IFileProcessingChannel" />
    public class FileProcessingChannel : IFileProcessingChannel
    {
        /// <summary>
        /// The maximum messages in channel
        /// </summary>
        private const int MaxMessagesInChannel = 100;

        /// <summary>
        /// The channel
        /// </summary>
        private readonly Channel<JobItem> _channel;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// The application data storage
        /// </summary>
        private readonly IAppDataStorage _appDataStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingChannel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="appDataStorage">The application data storage.</param>
        public FileProcessingChannel(ILogger logger, IAppDataStorage appDataStorage)
        {
            _logger = logger;
            _appDataStorage = appDataStorage;

            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleWriter = false,
                SingleReader = true
            };
            _channel = Channel.CreateBounded<JobItem>(options);
        }

        /// <summary>
        /// Adds the item asynchronous.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        public async Task<bool> AddItemAsync(JobItem jobItem, CancellationToken ct = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                ChannelMessageWriter(LogEventMap.FileProcessService_ListReceived, jobItem.Id, "Writing to channel.");

                if (_channel.Writer.TryWrite(jobItem))
                {
                    ChannelMessageWriter(LogEventMap.FileProcessService_ListReceivedSent, jobItem.Id, "Written to channel successfully.");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all job.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JobItem> GetAllJob()
        {
            return _appDataStorage.GetAllItems()?.Select(a => new JobItem(a));
        }

        /// <summary>
        /// Gets the job by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JobItem GetJobById(string id)
        {
            return _appDataStorage.GetItem(id);
        }

        public string DeleteJobById(string id)
        {
            var item = _appDataStorage.GetItem(id);
            if(item== null)
            {
                return $"Task Id not found";
            }
            else
            {
                _appDataStorage.DeleteItem(id);
                return "Task deleted successfully!!";
            }
        }


        /// <summary>
        /// Completes the writer.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void CompleteWriter(Exception ex = null) => _channel.Writer.Complete(ex);

        /// <summary>
        /// Tries the complete writer.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public bool TryCompleteWriter(Exception ex = null) => _channel.Writer.TryComplete(ex);


        /// <summary>
        /// Reads all asynchronous.
        /// </summary>
        /// <param name="ct">The ct.</param>
        /// <returns></returns>
        public IAsyncEnumerable<JobItem> ReadAllAsync(CancellationToken ct = default) =>
            _channel.Reader.ReadAllAsync(ct);

        /// <summary>
        /// Channels the message writer.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        private void ChannelMessageWriter(LogEventMap events, string id, string message)
        {
            _logger.Information($"{events} JobItemId {id} {message}");
        }


    }

}