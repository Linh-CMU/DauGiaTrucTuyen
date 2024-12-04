using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="loginDTO">The login dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> LoginAsync(Login loginDTO, bool google = false);
        /// <summary>
        /// Makes the user asynchronous.
        /// </summary>
        /// <param name="updatePermissionDTO">The update permission dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> MakeUSERAsync(AddAccountDTO updatePermissionDTO);
        /// <summary>
        /// Makes the adminsync.
        /// </summary>
        /// <param name="updatePermissionDTO">The update permission dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO);
        /// <summary>
        /// Changes the pass word.
        /// </summary>
        /// <param name="changepassDTO">The changepass dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> ChangePassWord(string userId, ChangepassDTO changepassDTO);
        /// <summary>
        /// Profiles the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Task<ResponseDTO> ProfileUser(string username);
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Task<ResponseDTO> ForgotPassword(string username);
        /// <summary>
        /// Resets the password asynchronous.
        /// </summary>
        /// <param name="resetPasswordDTO">The reset password dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> UpdateUserProfile(string userID, UProfileDTO uProfileDTO);
        /// <summary>
        /// Lists the account.
        /// </summary>
        /// <returns></returns>
        Task<ResponseDTO> ListAccount();
        /// <summary>
        /// Locks the account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> LockAccount(string accountID);
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> AddInformation(string userID, AddInforUserDTO uProfileDTO);
        /// <summary>
        /// Uns the lock account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> UnLockAccount(string accountID);

        Task<ResponseDTO> VerifyOtp(VerifyOtpViewModel model);
        string GenerateJwtToken(string email, string role);
        Task<bool> checkLoginEmail(string email);
        Task<ResponseDTO> CreateGoogle(AddAccountDTO account);
    }
}
