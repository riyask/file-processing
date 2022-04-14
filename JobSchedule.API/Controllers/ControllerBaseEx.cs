using JobSchedule.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JobSchedule.API.Controllers
{
    /// <summary>
    /// class ControllerBaseEx
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class ControllerBaseEx : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBaseEx"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ControllerBaseEx(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        protected void LogInfo(LogEventMap eventInfo, string id = null, string message = null)
        {
            _logger.Information($"{nameof(LogEventMap) } = {eventInfo}, Id = {id}, Message = {message}");
        }
    }
}
