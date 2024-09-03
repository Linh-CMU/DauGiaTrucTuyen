using Azure;
using DataAccess;
using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Authorize(Policy = "USER")]
        [Route("addAuctionItem")]
        public async Task<ActionResult> AddAuctionner(
            IFormFile imageAuction,
            string nameAuctioneer,
            string description,
            decimal startingPrice,
            int categoryID,
            string startDay,
            string startTime,
            string endDay,
            string endTime,
            decimal priceStep,
            IFormFile fileOfAuction,
            IFormFile signatureImg,
            IFormFile imageVerification
            )
        {
            var register = new RegisterAuctioneerDTO
            {
                Image = imageAuction,
                NameAuctioneer = nameAuctioneer,
                Description = description,
                StartingPrice = startingPrice,
                CategoryID = categoryID,
                StartDay = startDay,
                StartTime = startTime,
                EndDay = endDay,
                EndTime = endTime,
                PriceStep = priceStep,
                file = fileOfAuction,
                signatureImg = signatureImg,
                image = imageVerification
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var result = await _userService.RegiterAuctioneer(userId, register);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpPut]
        [Authorize(Policy = "USER")]
        [Route("UpdateAuctionItem")]
        public async Task<ActionResult> UpdateAuctionner(
            int auctionID,
            IFormFile imageAuction,
            string nameAuctionIten,
            string description,
            decimal startingPrice
            )
        {
            var update = new UDAuctionDTO
            {
                AuctionID = auctionID,
                Image= imageAuction,
                NameAuctioneer = nameAuctionIten,
                Description = description,
                StartingPrice = startingPrice
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var result = await _userService.UpdateAuctioneer(userId, update);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = "USER")]
        [Route("DeleteAuctionner")]
        public async Task<ActionResult> DeleteAuctionner(int id)
        {
            var result = await _userService.DeleteAuctioneer(id);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
