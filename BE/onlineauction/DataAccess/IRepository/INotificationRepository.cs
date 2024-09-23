using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        Task AddNotification(AddNotificationDTO notification, string uid);
        /// <summary>
        /// Lists the notification asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> ListNotificationAsync(string userId);
        /// <summary>
        /// Unreads the notification count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        int unreadNotificationCount(string userId);
    }
}
