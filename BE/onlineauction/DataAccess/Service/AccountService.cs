using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IAccountRepository _accountRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Logins the service.
        /// </summary>
        /// <param name="sign">The sign.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> loginService(Login sign)
        {
            var login = await _accountRepository.LoginAsync(sign);
            return login;
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> RegisterService(AddAccountDTO account)
        {
            var register = await _accountRepository.MakeUSERAsync(account);
            return register;

        }
        /// <summary>
        /// Changes the pass word asynchronous.
        /// </summary>
        /// <param name="changepassDTO">The changepass dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ChangePassWordAsync(string userId, ChangepassDTO changepassDTO)
        {
            var changepassword = await _accountRepository.ChangePassWord(userId, changepassDTO);
            return changepassword;
        }
        /// <summary>
        /// Profiles the user asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ProfileUserAsync(string username)
        {
            var changepassword = await _accountRepository.ProfileUser(username);
            return changepassword;
        }
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ForgotPassword(string username)
        {
            var forgotpassword = await _accountRepository.ForgotPassword(username);
            return forgotpassword;
        }
        /// <summary>
        /// Resets the password asynchronous.
        /// </summary>
        /// <param name="resetPasswordDTO">The reset password dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var resetpasword = await _accountRepository.ResetPasswordAsync(resetPasswordDTO);
            return resetpasword;
        }
        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateUserProfile(string userid, UProfileDTO uProfileDTO)
        {
            var updateProfile = await _accountRepository.UpdateUserProfile(userid,uProfileDTO);
            return updateProfile;
        }
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AddInformation(string userid, AddInforUserDTO uProfileDTO)
        {
            var updateProfile = await _accountRepository.AddInformation(userid, uProfileDTO);
            return updateProfile;
        }
        /// <summary>
        /// Makes the adminsync.
        /// </summary>
        /// <param name="updatePermissionDTO">The update permission dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO)
        {
            var register = await _accountRepository.MakeAdminsync(updatePermissionDTO);
            return register;
        }
        /// <summary>
        /// Lists the account.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO> ListAccount()
        {
            var listAccount = await _accountRepository.ListAccount();
            return listAccount;
        }
        /// <summary>
        /// Locks the account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> LockAccount(string accountID)
        {
            var lockaccount = await _accountRepository.LockAccount(accountID);
            return lockaccount;
        }
        /// <summary>
        /// Uns the lock account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UnLockAccount(string accountID)
        {
            var lockaccount = await _accountRepository.UnLockAccount(accountID);
            return lockaccount;
        }

        public async Task<ResponseDTO> VerifyOtp(VerifyOtpViewModel model)
        {
            var result = await _accountRepository.VerifyOtp(model);
            return result;
        }

        public string GenerateNewJsonWebToken(string email, string role)
        {
            var result = _accountRepository.GenerateJwtToken(email, role);
            return result;
        }
    }
}
