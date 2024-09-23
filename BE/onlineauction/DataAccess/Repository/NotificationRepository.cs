using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DataAccess.IRepository.INotificationRepository" />
    public class NotificationRepository : INotificationRepository
    {
        /// <summary>
        /// Adds the notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="uid">The uid.</param>
        public async Task AddNotification(AddNotificationDTO notification, string uid)
        {
            var notifi = new Notification
            {
                AccountID = uid,
                Title = notification.Title,
                Description = notification.Description,
                StatusNotification = false
            };
            await NotificationDAO.Instance.AddNotification(notifi);
        }

        /// <summary>
        /// Lists the notification asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListNotificationAsync(string userId)
        {
            try
            {
                var result = await NotificationDAO.Instance.ListNotificationAsync(userId);
                if (result == null)
                {
                    return new ResponseDTO() { IsSucceed = true, Message = "Not Found" };
                }
                else
                {
                    return new ResponseDTO() { IsSucceed = true, Result = result, Message = "Successfully" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { IsSucceed = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Unreads the notification count.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public int unreadNotificationCount(string userId)
        {
            return NotificationDAO.Instance.unreadNotificationCount(userId);
        }
    }
}
