﻿using BusinessObject.Model;
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
        public async Task<ActionResult> AcceptAuction(AcceptAutioneerDTO autioneer)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _adminService.AcceptAuctioneerForAdmin(autioneer, userId);
                if (autioneer.Status == true)
                {
                    var results = await _adminService.AuctionDetailBatchJob(autioneer.AutioneerID);
                    var enddate = ConvertToDateTime(results.EndDay, results.EndTime);
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
        [HttpPut]
        [Route("reUpAuction")]
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
        /// Converts to date time.
        /// </summary>
        /// <param name="endDay">The end day.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException">Định dạng EndDay hoặc EndTime không hợp lệ.</exception>
        private DateTime ConvertToDateTime(string endDay, string endTime)
        {
            string combinedDateTime = $"{endDay} {endTime}";

            if (DateTime.TryParseExact(combinedDateTime, "yyyy-MM-dd HH:mm:ss",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime endDateTime))
            {
                return endDateTime;
            }
            else
            {
                throw new FormatException("Định dạng EndDay hoặc EndTime không hợp lệ.");
            }
        }
    }
}
