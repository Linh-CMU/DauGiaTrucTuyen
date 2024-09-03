using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ConnectDB _context;
        private readonly UserManager<Account> _accountManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUploadRepository _upload;
        private readonly IUploadRepository _uploadRepository;

        public AccountRepository(
            ConnectDB context,
            UserManager<Account> accountManager,
            RoleManager<IdentityRole> roleManager,
            IUploadRepository uploadRepository,
            IUploadRepository upload,
            IConfiguration configuration)
        {
            _context = context;
            _accountManager = accountManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _uploadRepository = uploadRepository;
            _upload = upload;
        }

        public async Task<ResponseDTO> LoginAsync(Login loginDTO)
        {
            Account account = null;

            if (loginDTO.username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(loginDTO.username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(loginDTO.username);
            }

            if (account == null || !await _accountManager.CheckPasswordAsync(account, loginDTO.password))
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Invalid credentials" };
            }
            if (account.Status == true)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account had lock" };
            }

            var userRoles = await _accountManager.GetRolesAsync(account);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims, TimeSpan.FromDays(1));
            return new ResponseDTO() { IsSucceed = true, Message = token };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims, TimeSpan expiresIn)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.Add(expiresIn),
                claims: claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        public async Task<ResponseDTO> MakeUSERAsync(AddAccountDTO account)
        {
            var isExistEmail = await _accountManager.FindByEmailAsync(account.Email);
            var isExistUser = await _accountManager.FindByNameAsync(account.UserName);


            if (isExistUser != null || isExistEmail != null)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "You already have an account" };
            }

            var createAccount = new Account
            {
                UserName = account.UserName,
                Email = account.Email,
                Warning = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
                Status = false,
            };

            var createUserResult = await _accountManager.CreateAsync(createAccount, account.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = string.Join(" ", createUserResult.Errors.Select(e => e.Description));
                return new ResponseDTO() { IsSucceed = false, Message = "User creation failed because: " + errorString };
            }

            var createAccountDetail = new AccountDetail
            {
                AccountID = createAccount.Id,
            };

            var createAccountDetailResult = await AccountDAO.Instance.AddAccountDetailAsync(createAccountDetail);

            if (!createAccountDetailResult)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account detail creation failed" };
            }

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.USER);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> ChangePassWord(ChangepassDTO changepassDTO)
        {
            Account account = null;

            if (changepassDTO.username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(changepassDTO.username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(changepassDTO.username);
            }
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }
            var isPasswordValid = await _accountManager.CheckPasswordAsync(account, changepassDTO.oldpassword);
            if (!isPasswordValid)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Old password is incorrect" };
            }
            var changePasswordResult = await _accountManager.ChangePasswordAsync(account, changepassDTO.oldpassword, changepassDTO.newpassword);
            if (!changePasswordResult.Succeeded)
            {
                var errorString = string.Join(" ", changePasswordResult.Errors.Select(e => e.Description));
                return new ResponseDTO { IsSucceed = false, Message = "Password change failed because: " + errorString };
            }
            return new ResponseDTO { IsSucceed = true, Message = "Password changed successfully" };
        }

        public async Task<ResponseDTO> ProfileUser(string username)
        {
            ProfileDTO profileDTO = null;
            Account account = null;
            AccountDetail accountDetail = null;
            account = await _accountManager.FindByIdAsync(username);
            var roles = await _accountManager.GetRolesAsync(account);
            var role = roles.FirstOrDefault(); // Get the first role
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            accountDetail = await AccountDAO.Instance.ProfileDAO(account.Id);
            if (accountDetail == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account details not found" };
            }
            profileDTO = new ProfileDTO
            {
                AccountId = account.Id,
                UserName = account.UserName,
                Avatar = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.Avatar}",
                FrontCCCD = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.FrontCCCD}",
                BacksideCCCD = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.BacksideCCCD}",
                Email = account.Email,
                FullName = accountDetail.FullName,
                Phone = accountDetail.Phone,
                City = accountDetail.City,
                Ward = accountDetail.Ward,
                District = accountDetail.District,
                Address = accountDetail.Address,
                Warning = account.Warning,
                Status = account.Status,
                Role = role
            };
            return new ResponseDTO { Result = profileDTO, IsSucceed = true, Message = "Successfully" };
        }
        private string GetResetPasswordEmailContent(string resetLink)
        {
            string emailContent = @"<!DOCTYPE html>
                            <html lang='en'>
                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                <title>Reset Password</title>
                            </head>
                            <body>
                                <p>Dear User,</p>
                                <p>You recently requested to reset your password. Please click the link below to reset your password:</p>
                                <p>Enter the link to change your password <a href='" + resetLink + @"'>Link</a></p>
                                <p>If you did not request a password reset, please ignore this email. Your password will remain unchanged.</p>
                                <p>Best regards,</p>
                                <p>YourApp Team</p>
                            </body>
                            </html>";
            return emailContent;
        }
        public async Task<ResponseDTO> ForgotPassword(string username)
        {
            // Tìm kiếm tài khoản theo email hoặc tên người dùng
            Account account = null;
            if (username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(username);
            }

            // Kiểm tra xem tài khoản có tồn tại không
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Tạo token để đặt lại mật khẩu
            var code = await _accountManager.GeneratePasswordResetTokenAsync(account);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Tạo đường link đặt lại mật khẩu
            var resetLink = $"https://www.hienmaugiotmauhong.online/resetpassword/{code}/{account.Email}";

            // Gửi email chứa đường link đặt lại mật khẩu
            await MailUtils.SendMailGoogleSmtp(
                fromEmail: "nguyenanh0978638@gmail.com",
                toEmail: account.Email,
                subject: "Forgot Password",
                body: GetResetPasswordEmailContent(resetLink),
                gmailSend: "nguyenanh0978638@gmail.com",
                gmailPassword: "zwlcvsnblwndpbpe"
            );

            return new ResponseDTO { IsSucceed = true, Message = "Email sent successfully." };
        }

        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            // Tìm kiếm tài khoản theo email hoặc tên người dùng
            Account account = null;
            if (resetPasswordDTO.UsernameOrEmail.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(resetPasswordDTO.UsernameOrEmail);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(resetPasswordDTO.UsernameOrEmail);
            }

            // Kiểm tra xem tài khoản có tồn tại không
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Thực hiện đặt lại mật khẩu bằng token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDTO.ResetToken));
            var resetPasswordResult = await _accountManager.ResetPasswordAsync(account, decodedToken, resetPasswordDTO.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                var errorString = string.Join(" ", resetPasswordResult.Errors.Select(e => e.Description));
                return new ResponseDTO { IsSucceed = false, Message = "Password reset failed because: " + errorString };
            }

            return new ResponseDTO { IsSucceed = true, Message = "Password reset successfully" };
        }
        public async Task<ResponseDTO> AddInformation(string userID, AddInformationDTO uProfileDTO)
        {
            var account = await AccountDAO.Instance.ProfileDAO(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            var accountDetail = new AccountDetail
            {
                AccountID = userID,
                FullName = uProfileDTO.FullName,
                Phone = uProfileDTO.Phone,
                City = uProfileDTO.City,
                Ward = uProfileDTO.Ward,
                District = uProfileDTO.District,
                Address = uProfileDTO.Address,
                Avatar = uProfileDTO.Avatar != null ? await _upload.SaveFileAsync(uProfileDTO.Avatar, "avatars", userID) : account.Avatar,
                FrontCCCD = uProfileDTO.FrontCCCD != null ? await _upload.SaveFileAsync(uProfileDTO.FrontCCCD, "cccd/front", userID) : account.FrontCCCD,
                BacksideCCCD = uProfileDTO.BacksideCCCD != null ? await _upload.SaveFileAsync(uProfileDTO.BacksideCCCD, "cccd/back", userID) : account.BacksideCCCD
            };

            try
            {
                await AccountDAO.Instance.UpdateAccountDetail(accountDetail);
                return new ResponseDTO { IsSucceed = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Profile update failed: " + ex.Message };
            }
        }
        public async Task<ResponseDTO> UpdateUserProfile(string userID, UProfileDTO uProfileDTO)
        {
            var account = await AccountDAO.Instance.ProfileDAO(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            var accountDetail = new AccountDetail
            {
                AccountID = userID,
                FullName = uProfileDTO.FullName,
                Phone = uProfileDTO.Phone,
                City = uProfileDTO.City,
                Ward = uProfileDTO.Ward,
                District = uProfileDTO.District,
                Address = uProfileDTO.Address,
                Avatar = uProfileDTO.Avatar != null ? await _upload.SaveFileAsync(uProfileDTO.Avatar, "avatars", userID) : account.Avatar
            };

            try
            {
                await AccountDAO.Instance.UpdateAccountDetail(accountDetail);
                return new ResponseDTO { IsSucceed = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Profile update failed: " + ex.Message };
            }
        }



        public async Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO)
        {
            var isExistEmail = await _accountManager.FindByEmailAsync(updatePermissionDTO.Email);
            var isExistUser = await _accountManager.FindByNameAsync(updatePermissionDTO.UserName);


            if (isExistUser != null || isExistEmail != null)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "You already have an account" };
            }

            var createAccount = new Account
            {
                UserName = updatePermissionDTO.UserName,
                Email = updatePermissionDTO.Email,
                Warning = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
                Status = false
            };

            var createUserResult = await _accountManager.CreateAsync(createAccount, updatePermissionDTO.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = string.Join(" ", createUserResult.Errors.Select(e => e.Description));
                return new ResponseDTO() { IsSucceed = false, Message = "User creation failed because: " + errorString };
            }

            var createAccountDetail = new AccountDetail
            {
                AccountID = createAccount.Id
            };

            var createAccountDetailResult = await AccountDAO.Instance.AddAccountDetailAsync(createAccountDetail);

            if (!createAccountDetailResult)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account detail creation failed" };
            }

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.ADMIN);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> ListAccount()
        {
            try
            {
                // Lấy danh sách tài khoản và chi tiết tài khoản
                var accounts = await (from acc in _accountManager.Users.AsNoTracking()
                                      join accDetail in _context.AccountDetails.AsNoTracking() on acc.Id equals accDetail.AccountID
                                      select new
                                      {
                                          acc,
                                          accDetail
                                      }).ToListAsync();

                if (accounts == null || !accounts.Any())
                {
                    return new ResponseDTO { IsSucceed = false, Message = "No accounts found" };
                }

                var accountList = new List<ProfileDTO>();

                foreach (var item in accounts)
                {
                    // Lấy danh sách vai trò của từng tài khoản
                    var roles = await _accountManager.GetRolesAsync(item.acc);

                    // Thêm thông tin tài khoản và vai trò vào danh sách kết quả
                    accountList.Add(new ProfileDTO
                    {
                        AccountId = item.acc.Id,
                        UserName = item.acc.UserName,
                        Email = item.acc.Email,
                        FullName = item.accDetail.FullName,
                        Phone = item.accDetail.Phone,
                        City = item.accDetail.City,
                        Ward = item.accDetail.Ward,
                        District = item.accDetail.District,
                        Address = item.accDetail.Address,
                        Avatar = item.accDetail.Avatar,
                        FrontCCCD = item.accDetail.FrontCCCD,
                        BacksideCCCD = item.accDetail.BacksideCCCD,
                        Warning = item.acc.Warning,
                        Status = item.acc.Status,
                        Role = string.Join(", ", roles) // Nối các vai trò thành chuỗi
                    });
                }

                return new ResponseDTO { IsSucceed = true, Result = accountList, Message = "Successfully retrieved accounts" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi tương ứng
                return new ResponseDTO { IsSucceed = false, Message = "Internal server error: " + ex.Message };
            }
        }


        public async Task<ResponseDTO> LockAccount(string accountID)
        {
            // Tìm tài khoản theo ID
            var account = await _accountManager.FindByIdAsync(accountID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Cập nhật trạng thái khóa của tài khoản
            account.Status = true; // Giả sử 'true' nghĩa là tài khoản bị khóa, nếu ngược lại, thay đổi cho phù hợp

            try
            {
                // Cập nhật tài khoản với trạng thái mới
                var result = await _accountManager.UpdateAsync(account);
                if (!result.Succeeded) // .Succeeded được sử dụng để kiểm tra kết quả của việc cập nhật
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Failed to update account status." };
                }
                return new ResponseDTO { IsSucceed = true, Message = "Account locked successfully." };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Account lock failed: " + ex.Message };
            }
        }
        public async Task<ResponseDTO> UnLockAccount(string accountID)
        {
            // Tìm tài khoản theo ID
            var account = await _accountManager.FindByIdAsync(accountID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Cập nhật trạng thái khóa của tài khoản
            account.Status = false; // Giả sử 'true' nghĩa là tài khoản bị khóa, nếu ngược lại, thay đổi cho phù hợp

            try
            {
                // Cập nhật tài khoản với trạng thái mới
                var result = await _accountManager.UpdateAsync(account);
                if (!result.Succeeded) // .Succeeded được sử dụng để kiểm tra kết quả của việc cập nhật
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Failed to update account status." };
                }
                return new ResponseDTO { IsSucceed = true, Message = "Account locked successfully." };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Account lock failed: " + ex.Message };
            }
        }
    }
}
