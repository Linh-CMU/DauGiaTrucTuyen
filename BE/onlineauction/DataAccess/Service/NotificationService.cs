using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationService
    {
        /// <summary>
        /// The notification
        /// </summary>
        private readonly INotificationRepository _notification;
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public NotificationService(INotificationRepository notification)
        {
            _notification = notification;
        }
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AddNotification(AddNotificationDTO notification, string uid)
        {
            try
            {
                await _notification.AddNotification(notification, uid);
                return new ResponseDTO() { IsSucceed = true, Message = "Successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Failed" };
            }
        }
        /// <summary>
        /// Lists the notification asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListNotificationAsync(string userId)
        {
            return await _notification.ListNotificationAsync(userId);
        }
        /// <summary>
        /// Unreads the notification count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public int unreadNotificationCount(string userId)
        {
            return _notification.unreadNotificationCount(userId);
        }
    }
}
