using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DataAccess.IRepository.IAccountRepository" />
    public class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly ConnectDB _context;
        /// <summary>
        /// The account manager
        /// </summary>
        private readonly UserManager<Account> _accountManager;
        /// <summary>
        /// The role manager
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// The upload
        /// </summary>
        private readonly IUploadRepository _upload;
        /// <summary>
        /// The upload repository
        /// </summary>
        private readonly IUploadRepository _uploadRepository;

        private readonly DigitalSignatureHelper _signatureHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="uploadRepository">The upload repository.</param>
        /// <param name="upload">The upload.</param>
        /// <param name="configuration">The configuration.</param>
        public AccountRepository(
            ConnectDB context,
            UserManager<Account> accountManager,
            RoleManager<IdentityRole> roleManager,
            IUploadRepository uploadRepository,
            IUploadRepository upload,
            DigitalSignatureHelper signatureHelper,
            IConfiguration configuration)
        {
            _context = context;
            _accountManager = accountManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _uploadRepository = uploadRepository;
            _upload = upload;
            _signatureHelper = signatureHelper;
        }

        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <param name="loginDTO">The login dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> LoginAsync(Login loginDTO, bool google = false)
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
            if (!google)
            {
                if (account == null || !await _accountManager.CheckPasswordAsync(account, loginDTO.password))
                {
                    return new ResponseDTO() { IsSucceed = false, Message = "Invalid credentials" };
                }
            }
            if (account.Status == true && account.EmailConfirmed != false)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account had lock" };
            }

            var userRoles = await _accountManager.GetRolesAsync(account);
            var user = AccountDAO.Instance.ProfileDAO(account.Id);
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
            var check = new
            {
                Role = userRoles[0],
                Token = token,
                Check = user.Result.BacksideCCCD == null ? false : true,
            };
            return new ResponseDTO() { Result = check, IsSucceed = true, Message = "Successfully" };
        }

        /// <summary>
        /// Generates the new json web token.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="expiresIn">The expires in.</param>
        /// <returns></returns>
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

        public string GenerateJwtToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])); // Key từ appsettings
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Thêm claims, bao gồm role
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),  // Email của user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID token
                new Claim(ClaimTypes.Role, role) // Role
            };

            // Tạo token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Thời gian hết hạn
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// Makes the user asynchronous.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
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
                Status = true,
            };

            var createUserResult = await _accountManager.CreateAsync(createAccount, account.Password);

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

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.USER);
            if (createUserResult.Succeeded)
            {
                var otpResult = await SendOtp(createAccount);
                if (!otpResult)
                {
                    await _accountManager.DeleteAsync(createAccount);
                    await AccountDAO.Instance.RemoveAccount(createAccount.Id);
                    return new ResponseDTO() { IsSucceed = false, Message = "Failed to send OTP. Please try again." };
                }
            }
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> CreateGoogle(AddAccountDTO account)
        {
            var createAccount = new Account
            {
                UserName = account.UserName,
                Email = account.Email,
                Warning = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
                Status = false,
                EmailConfirmed = true,
            };


            var createUserResult = await _accountManager.CreateAsync(createAccount, account.Password);

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

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.USER);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        /// <summary>
        /// Changes the pass word.
        /// </summary>
        /// <param name="changepassDTO">The changepass dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ChangePassWord(string userId, ChangepassDTO changepassDTO)
        {
            Account account = null;
            account = await _accountManager.FindByIdAsync(userId);
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

        /// <summary>
        /// Profiles the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
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
            var signature = await AccountDAO.Instance.getSignature(account.Id);
            if (accountDetail == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account details not found" };
            }
            profileDTO = new ProfileDTO
            {
                AccountId = account.Id,
                UserName = account.UserName,
                Avatar = accountDetail.Avatar,
                FrontCCCD = accountDetail.FrontCCCD,
                BacksideCCCD = accountDetail.BacksideCCCD,
                signature = signature != null ? signature.SignatureImg : "",
                Email = account.Email,
                FullName = accountDetail.FullName,
                Phone = accountDetail.Phone,
                City = accountDetail.City,
                Ward = accountDetail.Ward,
                District = accountDetail.District,
                Address = accountDetail.Address,
                Warning = account.Warning,
                Status = account.Status,
                Role = role,
                birthdate = accountDetail.Birthdate,
                gender = accountDetail.Gender,
                dateOfIssue = accountDetail.DateOfIssue,
                placeOfIssue = accountDetail.PlaceOfIssue,
                placeOfResidence = accountDetail.PlaceOfResidence,
                categoryId = accountDetail.CategoryId,
            };
            return new ResponseDTO { Result = profileDTO, IsSucceed = true, Message = "Successfully" };
        }

        public async Task<bool> checkLoginEmail(string email)
        {
            var check = await _accountManager.FindByEmailAsync(email);
            if (check != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the content of the reset password email.
        /// </summary>
        /// <param name="resetLink">The reset link.</param>
        /// <returns></returns>
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
        private async Task<bool> SendOtp(Account user)
        {
            var otp = GenerateOtp(5);
            var otpContent = GetOtpEmailContent(otp);

            // Send OTP to user's email using an email service (MailUtils.SendMailGoogleSmtp)
            var emailSent = await MailUtils.SendMailGoogleSmtp(
                fromEmail: "nguyenanh0978638@gmail.com", // Sender email (consider moving to config)
                toEmail: user.Email,
                subject: "Your OTP for Account Verification",
                body: otpContent
            );
            if (emailSent)
            {
                await AccountDAO.Instance.StoreOtpForUser(user, otp, DateTime.UtcNow.AddMinutes(1));  // OTP expires in 10 minutes.
            }

            return emailSent;
        }

        // Helper method to generate the OTP email content
        private string GetOtpEmailContent(string otp)
        {
            return $@"
                <p>Hello,</p>
                <p>We received a request to verify your email address. Use the OTP below to complete the verification:</p>
                <p><strong>Your OTP is: {otp}</strong></p>
                <p>This OTP will expire in 10 minutes.</p>
                <p>If you did not request this, please ignore this email.</p>
                <p>Best regards,<br />Your Company Name</p>
            ";
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
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
            var resetLink = $"http://localhost:5173/resetPasswordPage/{code}/{account.Email}";

            // Gửi email chứa đường link đặt lại mật khẩu
            await MailUtils.SendMailGoogleSmtp(
                fromEmail: "nguyenanh0978638@gmail.com",
                toEmail: account.Email,
                subject: "Forgot Password",
                body: GetResetPasswordEmailContent(resetLink)
            );

            return new ResponseDTO { IsSucceed = true, Message = "Email sent successfully." };
        }

        /// <summary>
        /// Resets the password asynchronous.
        /// </summary>
        /// <param name="resetPasswordDTO">The reset password dto.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AddInformation(string userID, AddInforUserDTO uProfileDTO)
        {
            var account = await AccountDAO.Instance.ProfileDAO(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }
            var keys = _signatureHelper.GenerateKeys();
            if (uProfileDTO.signature == null || uProfileDTO.signature.Length == 0)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Hình ảnh chữ ký không được để trống." };
            }
            string base64SignatureImage;

            using (var memoryStream = new MemoryStream())
            {
                await uProfileDTO.signature.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                base64SignatureImage = Convert.ToBase64String(imageBytes);
            }
            var signature = _signatureHelper.SignData(base64SignatureImage, keys.privateKey);
            var fileAttach = new DigitalSignature
            {
                AccountID = userID,
                Base64SignatureImage = base64SignatureImage,
                SignatureImg = await _upload.SaveFileAsync(uProfileDTO.signature, "DigitalSignature", userID),
                Signature = signature,
                PublicKey = keys.publicKey,
                PrivateKey = keys.privateKey,
                CreatedAt = DateTime.Now,
            };
            var accountDetail = new AccountDetail
            {
                AccountID = userID,
                FullName = uProfileDTO.fullName,
                Phone = uProfileDTO.phone,
                City = uProfileDTO.city,
                Ward = uProfileDTO.ward,
                District = uProfileDTO.district,
                Address = uProfileDTO.address,
                Avatar = uProfileDTO.avatar != null ? await _upload.SaveFileAsync(uProfileDTO.avatar, "avatars", userID) : account.Avatar,
                FrontCCCD = uProfileDTO.frontCCCD != null ? await _upload.SaveFileAsync(uProfileDTO.frontCCCD, "cccd/front", userID) : account.FrontCCCD,
                BacksideCCCD = uProfileDTO.backsideCCCD != null ? await _upload.SaveFileAsync(uProfileDTO.backsideCCCD, "cccd/back", userID) : account.BacksideCCCD,
                Birthdate = uProfileDTO.birthdate,
                Gender = uProfileDTO.gender,
                PlaceOfResidence = uProfileDTO.placeOfResidence,
                PlaceOfIssue = uProfileDTO.placeOfIssue,
                DateOfIssue = uProfileDTO.dateOfIssue,
            };

            try
            {
                await FileAttachmentsDAO.Instance.AddFileAttachment(fileAttach);
                await AccountDAO.Instance.UpdateAccountDetail(accountDetail);
                return new ResponseDTO { IsSucceed = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Profile update failed: " + ex.Message };
            }
        }
        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="uProfileDTO">The u profile dto.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Makes the adminsync.
        /// </summary>
        /// <param name="updatePermissionDTO">The update permission dto.</param>
        /// <returns></returns>
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
                AccountID = createAccount.Id,
                CategoryId = updatePermissionDTO.category == 0 ? null : updatePermissionDTO.category,
            };

            var createAccountDetailResult = await AccountDAO.Instance.AddAccountDetailAsync(createAccountDetail);

            if (!createAccountDetailResult)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account detail creation failed" };
            }

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.ADMIN);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        /// <summary>
        /// Lists the account.
        /// </summary>
        /// <returns></returns>
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
                        Role = string.Join(", ", roles), // Nối các vai trò thành chuỗi
                        birthdate = item.accDetail.Birthdate,
                        gender = item.accDetail.Gender,
                        dateOfIssue = item.accDetail.DateOfIssue,
                        placeOfIssue = item.accDetail.PlaceOfIssue,
                        placeOfResidence = item.accDetail.PlaceOfResidence,
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


        /// <summary>
        /// Locks the account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Uns the lock account.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
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
        private string GenerateOtp(int length)
        {
            var random = new Random();
            var otp = string.Empty;

            // Generate OTP consisting of 5 random digits
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString(); // Random number between 0 and 9
            }

            return otp;
        }

        public async Task<ResponseDTO> VerifyOtp(VerifyOtpViewModel model)
        {
            try
            {
                var result = await AccountDAO.Instance.VerifyOtp(model);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Successfully" };
                }
                return new ResponseDTO { IsSucceed = false, Message = "Failed." };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = ex.Message };
            }
        }
    }
}
