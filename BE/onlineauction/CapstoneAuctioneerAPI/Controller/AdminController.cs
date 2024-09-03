using DataAccess.DTO;
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
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        [Route("ListAuctionAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListAuction(int status)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var auctioneerDetails = await _adminService.ListAuction(userId, status);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(auctioneerDetails);
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
        [HttpGet]
        [Route("ListAuctionCategoryAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListYourAuctioneerCategoryAdmin(int status, int category)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var auctioneerDetails = await _adminService.ListYourAuctioneerCategoryAdmin(userId, status, category);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(auctioneerDetails);
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
        [HttpGet]
        [Route("SearchAuctioneerAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> SearchAuctioneerAdmin(string content)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var auctioneerDetails = await _adminService.SearchAuctioneerAdmin(userId, content);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(auctioneerDetails);
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
        [HttpGet]
        [Route("auctiondetail")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AuctioneerDetail(int id)
        {
            try
            {
                var result = await _adminService.AuctioneerDetail(id);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("inforuser")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> inforuser(string id)
        {
            try
            {
                var result = await _adminService.ProfileUser(id);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("listinforuser")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> listinforuser()
        {
            try
            {
                var result = await _adminService.ListAccount();
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("AddCategory")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AddCategory(string name)
        {
            try
            {
                var result = await _adminService.AddCategory(name);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("ApproveorRejectAuction")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AcceptAuction(AcceptAutioneerDTO autioneer)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                var result = await _adminService.AcceptAuctioneerForAdmin(autioneer, userId);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("UpdateCategory")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AcceptAuction(int id, string Namecategory)
        {
            try
            {
                var result = await _adminService.UpdateCategoryAsync(id, Namecategory);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("DeleteCategory")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _adminService.DeleteCategoryAsync(id);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
