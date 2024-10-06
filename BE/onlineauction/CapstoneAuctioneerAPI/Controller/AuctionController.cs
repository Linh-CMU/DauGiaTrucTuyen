using BusinessObject.Model;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;
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
            try
            {
                var result = await _auctionService.AuctionDetail(id);
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
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet("listAuctioneerforuser")]
        public async Task<IActionResult> ListAuctioneer(int status)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _auctionService.ListAuctioneer(status, userId);
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
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet("listAuctioneerflCategoryuser")]
        public async Task<IActionResult> AuctioneerFlCategory(int category, int status)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _auctionService.AuctioneerFlCategory(category, status, userId);
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
        /// Searchs the auctioneeryuser.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        [HttpGet("searchAuctioneeryuser")]
        public async Task<IActionResult> searchAuctioneeryuser(string content)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _auctionService.SearchAuctioneer(content, userId);
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
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        [HttpGet("listofregisteredbidders")]
        [Authorize(Policy = "USER")]
        public async Task<IActionResult> Listofregisteredbidders(int status, bool? statusauction)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _auctionService.Listofregisteredbidders(userId, status, statusauction);
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
