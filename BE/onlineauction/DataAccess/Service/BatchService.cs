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
                        Description = "Bạn đã không thanh toán đúng hẹn và bạn sẽ chịu phạt nếu đủ 3 lần tài khoản bạn sẽ bị khóa. và bạn sẽ không nhận lại được tiền cọc",
                        StatusNotification = false
                    };
                    await NotificationDAO.Instance.AddNotification(notifications);
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người trúng thầu không trả tiền đúng hẹn nên sản phẩm đấu giá không thành công",
                        StatusNotification = false
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người trúng thầu không trả tiền đúng hẹn nên sản phẩm đấu giá không thành công",
                        StatusNotification = false
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
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia",
                        StatusNotification = false
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia",
                        StatusNotification = false
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
                if (result.BidderEmail != null && (int)result.Price > 0)
                {
                    var checkpay = await AuctionDAO.Instance.findPay(result.RAID);
                    if (checkpay == null)
                    {
                        ItemData itemData = new ItemData(result.Title, 1, (int)result.Price);
                        List<ItemData> items = new List<ItemData>();
                        items.Add(itemData);
                        var orderCode = GenerateUniqueId();
                        var pay = new Payment
                        {
                            RAID = result.RAID,
                            Status = false,
                            PaymentDate = DateTime.Now.ToString("dd/MM/yyyy"),
                            OrderCode = orderCode.ToString(),
                            PaymentType = "Payment online",
                        };
                        var pays = await _userService.Payment(pay);
                        if (pays != true)
                        {
                            throw new Exception();
                        }
                        string shortenedName = result.Title.Length > 10
                                ? result.Title.Substring(0, 10)
                                : result.Title;
                        var expirationTime = DateTime.Now.AddHours(24).ToString("yyyy-MM-dd HH:mm:ss");
                        PaymentData paymentData = new PaymentData(orderCode, (int)result.Price, $"Thanh Toán {shortenedName}", items, "https://auction-fe-nu.vercel.app/cancel", "https://auction-fe-nu.vercel.app/success", expirationTime);
                        CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                        var payments = GetResetPasswordEmailContent(createPayment.checkoutUrl.ToString());
                        // Gửi email cho Bidder
                        var bidderSuccessBody = new StringBuilder();
                        bidderSuccessBody.AppendLine("<html>");
                        bidderSuccessBody.AppendLine("<body style='font-family: Arial, sans-serif; line-height: 1.6;'>");
                        bidderSuccessBody.AppendLine("<h2 style='color: #4CAF50;'>Chúc mừng! Bạn đã thắng cuộc đấu giá.</h2>");
                        bidderSuccessBody.AppendLine("<p><strong>Giá đấu thành công:</strong> " + result.Price + "</p>");
                        bidderSuccessBody.AppendLine("<p>Yêu cầu thanh toán trong vòng <strong>1 ngày</strong>. Nếu không thanh toán, bạn sẽ bị nhường lại cho người khác.</p>");
                        bidderSuccessBody.AppendLine("<p style='color: #FF0000;'>Xin lưu ý: Nếu bạn không thanh toán quá <strong>3 lần</strong>, tài khoản của bạn sẽ bị khóa.</p>");
                        bidderSuccessBody.AppendLine("<hr>");
                        bidderSuccessBody.AppendLine("<p>Phương thức thanh toán:</p>");
                        bidderSuccessBody.AppendLine($"<p>{payments}</p>");
                        bidderSuccessBody.AppendLine("</body>");
                        bidderSuccessBody.AppendLine("</html>");

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
                                    Description = "Xin chia buồn với bạn đã không đấu giá được sản phẩm với mức giá mong muốn.",
                                    StatusNotification = false
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
                                Description = bidderSuccessBody.ToString(),
                                StatusNotification = false
                            };
                            await NotificationDAO.Instance.AddNotification(notificationForBidder);

                            // Thông báo cho admin
                            var adminNotification = new Notification
                            {
                                AccountID = result.AccountAdminId,
                                Title = $"Kết quả buổi đấu giá: {result.Title}",
                                Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}",
                                StatusNotification = false
                            };
                            await NotificationDAO.Instance.AddNotification(adminNotification);

                            // Thông báo cho auctioneer
                            var auctioneerNotification = new Notification
                            {
                                AccountID = result.AccountAuctionId,
                                Title = $"Kết quả buổi đấu giá: {result.Title}",
                                Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}",
                                StatusNotification = false
                            };
                            await NotificationDAO.Instance.AddNotification(auctioneerNotification);

                            CreateAuction(RAID, DateTime.Now.AddDays(1), result.endTime, result.AccountId);
                        }
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
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAuctioneerFailureEmailBody(result)}"
                        );
                    }

                    // Gửi email cho Admin
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Auction Results - Failure",
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAuctioneerFailureEmailBody(result)}"
                        );
                    }
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia",
                        StatusNotification = false
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia",
                        StatusNotification = false
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
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Kính gửi Quý Quản Trị,</p>
                    <p>Cuộc đấu giá đã được xử lý.</p>
                    <p><strong>Thời gian kết thúc:</strong> {result.endTime}</p>
                    {(result.BidderEmail != null ? $@"
                    <p><strong>Cuộc đấu giá đã thành công.</strong></p>
                    <p><strong>Người thắng cuộc:</strong> {result.BidderEmail}</p>
                    <p><strong>Giá đấu thành công:</strong> {result.Price}</p>
                    " : "<p>Cuộc đấu giá đã thất bại do không có người thắng cuộc.</p>")}
                    <p>Xin vui lòng kiểm tra và xử lý nếu cần thiết.</p>
                    <p>Đội ngũ hỗ trợ.</p>
                </body>
                </html>";
        }


        /// <summary>
        /// Generates the auctioneer email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerEmailBody(SetTimeForBatch result)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Kính gửi Quý Nhà Đấu Giá,</p>
                    <p>Cuộc đấu giá của bạn đã được xử lý.</p>
                    <p><strong>Thời gian kết thúc:</strong> {result.endTime}</p>
                    {(result.BidderEmail != null ? $@"
                    <p><strong>Chúc mừng! Cuộc đấu giá đã thành công.</strong></p>
                    <p><strong>Người thắng cuộc:</strong> {result.BidderEmail}</p>
                    <p><strong>Giá đấu thành công:</strong> {result.Price}</p>
                    " : "<p>Rất tiếc! Cuộc đấu giá đã thất bại do không có người thắng cuộc.</p>")}
                    <p>Xin chân thành cảm ơn!</p>
                    <p>Đội ngũ hỗ trợ.</p>
                </body>
                </html>";
        }

        /// <summary>
        /// Generates the auctioneer failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerFailureEmailBody(SetTimeForBatch result)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Kính gửi Quý Nhà Đấu Giá,</p>
                <p>Rất tiếc! Buổi đấu giá đã không thành công do không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.</p>
                <p>Thông tin đấu giá đã được hủy bỏ.</p>
                <p>Xin vui lòng kiểm tra và xử lý nếu cần thiết.</p>
                <p>Xin chân thành cảm ơn!</p>
                <p>Đội ngũ hỗ trợ.</p>
            </body>
            </html>";
        }

        /// <summary>
        /// Generates the admin failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAdminFailureEmailBody(SetTimeForBatch result)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Kính gửi Quý Quản Trị,</p>
                <p>Buổi đấu giá đã thất bại vì không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.</p>
                <p>Thông tin đấu giá đã được hủy bỏ.</p>
                <p>Xin vui lòng kiểm tra và xử lý nếu cần thiết.</p>
                <p>Đội ngũ hỗ trợ.</p>
            </body>
            </html>";
        }
    }
}
