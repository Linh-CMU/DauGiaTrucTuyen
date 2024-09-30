using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/auction")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        /// <summary>
        /// The auction service
        /// </summary>
        private readonly AuctionService _auctionService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionController"/> class.
        /// </summary>
        /// <param name="auctionService">The auction service.</param>
        public AuctionController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("auctionDetailforuser")]
        public async Task<IActionResult> AuctionDetail(int id)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = await _auctionService.AuctionDetail(id);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(1000); // Gửi dữ liệu mỗi 1 giây
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
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet("listAuctioneerforuser")]
        [Produces("application/json; charset=utf-8")]
        public async Task<IActionResult> ListAuctioneer(int status)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    try
                    {
                        while (webSocket.State == WebSocketState.Open)
                        {
                            var AuctionDetails = await _auctionService.ListAuctioneer(status);

                            if (AuctionDetails == null)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "No data found", CancellationToken.None);
                                return new EmptyResult();
                            }

                            var options = new JsonSerializerOptions
                            {
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
                            };
                            string jsonString = JsonSerializer.Serialize(AuctionDetails, options);

                            // Verify JSON serialization
                            Console.WriteLine($"Serialized JSON: {jsonString}");

                            var bytes = Encoding.UTF8.GetBytes(jsonString);
                            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                            await Task.Delay(1000); // Send data every 1 second
                        }

                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
                        return new EmptyResult(); // Close WebSocket
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Console.WriteLine($"Error: {ex.Message}");
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Error occurred", CancellationToken.None);
                        return new EmptyResult(); // Close WebSocket
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new BadRequestResult(); // Return bad request status code
            }
        }

        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet("listAuctioneerflCategoryuser")]
        public async Task<IActionResult> AuctioneerFlCategory(int category, int status)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = await _auctionService.AuctioneerFlCategory(category, status);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(1000); // Gửi dữ liệu mỗi 1 giây
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
        /// <summary>
        /// Searchs the auctioneeryuser.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpGet("searchAuctioneeryuser")]
        public async Task<IActionResult> searchAuctioneeryuser(string content)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = await _auctionService.SearchAuctioneer(content);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(1000); // Gửi dữ liệu mỗi 1 giây
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
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        [HttpGet("listofregisteredbidders")]
        public async Task<IActionResult> Listofregisteredbidders(string userid, int status, bool? statusauction)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = await _auctionService.Listofregisteredbidders(userid, status, statusauction);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(1000); // Gửi dữ liệu mỗi 1 giây
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
