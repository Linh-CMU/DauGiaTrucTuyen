using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAccountRepository
    {
        Task<ResponseDTO> LoginAsync(Login loginDTO);
        Task<ResponseDTO> MakeUSERAsync(AddAccountDTO updatePermissionDTO);
        Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO);
        Task<ResponseDTO> ChangePassWord(ChangepassDTO changepassDTO);
        Task<ResponseDTO> ProfileUser(string username);
        Task<ResponseDTO> ForgotPassword(string username);
        Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<ResponseDTO> UpdateUserProfile(string userID, UProfileDTO uProfileDTO);
        Task<ResponseDTO> ListAccount();
        Task<ResponseDTO> LockAccount(string accountID);
        Task<ResponseDTO> AddInformation(string userID, AddInformationDTO uProfileDTO);
        Task<ResponseDTO> UnLockAccount(string accountID);
    }
}
