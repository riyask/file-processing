using JobSchedule.Shared.Model;
using System.Collections.Generic;

namespace JobSchedule.Data
{
    /// <summary>
    /// interface IAppDataStorage
    /// </summary>
    public interface IAppDataStorage
    {
        /// <summary>
        /// Adds the specified job item.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        public void Add(JobItem jobItem);
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JobItem GetItem(string id);
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns></returns>
        public List<JobItem> GetAllItems();
        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteItem(string id);
    }
}
