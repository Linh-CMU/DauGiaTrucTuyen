using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class BatchService
    {

        private readonly UserService _userService;
        private readonly PayOS _payOS;
        private readonly AuctionService _auctionService;

        public BatchService(UserService userService, PayOS payOS, AuctionService auctionService)
        {
            _userService = userService;
            _payOS = payOS;
            _auctionService = auctionService;
        }

        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="endTime">The end time.</param>
        public void CreateAuction(int id, DateTime endTime)
        {
            Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {endTime}.");
            DateTime notificationTime = endTime.AddMinutes(2);
            TimeSpan delay = notificationTime - DateTime.Now;

            // Kiểm tra xem delay có nhỏ hơn 0 không, nếu có thì không tạo job
            if (delay.TotalMilliseconds > 0)
            {
                BackgroundJob.Schedule(() => NotifyAuctionComplete(id), delay);
                Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {delay}.");
            }
            else
            {
                Console.WriteLine("Thời gian thông báo không hợp lệ.");
            }
        }
        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="date">The date.</param>
        /// <param name="account">The account.</param>
        public void CreateAuction(int id, DateTime endTime, DateTime date, string account)
        {
            Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {endTime}.");
            DateTime notificationTime = endTime.AddMinutes(15);
            TimeSpan delay = notificationTime - DateTime.Now;
            if (delay.TotalMilliseconds > 0)
            {
                BackgroundJob.Schedule(() => NotifyAuctionComplete(id, date, account), delay);
            }
            else
            {
                Console.WriteLine("Thời gian thông báo không hợp lệ.");
            }
        }
        /// <summary>
        /// Notifies the auction complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="account">The account.</param>
        public async Task NotifyAuctionComplete(int id, DateTime date, string account)
        {
            var check = await RegistAuctionDAO.Instance.checkusertopayment(id);
            var result = new SetTimeForBatch
            {
                EmailAdmin = check.EmailAdmin,
                AuctioneerEmail = check.AuctioneerEmail,
                BidderEmail = check?.BidderEmail,
                endTime = date,
                Price = check.Price,
                AccountId = check?.AccountId,
                Title = check.Title,
                AccountAdminId = check?.AccountAdminId,
                AccountAuctionId = check?.AccountAuctionId
            };

            if (check.status != true)
            {
                // Kiểm tra BidderEmail
                if (result.BidderEmail != null)
                {
                    // Thông báo cho người không thanh toán
                    var notifications = new Notification
                    {
                        AccountID = account,
                        Title = $"Cảnh báo không thanh toán: {result.Title}",
                        Description = "Bạn đã không thanh toán đúng hẹn và bạn sẽ chịu phạt nếu đủ 3 lần tài khoản bạn sẽ bị khóa. và bạn sẽ không nhận lại được tiền cọc"
                    };
                    await NotificationDAO.Instance.AddNotification(notifications);
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người trúng thầu không trả tiền đúng hẹn nên sản phẩm đấu giá không thành công"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người trúng thầu không trả tiền đúng hẹn nên sản phẩm đấu giá không thành công"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                    CreateAuction(id, DateTime.Now);
                }
                else
                {
                    // Gửi email thông báo đấu giá thất bại cho Auctioneer
                    if (result.AuctioneerEmail != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.AuctioneerEmail,
                            subject: "Auction Results - Failure",
                            body: GenerateAuctioneerFailureEmailBody(result)
                        );
                    }

                    // Gửi email thông báo đấu giá thất bại cho Admin
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Auction Results - Failure",
                            body: GenerateAdminFailureEmailBody(result)
                        );
                    }
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                }
            }
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
                                <p>Enter the link to payment <a href='" + resetLink + @"'>Link</a></p>
                            </body>
                            </html>";
            return emailContent;
        }
        private int GenerateUniqueId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            long longValue = BitConverter.ToInt64(buffer, 0); // Chuyển GUID thành long

            // Giới hạn giá trị trả về chỉ trong phạm vi 5 chữ số
            int uniqueId = (int)(Math.Abs(longValue) % 100000); // Lấy giá trị dương và chỉ lấy 5 chữ số cuối cùng

            return uniqueId;
        }
        /// <summary>
        /// Notifies the auction complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task NotifyAuctionComplete(int id)
        {
            var result = await AuctionDAO.Instance.GetInforSendMail(id);

            if (result != null)
            {
                // Kiểm tra BidderEmail
                if (result.BidderEmail != null)
                {
                    var payment = await _auctionService.TotalPay(id, result.AccountId);
                    ItemData itemData = new ItemData(payment.nameAuction, 1, payment.priceAuction);
                    List<ItemData> items = new List<ItemData>();
                    items.Add(itemData);
                    var orderCode = GenerateUniqueId();
                    var pay = new Payment
                    {
                        RAID = payment.IdResgiter,
                        Status = false,
                        OrderCode = orderCode.ToString(),
                        PaymentType = "Payment online",
                    };
                    var pays = await _userService.Payment(pay);
                    if (pays != true)
                    {
                        throw new Exception();
                    }
                    string shortenedName = payment.nameAuction.Length > 10
                            ? payment.nameAuction.Substring(0, 10)
                            : payment.nameAuction;
                    var expirationTime = DateTime.Now.AddHours(24).ToString("yyyy-MM-dd HH:mm:ss");
                    PaymentData paymentData = new PaymentData(orderCode, payment.priceAuction, $"Thanh Toán {shortenedName}", items, "http://localhost:5173/cancel", "http://localhost:5173/success", expirationTime);
                    CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                    var payments = GetResetPasswordEmailContent(createPayment.ToString());
                    // Gửi email cho Bidder
                    var bidderSuccessBody = new StringBuilder();
                    bidderSuccessBody.AppendLine("Chúc mừng! Bạn đã thắng cuộc đấu giá.");
                    bidderSuccessBody.AppendLine($"Giá đấu thành công: {result.Price}");
                    bidderSuccessBody.AppendLine("Yêu cầu thanh toán trong vòng 2 ngày. Nếu không thanh toán, bạn sẽ bị nhường lại cho người khác.");
                    bidderSuccessBody.AppendLine("Xin lưu ý: Nếu bạn không thanh toán quá 3 lần, tài khoản của bạn sẽ bị khóa.");
                    bidderSuccessBody.AppendLine($"{payments}");

                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.BidderEmail,
                        subject: "Auction Results - Success",
                        body: bidderSuccessBody.ToString()
                    );

                    // Gửi email cho Auctioneer
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.AuctioneerEmail,
                        subject: "Auction Results - Success",
                        body: GenerateAuctioneerEmailBody(result)
                    );

                    // Gửi email cho Admin
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.EmailAdmin,
                        subject: "Auction Results - Success",
                        body: GenerateAdminEmailBody(result)
                    );

                    RegistAuctionDAO.Instance.UpdateInforPayment(id);
                    var RAID = await AuctionDAO.Instance.UpdateWinningStatus(id);

                    // Tạo thông báo cho người thua cuộc
                    var checkuser = await AuctionDAO.Instance.InformationOfTheLoser(RAID, id);
                    if (checkuser != null)
                    {
                        foreach (var item in checkuser)
                        {
                            var notifications = new Notification
                            {
                                AccountID = item,
                                Title = $"Kết quả buổi đấu giá: {result.Title}",
                                Description = "Xin chia buồn với bạn đã không đấu giá được sản phẩm với mức giá mong muốn."
                            };
                            await NotificationDAO.Instance.AddNotification(notifications);
                        }
                    }
                    else
                    {
                        var notificationForBidder = new Notification
                        {
                            AccountID = result.AccountId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = bidderSuccessBody.ToString()
                        };
                        await NotificationDAO.Instance.AddNotification(notificationForBidder);

                        // Thông báo cho admin
                        var adminNotification = new Notification
                        {
                            AccountID = result.AccountAdminId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}"
                        };
                        await NotificationDAO.Instance.AddNotification(adminNotification);

                        // Thông báo cho auctioneer
                        var auctioneerNotification = new Notification
                        {
                            AccountID = result.AccountAuctionId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}"
                        };
                        await NotificationDAO.Instance.AddNotification(auctioneerNotification);

                        CreateAuction(RAID, DateTime.Now.AddDays(1), result.endTime, result.AccountId);
                    }
                }
                else
                {
                    // Nếu BidderEmail là null, gửi thông báo đấu giá thất bại cho Auctioneer và Admin
                    var failureMessage = "Buổi đấu giá đã thất bại do không có người thắng cuộc.";

                    // Gửi email cho Auctioneer
                    if (result.AuctioneerEmail != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.AuctioneerEmail,
                            subject: "Auction Results - Failure",
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAuctioneerEmailBody(result)}"
                        );
                    }

                    // Gửi email cho Admin
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Auction Results - Failure",
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAdminEmailBody(result)}"
                        );
                    }
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                }
            }
        }

        /// <summary>
        /// Generates the admin email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAdminEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Quản Trị,");
            body.AppendLine();

            // Thông báo về cuộc đấu giá
            body.AppendLine("Cuộc đấu giá đã được xử lý.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");

            if (result.BidderEmail != null)
            {
                body.AppendLine("Cuộc đấu giá đã thành công.");
                body.AppendLine($"Người thắng cuộc: {result.BidderEmail}");
                body.AppendLine($"Giá đấu thành công: {result.Price}");
            }
            else
            {
                body.AppendLine("Cuộc đấu giá đã thất bại do không có người thắng cuộc.");
            }

            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }

        /// <summary>
        /// Generates the auctioneer email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerEmailBody(SetTimeForBatch result)
        {
            // Thông tin cơ bản
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Nhà Đấu Giá,");
            body.AppendLine();

            // Thông báo về cuộc đấu giá
            body.AppendLine("Cuộc đấu giá của bạn đã được xử lý.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");

            if (result.BidderEmail != null)
            {
                body.AppendLine("Chúc mừng! Cuộc đấu giá đã thành công.");
                body.AppendLine($"Người thắng cuộc: {result.BidderEmail}");
                body.AppendLine($"Giá đấu thành công: {result.Price}");
            }
            else
            {
                body.AppendLine("Rất tiếc! Cuộc đấu giá đã thất bại do không có người thắng cuộc.");
            }

            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }
        /// <summary>
        /// Generates the auctioneer failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerFailureEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Nhà Đấu Giá,");
            body.AppendLine();
            body.AppendLine("Rất tiếc! Buổi đấu giá đã không thành công do không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.");
            body.AppendLine("Thông tin đấu giá đã được hủy bỏ.");
            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }
        /// <summary>
        /// Generates the admin failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAdminFailureEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Quản Trị,");
            body.AppendLine();
            body.AppendLine("Buổi đấu giá đã thất bại vì không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.");
            body.AppendLine("Thông tin đấu giá đã được hủy bỏ.");
            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine();
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }


    }
}
