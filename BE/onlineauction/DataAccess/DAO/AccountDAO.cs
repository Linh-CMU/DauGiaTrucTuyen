using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AccountDAO
    {
        private static AccountDAO _instance = null;
        private static readonly object _instanceLock = new object();

        private AccountDAO() { }

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


    }
}
