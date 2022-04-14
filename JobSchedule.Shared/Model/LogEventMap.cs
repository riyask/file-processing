namespace JobSchedule.Shared.Model
{
    /// <summary>
    /// enum LogEventMap
    /// </summary>
    public enum LogEventMap
    {
        /// <summary>
        /// The hosting service started
        /// </summary>
        HostingService_Started = 1,
        /// <summary>
        /// The hosting service item received
        /// </summary>
        HostingService_ItemReceived,
        /// <summary>
        /// The hosting service item sorting in progress
        /// </summary>
        HostingService_ItemFileProcessingInProgress,
        /// <summary>
        /// The hosting service item sorting completed
        /// </summary>
        HostingService_ItemFileProcessingCompleted,

        /// <summary>
        /// The web API scheduled job all job status
        /// </summary>
        WebApi_ScheduledTask_AllJobStatus = 1000,
        /// <summary>
        /// The web API scheduled job specific job status
        /// </summary>
        WebApi_ScheduledTask_SpecificJobStatus,
        /// <summary>
        /// The web API scheduled job specific job not found
        /// </summary>
        WebApi_ScheduledTask_SpecificJob_NotFound,

        /// <summary>
        /// The web API scheduled job item received
        /// </summary>
        WebApi_ScheduledTask_ItemReceived = 2000,
        /// <summary>
        /// The web API scheduled job item received is empty
        /// </summary>
        WebApi_ScheduledTask_ItemReceived_IsEmpty,
        /// <summary>
        /// The web API scheduled job item received returned
        /// </summary>
        WebApi_ScheduledTask_ItemReceived_Returned,
        /// <summary>
        /// The web API scheduled job item received scheduled
        /// </summary>
        WebApi_ScheduledTask_ItemReceived_Scheduled,

        /// <summary>
        /// The sort service list received
        /// </summary>
        FileProcessService_ListReceived = 10000,
        /// <summary>
        /// The sort service list received sent
        /// </summary>
        FileProcessService_ListReceivedSent,
        /// <summary>
        /// The sort service list enqueued
        /// </summary>
        FileProcessService_ListEnqueued,
        /// <summary>
        /// The sort service in progress
        /// </summary>
        FileProcessService_InProgress,
        /// <summary>
        /// The sort service completed
        /// </summary>
        FileProcessService_Completed
    }
}