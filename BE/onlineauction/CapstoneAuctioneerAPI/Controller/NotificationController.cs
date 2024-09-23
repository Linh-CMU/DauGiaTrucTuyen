using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        /// <summary>
        /// The notification
        /// </summary>
        private readonly NotificationService _notification;
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public NotificationController(NotificationService notification)
        {
            _notification = notification;
        }
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("listNotification")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _notification.ListNotificationAsync(uid);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result); // Return 400 with the error message
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        /// <summary>
        /// Unreads the notification count.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("unreadNotificationCount")]
        public async Task<IActionResult> unreadNotificationCount(string uid)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = _notification.unreadNotificationCount(uid);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(3000); // Gửi dữ liệu mỗi 3 giây
                    }

                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
                    return new EmptyResult(); // Kết thúc WebSocket
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new BadRequestResult(); // Trả về mã trạng thái lỗi nếu không phải yêu cầu WebSocket
            }
        }
    }
}
