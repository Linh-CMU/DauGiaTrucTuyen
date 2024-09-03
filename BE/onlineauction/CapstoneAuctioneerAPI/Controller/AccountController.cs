using DataAccess.Service;
using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using System.IdentityModel.Tokens.Jwt;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api")]
    [ApiController]
    public class AccountController : ODataController
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        [Route("account/login")]
        public async Task<ActionResult> Login(Login login)
        {
            try
            {
                var result = await _accountService.loginService(login);
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

        [HttpPost]
        [Route("account/register")]
        public async Task<IActionResult> Register(AddAccountDTO account)
        {
            try
            {
                var result = await _accountService.RegisterService(account);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result); // Return 400 with the error message
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResponseDTO() { IsSucceed = false, Message = "Internal server error: " + ex.Message });
            }
        }
        [HttpPost]
        [Route("Admin/make-admin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> MakeAdminsync(AddAccountDTO account)
        {
            try
            {
                var result = await _accountService.MakeAdminsync(account);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result); // Return 400 with the error message
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResponseDTO() { IsSucceed = false, Message = "Internal server error: " + ex.Message });
            }
        }
        [HttpPut]
        [Authorize]
        [Route("UserOrAdmin/changepassword")]
        public async Task<ActionResult> ChangePassword(ChangepassDTO changepassDTO)
        {
            try
            {
                var result = await _accountService.ChangePassWordAsync(changepassDTO);
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
        [Authorize]
        [Route("UserOrAdmin/profile")]
        public async Task<ActionResult> ProfileUser()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
                var result = await _accountService.ProfileUserAsync(userId);
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
        [Route("account/forgot")]
        public async Task<ActionResult> ForgotPassword(string username)
        {
            try
            {
                var result = await _accountService.ForgotPassword(username);
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
        [Route("account/resetPass")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(resetPasswordDTO);
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
        [HttpPut("UserOrAdmin/addInformation")]
        [Authorize]
        public async Task<IActionResult> AddInformation(
            IFormFile? avatar,
            string? fullName,
            string? phone,
            IFormFile? frontCCCD,
            IFormFile? backsideCCCD,
            string? city,
            string? ward,
            string? district,
            string? address
            )
        {
            var uProfileDTO = new AddInformationDTO()
            {
                Avatar = avatar,
                FullName = fullName,
                Phone = phone,
                FrontCCCD = frontCCCD,
                BacksideCCCD = backsideCCCD,
                City = city,
                Ward = ward,
                District = district,
                Address = address
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            var response = await _accountService.AddInformation(userId, uProfileDTO);

            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpPut("UserOrAdmin/update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(
            IFormFile? avatar,
            string? fullName,
            string? phone,
            string? city,
            string? ward,
            string? district,
            string? address
            )
        {
            var uProfileDTO = new UProfileDTO()
            {
                Avatar = avatar,
                FullName = fullName,
                Phone = phone,
                City = city,
                Ward = ward,
                District = district,
                Address = address
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            var response = await _accountService.UpdateUserProfile(userId, uProfileDTO);

            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("Admin/listAccount")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _accountService.ListAccount();
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
        [HttpPut]
        [Route("Admin/lockaccount")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> lockaccount(string accountID)
        {
            try
            {
                var result = await _accountService.LockAccount(accountID);
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
        [HttpPut]
        [Route("Admin/unlockaccount")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> unlockaccount(string accountID)
        {
            try
            {
                var result = await _accountService.UnLockAccount(accountID);
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
        [HttpGet("check-token-expiration")]
        [Authorize]
        public IActionResult CheckTokenExpiration()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return BadRequest(new { Message = "Invalid token" });

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expClaim == null)
                return BadRequest(new { Message = "Expiration claim not found" });

            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
            var currentDate = DateTime.UtcNow;

            if (expDate.UtcDateTime.Date > currentDate.Date)
            {
                return Ok(new
                {
                    IsValid = true
                });
            }
            else
            {
                return Ok(new
                {
                    IsValid = false
                });
            }
        }

    }
}
