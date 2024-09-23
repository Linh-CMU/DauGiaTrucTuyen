using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class NotificationDAO
    {

        /// <summary>
        /// The instance
        /// </summary>
        private static NotificationDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="NotificationDAO"/> class from being created.
        /// </summary>
        private NotificationDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static NotificationDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new NotificationDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>Adds the notification.</summary>
        /// <param name="notification">The notification.</param>
        /// <exception cref="System.Exception"></exception>
        public async Task AddNotification(Notification notification)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.Notications.Add(notification);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>Lists the notification asynchronous.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<List<Notification>> ListNotificationAsync(string userId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Retrieve the list of notifications for the specified user
                    var result = await context.Notications
                                              .Where(u => u.AccountID == userId)
                                              .ToListAsync();

                    // Update the notification statuses if they are false
                    foreach (var notification in result)
                    {
                        if (!notification.StatusNotification)
                        {
                            notification.StatusNotification = true;
                            context.Entry(notification).State = EntityState.Modified;
                        }
                    }

                    // Save changes only once after all updates
                    await context.SaveChangesAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>Unreads the notification count.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.Exception"></exception>
        public int unreadNotificationCount(string userId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = context.Notications
                                              .Where(u => u.AccountID == userId && u.StatusNotification == false).ToList().Count();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
