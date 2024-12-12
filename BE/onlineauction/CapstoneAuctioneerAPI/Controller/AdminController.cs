using BusinessObject.Model;
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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// The admin service
        /// </summary>
        private readonly AdminService _adminService;
        /// <summary>
        /// The batch service
        /// </summary>
        private readonly BatchService _batchService;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController" /> class.
        /// </summary>
        /// <param name="adminService">The admin service.</param>
        /// <param name="batchService">The batch service.</param>
        public AdminController(AdminService adminService, BatchService batchService)
        {
            _adminService = adminService;
            _batchService = batchService;
        }
        /// <summary>
        /// Lists the auction.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListAuctionAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListAuction(int status)
        {
            
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _adminService.ListAuction(userId, status);
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
        /// <summary>
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ListAuctionCategoryAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListYourAuctioneerCategoryAdmin(int status, int category)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _adminService.ListYourAuctioneerCategoryAdmin(userId, status, category);
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
        /// <summary>
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchAuctioneerAdmin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> SearchAuctioneerAdmin(string content)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _adminService.SearchAuctioneerAdmin(userId, content);
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
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("auctiondetail")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AuctionDetail(int id)
        {
            try
            {
                var result = await _adminService.AuctionDetail(id);
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
        /// <summary>
        /// Inforusers the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Listinforusers this instance.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Lists the auctioneer by user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("listAuctioneerByUser")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListAuctioneerByUser(string iduser, int status)
        {
            try
            {
                var result = await _adminService.ListAuctioneerByUser(iduser, status);
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
        [Route("ListAuctioneerRegisterByUser")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListAuctioneerRegisterByUser(string iduser, int status)
        {
            try
            {
                var result = await _adminService.ListAuctioneerRegisterByUser(iduser, status);
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
        [Route("auctionRoom")]
        public async Task<ActionResult> auctionRoom(int auctionId)
        {
            try
            {
                var result = await _adminService.AuctionRoomAdmin(auctionId);
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
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Accepts the auction.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ApproveorRejectAuction")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> AcceptAuction([FromForm] AcceptAutioneerDTO autioneer)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _adminService.AcceptAuctioneerForAdmin(autioneer, userId);
                if (autioneer.Status == true)
                {
                    var results = await _adminService.AuctionDetailBatchJob(autioneer.AutioneerID);
                    var enddate = ConvertToDateTime(results.EndDay, results.EndTime, results.TimePerLap);
                    _batchService.CreateAuction(results.ID, enddate);
                }
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
        /// <summary>
        /// Accepts the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Namecategory">The namecategory.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
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
        /// <summary>
        /// Res up auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("reUpAuction")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ReUpAuction(int id)
        {
            try
            {
                var result = await _adminService.ReUpAuction(id);
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
        /// <summary>
        /// Lists the user in auction.
        /// </summary>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("listBidderInAuction")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> ListUserInAuction(int auctionId)
        {
            try
            {
                var result = await _adminService.listBidderInAuction(auctionId);
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
        [Route("product-statistics")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> Productstatistics()
        {
            try
            {
                var result = await _adminService.Productstatistics();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("money-statistics")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> MonthlyIncomeStatistics()
        {
            try
            {
                var result = await _adminService.MonthlyIncomeStatistics();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("total")]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult> TotalAsync()
        {
            try
            {
                var result = await _adminService.TotalAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Converts to date time.
        /// </summary>
        /// <param name="endDay">The end day.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Định dạng EndDay hoặc EndTime không hợp lệ.</exception>
        private DateTime ConvertToDateTime(string endDay, string endTime, string timePerLap)
        {
            try
            {
                string combinedDateTime = $"{endDay} {endTime}";

                if (DateTime.TryParseExact(combinedDateTime, "dd/MM/yyyy HH:mm",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None, out DateTime endDateTime))
                {
                    if (TimeSpan.TryParseExact(timePerLap, @"hh\:mm", System.Globalization.CultureInfo.InvariantCulture, out TimeSpan timePerLapSpan))
                    {
                        return endDateTime.Add(timePerLapSpan);
                    }
                    else
                    {
                        throw new FormatException("Định dạng TimePerLap không hợp lệ.");
                    }
                }
                else
                {
                    throw new FormatException("Định dạng EndDay hoặc EndTime không hợp lệ.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chuyển đổi thời gian: {ex.Message}");
            }
        }

    }
}
