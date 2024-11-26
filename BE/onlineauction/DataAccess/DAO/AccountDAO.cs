using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static AccountDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="AccountDAO"/> class from being created.
        /// </summary>
        private AccountDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static AccountDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AccountDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>
        /// Adds the account detail asynchronous.
        /// </summary>
        /// <param name="accountDetail">The account detail.</param>
        /// <returns></returns>
        public async Task<bool> AddAccountDetailAsync(AccountDetail accountDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.AccountDetails.Add(accountDetail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle exception
                return false;
            }
        }
        public async Task<bool> RemoveAccount(string userId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var user = context.AccountDetails.FirstOrDefault(u => u.AccountID == userId);
                    context.AccountDetails.Remove(user);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle exception
                return false;
            }
        }
        /// <summary>
        /// Profiles the DAO.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<AccountDetail> ProfileDAO(string accountID)
        {
            AccountDetail accountDetail = null;
            try
            {
                using (var context = new ConnectDB())
                {
                    accountDetail = await context.AccountDetails.FirstOrDefaultAsync(a => a.AccountID == accountID);
                    return accountDetail;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates the account detail.
        /// </summary>
        /// <param name="accountDetail">The account detail.</param>
        /// <exception cref="System.Exception">
        /// Account detail not found.
        /// or
        /// An error occurred while updating the account detail: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task UpdateAccountDetail(AccountDetail accountDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAccountDetail = await context.AccountDetails
                        .FirstOrDefaultAsync(ad => ad.AccountID == accountDetail.AccountID);

                    if (existingAccountDetail == null)
                    {
                        throw new Exception("Account detail not found.");
                    }

                    // Cập nhật các thông tin của tài khoản
                    existingAccountDetail.Avatar = accountDetail.Avatar;
                    existingAccountDetail.FullName = accountDetail.FullName;
                    existingAccountDetail.Phone = accountDetail.Phone;
                    existingAccountDetail.FrontCCCD = accountDetail.FrontCCCD;
                    existingAccountDetail.BacksideCCCD = accountDetail.BacksideCCCD;
                    existingAccountDetail.City = accountDetail.City;
                    existingAccountDetail.Ward = accountDetail.Ward;
                    existingAccountDetail.District = accountDetail.District;
                    existingAccountDetail.Address = accountDetail.Address;
                    existingAccountDetail.Birthdate = accountDetail.Birthdate;
                    existingAccountDetail.Gender = accountDetail.Gender;
                    existingAccountDetail.PlaceOfResidence = accountDetail.PlaceOfResidence;
                    existingAccountDetail.PlaceOfIssue = accountDetail.PlaceOfIssue;
                    existingAccountDetail.DateOfIssue = accountDetail.DateOfIssue;


                    // Đánh dấu entity là đã sửa đổi và lưu các thay đổi
                    context.Entry(existingAccountDetail).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Ghi log lỗi chi tiết hoặc xử lý theo cách bạn muốn
                throw new Exception($"An error occurred while updating the account detail: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác (không phải DbUpdateException)
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        public async Task StoreOtpForUser(Account user, string otp, DateTime expirationTime)
        {
            using (var context = new ConnectDB())
            {
                var otpRecord = new UserOtp
                {
                    UserId = user.Id,
                    Otp = otp,
                    ExpirationTime = expirationTime,
                    Attempts = 0
                };

                context.UserOtp.Add(otpRecord);
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> VerifyOtp(VerifyOtpViewModel model)
        {
            try
            {
                using(var _context = new ConnectDB())
                {
                    var user = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);
                    var userdetail = await _context.AccountDetails.FirstOrDefaultAsync(a => a.AccountID == user.Id);
                    var otpRecord = await _context.UserOtp
                        .FirstOrDefaultAsync(otp => otp.UserId == user.Id && otp.Otp == model.Otp);
                    if (otpRecord == null)
                    {
                        return false;
                    }
                    if (otpRecord.ExpirationTime < DateTime.UtcNow)
                    {
                        _context.AccountDetails.Remove(userdetail);
                        _context.UserOtp.Remove(otpRecord);
                        _context.Accounts.Remove(user);
                        await _context.SaveChangesAsync();
                        return false;
                    }
                    if (otpRecord.Attempts >= 5)
                    {
                        // You can either lock the account or delete the user
                        _context.AccountDetails.Remove(userdetail);
                        _context.UserOtp.Remove(otpRecord);
                        _context.Accounts.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                    if (user != null)
                    {
                        user.EmailConfirmed = true; // Mark the email as confirmed
                        user.Status = false;
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    otpRecord.Attempts++;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
