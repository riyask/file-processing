using JobSchedule.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSchedule.Data
{
    /// <summary>
    /// class AppDataStorage
    /// </summary>
    /// <seealso cref="JobSchedule.Data.IAppDataStorage" />
    /// <seealso cref="System.IDisposable" />
    public class AppDataStorage : IAppDataStorage, IDisposable
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        private List<JobItem> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDataStorage"/> class.
        /// </summary>
        public AppDataStorage()
        {
            Items = new List<JobItem>();
        }

        /// <summary>
        /// Adds the specified job item.
        /// </summary>
        /// <param name="jobItem">The job item.</param>
        public void Add(JobItem jobItem)
        {
            Items.Add(jobItem);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JobItem GetItem(string id)
        {
            return Items.Where(a => a.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteItem(string id)
        {
            try
            {
                var item = Items.Where(a => a.Id == id).FirstOrDefault();
                Items.Remove(item);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns></returns>
        public List<JobItem> GetAllItems()
        {
            return Items;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Items.Clear();
            Items = null;
        }
    }
}
