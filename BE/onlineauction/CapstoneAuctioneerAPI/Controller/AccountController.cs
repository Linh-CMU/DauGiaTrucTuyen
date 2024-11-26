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
using Microsoft.AspNetCore.Identity;
using BusinessObject.Model;
using Google.Apis.Auth;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.OData.Routing.Controllers.ODataController" />
    [Route("api")]
    [ApiController]
    public class AccountController : ODataController
    {
        /// <summary>
        /// The account service
        /// </summary>
        private readonly AccountService _accountService;
        private readonly UserManager<Account> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">The account service.</param>
        public AccountController(AccountService accountService, UserManager<Account> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }
        /// <summary>
        /// Logins the specified login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns></returns>
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

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.TokenId, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { "800544947907-gqq1fut5e84qhsdtapqs1nf1f3rao28r.apps.googleusercontent.com" }
                });

                // Tạo JWT Token
                var role = "user";
                var token = _accountService.GenerateNewJsonWebToken(payload.Email, role);

                //var check = new
                //{
                //    Role = role,
                //    Token = token,
                //    Check = user.Result.BacksideCCCD == null ? false : true,
                //};
                //return new ResponseDTO() { Result = check, IsSucceed = true, Message = "Successfully" };
                return Ok(new
                {
                    token,
                    user = new
                    {
                        payload.Email,
                        payload.Name,
                        payload.Picture
                    }
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = "Invalid Google Token", Error = ex.Message });
            }
        }

        /// <summary>
        /// Registers the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
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
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResponseDTO() { IsSucceed = false, Message = "Internal server error: " + ex.Message });
            }
        }
        [HttpPost]
        [Route("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            try
            {
                var result = await _accountService.VerifyOtp(model);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResponseDTO() { IsSucceed = false, Message = "Internal server error: " + ex.Message });
            }
        }
        /// <summary>
        /// Makes the adminsync.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="changepassDTO">The changepass dto.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("UserOrAdmin/changepassword")]
        public async Task<ActionResult> ChangePassword(ChangepassDTO changepassDTO)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _accountService.ChangePassWordAsync(userId ,changepassDTO);
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
        /// Profiles the user.
        /// </summary>
        /// <returns></returns>
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
        [Authorize]
        [Route("Admin/userDetail")]
        public async Task<ActionResult> UserDetail(string uid)
        {
            try
            {
                var result = await _accountService.ProfileUserAsync(uid);
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
        /// Forgots the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPasswordDTO">The reset password dto.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="frontCCCD">The front CCCD.</param>
        /// <param name="backsideCCCD">The backside CCCD.</param>
        /// <param name="city">The city.</param>
        /// <param name="ward">The ward.</param>
        /// <param name="district">The district.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        [HttpPut("UserOrAdmin/addInformation")]
        [Authorize]
        public async Task<IActionResult> AddInformation(
           [FromForm] AddInforUserDTO addInforUserDTO
            )
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            var response = await _accountService.AddInformation(userId, addInforUserDTO);

            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="city">The city.</param>
        /// <param name="ward">The ward.</param>
        /// <param name="district">The district.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        [HttpPut("UserOrAdmin/update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] UProfileDTO uProfileDTO)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            var response = await _accountService.UpdateUserProfile(userId, uProfileDTO);

            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Lockaccounts the specified account identifier.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Unlockaccounts the specified account identifier.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Checks the token expiration.
        /// </summary>
        /// <returns></returns>
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
