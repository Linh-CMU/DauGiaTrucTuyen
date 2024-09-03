using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Service
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ResponseDTO> loginService(Login sign)
        {
            var login = await _accountRepository.LoginAsync(sign);
            return login;
        }

        public async Task<ResponseDTO> RegisterService(AddAccountDTO account)
        {
            var register = await _accountRepository.MakeUSERAsync(account);
            return register;

        }
        public async Task<ResponseDTO> ChangePassWordAsync(ChangepassDTO changepassDTO)
        {
            var changepassword = await _accountRepository.ChangePassWord(changepassDTO);
            return changepassword;
        }
        public async Task<ResponseDTO> ProfileUserAsync(string username)
        {
            var changepassword = await _accountRepository.ProfileUser(username);
            return changepassword;
        }
        public async Task<ResponseDTO> ForgotPassword(string username)
        {
            var forgotpassword = await _accountRepository.ForgotPassword(username);
            return forgotpassword;
        }
        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var resetpasword = await _accountRepository.ResetPasswordAsync(resetPasswordDTO);
            return resetpasword;
        }
        public async Task<ResponseDTO> UpdateUserProfile(string userid, UProfileDTO uProfileDTO)
        {
            var updateProfile = await _accountRepository.UpdateUserProfile(userid,uProfileDTO);
            return updateProfile;
        }
        public async Task<ResponseDTO> AddInformation(string userid, AddInformationDTO uProfileDTO)
        {
            var updateProfile = await _accountRepository.AddInformation(userid, uProfileDTO);
            return updateProfile;
        }
        public async Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO)
        {
            var register = await _accountRepository.MakeAdminsync(updatePermissionDTO);
            return register;
        }
        public async Task<ResponseDTO> ListAccount()
        {
            var listAccount = await _accountRepository.ListAccount();
            return listAccount;
        }
        public async Task<ResponseDTO> LockAccount(string accountID)
        {
            var lockaccount = await _accountRepository.LockAccount(accountID);
            return lockaccount;
        }
        public async Task<ResponseDTO> UnLockAccount(string accountID)
        {
            var lockaccount = await _accountRepository.UnLockAccount(accountID);
            return lockaccount;
        }
    }
}
