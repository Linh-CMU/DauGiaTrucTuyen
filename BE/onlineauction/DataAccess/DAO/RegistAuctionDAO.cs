using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistAuctionDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static RegistAuctionDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="RegistAuctionDAO" /> class from being created.
        /// </summary>
        private RegistAuctionDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static RegistAuctionDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new RegistAuctionDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>
        /// Registers the auction.
        /// </summary>
        /// <param name="registAuction">The regist auction.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<ResponseDTO> RegisterAuction(RegistAuction registAuction)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var check = context.RegistAuctioneers.Where(x => x.ListAuctionID == registAuction.ListAuctionID && x.AccountID == registAuction.AccountID).FirstOrDefault();
                    if (check != null)
                    {
                        var checkPay = context.Deposits.Where(p => p.RAID == check.RAID).FirstOrDefault();
                        context.Deposits.Remove(checkPay);
                        context.RegistAuctioneers.Remove(check);
                        await context.SaveChangesAsync();
                    }
                    context.RegistAuctioneers.Add(registAuction);
                    await context.SaveChangesAsync();
                    return new ResponseDTO { IsSucceed = true, Message = "You have successfully registered for this auction." };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Register failed" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> Listofregisteredbidders(string userid)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                            join d in context.Deposits on r.RAID equals d.RAID
                                            where r.AccountID == userid && d.status == "success"
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                TimePerLap = ad.TimePerLap,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice,
                                                winningBid = context.Bets
                                                .Join(context.RegistAuctioneers,
                                                      b => b.RAID,
                                                      r => r.RAID,
                                                      (b, r) => new { b.PriceBit, r.ListAuctionID })
                                                .Where(br => br.ListAuctionID == a.ListAuctionID)
                                                .OrderByDescending(br => br.PriceBit)
                                                .Select(br => br.PriceBit)
                                                .FirstOrDefault(),
                                                status = r.AuctionStatus == true ? "chúc mừng" : "Chia buồn",
                                            }).Distinct().ToListAsync();


                return auctioneerList;
            }
        }
        /// <summary>
        /// Filters the auctioneers by status.
        /// </summary>
        /// <param name="auctioneerList">The auctioneer list.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private List<ListAuctioneerDTO> FilterAuctioneersByStatus(List<ListAuctioneerDTO> auctioneerList, int status)
        {
            var today = DateTime.Today;
            var currentTime = DateTime.Now.TimeOfDay;

            DateTime? ParseDate(string date) =>
                DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate) ? parsedDate : (DateTime?)null;

            TimeSpan? ParseTime(string time) =>
                TimeSpan.TryParse(time, out var parsedTime) ? parsedTime : (TimeSpan?)null;

            if (status == 1) // Upcoming auctions
            {
                return auctioneerList.Where(a =>
                {
                    var startDate = ParseDate(a.StartDay);
                    var startTime = ParseTime(a.StartTime);
                    return startDate > today || (startDate == today && startTime > currentTime);
                }).ToList();
            }
            else if (status == 2) // Ongoing auctions
            {
                return auctioneerList.Where(a =>
                {
                    var startDate = ParseDate(a.StartDay);
                    var startTime = ParseTime(a.StartTime);
                    var endDate = ParseDate(a.EndDay);
                    var endTime = ParseTime(a.EndTime);
                    return (startDate < today || (startDate == today && startTime <= currentTime)) &&
                           (endDate > today || (endDate == today && endTime >= currentTime));
                }).ToList();
            }
            else if (status == 3) // Past auctions
            {
                return auctioneerList.Where(a =>
                {
                    var endDate = ParseDate(a.EndDay);
                    var endTime = ParseTime(a.EndTime);
                    return endDate < today || (endDate == today && endTime < currentTime);
                }).ToList();
            }

            return auctioneerList;
        }
        /// <summary>
        /// Bets the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<PlacingABid> BetAsync(int id)
        {
            using (var context = new ConnectDB())
            {
                var result = await (from r in context.RegistAuctioneers
                                    join b in context.Bets on r.RAID equals b.RAID
                                    where r.ListAuctionID == id
                                    select b)
                                .OrderByDescending(b => b.PriceBit)  // Sort by Price in descending order
                                .FirstOrDefaultAsync();  // Get the top 1 record

                return result;
            }
        }
        /// <summary>
        /// Selects the identifier.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> SelectId(string userid, int id)
        {
            using (var context = new ConnectDB())
            {
                var result = await (from r in context.RegistAuctioneers
                                    where r.AccountID == userid && r.ListAuctionID == id
                                    select r.RAID)
                                .FirstOrDefaultAsync();  // Get the top 1 record

                return result;
            }
        }

        public async Task<decimal> CheckPrice(int id)
        {
            using (var context = new ConnectDB())
            {
                var result = await context.ListAuctions.Where(a => a.ListAuctionID == id).FirstOrDefaultAsync();  // Get the top 1 record

                return result.MoneyDeposit;
            }
        }

        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<ResponseDTO> PlaceBid(PlacingABid bet, decimal price, bool plac)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var check = await (from a in context.ListAuctions
                                       join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                       join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                       where r.RAID == bet.RAID
                                       select new
                                       {
                                           AuctionDetail = ad,
                                           ListAuction = a
                                       }).FirstOrDefaultAsync();
                    if (plac)
                    {
                        // Existing bet found, so update it
                        bet.PriceBit = check.ListAuction.StartingPrice + price;
                        bet.BidTime = DateTime.Now;
                        context.Bets.Add(bet);  // Add the new bet
                    }
                    else
                    {
                        bet.PriceBit = bet.PriceBit + price;
                        bet.BidTime = DateTime.Now;
                        context.Bets.Add(bet);  // Add the new bet
                    }
                    await context.SaveChangesAsync();
                    return new ResponseDTO { IsSucceed = true, Message = "successfully" };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "failed" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDTO> UpdatePayment(string id, string status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Tìm kiếm các bản ghi tương ứng với `RAID` trong hai bảng
                    var find = await context.Deposits.FirstOrDefaultAsync(rg => rg.DID == id);
                    if (find != null)
                    {
                        var re = await context.RegistAuctioneers.FirstOrDefaultAsync(rg => rg.RAID == find.RAID);
                        if (status == "cancel")
                        {
                            if (find != null) context.Deposits.Remove(find);
                            if (re != null) context.RegistAuctioneers.Remove(re);
                            await context.SaveChangesAsync();
                            return new ResponseDTO { IsSucceed = true, Message = "Update successful - records deleted" };
                        }
                        else if (re != null)
                        {
                            find.status = status;
                            context.Entry(find).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                            return new ResponseDTO { IsSucceed = true, Message = "Update successful - status updated" };
                        }
                    }
                    else
                    {
                        var pay = await context.Payments.FirstOrDefaultAsync(p => p.OrderCode == id);
                        if (pay != null)
                        {
                            if (status == "cancel")
                            {
                                context.Payments.Remove(pay);
                            }
                            else
                            {
                                var result = await (from a in context.Accounts
                                                    join rg in context.RegistAuctioneers on a.Id equals rg.AccountID
                                                    join p in context.ListAuctions on rg.ListAuctionID equals p.ListAuctionID
                                                    join c in context.Accounts on p.Creator equals c.Id
                                                    join ad in context.Accounts on p.Manager equals ad.Id
                                                    where rg.RAID == pay.RAID
                                                    select new
                                                    {
                                                        Payer = a,
                                                        Admin = ad,
                                                        Auction = p
                                                    }).FirstOrDefaultAsync();
                                var auctioneerNotification = new Notification
                                {
                                    AccountID = result.Admin.Id,
                                    Title = $"Thông báo thanh toán",
                                    Description = $"Người dùng đã thanh toán sản phẩm {result.Auction.NameAuction} ngày {DateTime.Now.ToString("dd/MM/yyyy")}",
                                    StatusNotification = false
                                };
                                await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                                var create = new Notification
                                {
                                    AccountID = result.Payer.Id,
                                    Title = $"Thông báo thanh toán",
                                    Description = $"Bạn đã thanh toán thành công với sản phẩm {result.Auction.NameAuction} ngày {DateTime.Now.ToString("dd/MM/yyyy")}",
                                    StatusNotification = false
                                };
                                await NotificationDAO.Instance.AddNotification(create);
                                pay.Status = true;
                                context.Entry(pay).State = EntityState.Modified;
                            }
                            await context.SaveChangesAsync();
                            return new ResponseDTO { IsSucceed = true, Message = "Update successful - status updated" };
                        }
                        return new ResponseDTO { IsSucceed = false, Message = "Record not found" };
                    }
                    return new ResponseDTO { IsSucceed = false, Message = "Record not found" };
                }
            }
            catch (DbUpdateException)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Database update failed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = ex.Message };
            }
        }


        /// <summary>
        /// Withdraws the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<ResponseDTO> Withdraw(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var auctioneer = await context.RegistAuctioneers.FirstOrDefaultAsync(i => i.ListAuctionID == id);
                    if (auctioneer == null)
                    {
                        return new ResponseDTO { IsSucceed = true, Message = "NotFound" };
                    }
                    var bet = await context.Bets.Where(b => b.RAID == auctioneer.RAID).ToListAsync();
                    if (bet.Any())
                    {
                        foreach (var i in bet)
                        {
                            context.Bets.Remove(i);
                        }
                    }
                    context.RegistAuctioneers.Remove(auctioneer);
                    await context.SaveChangesAsync();
                    return new ResponseDTO { IsSucceed = true, Message = "successfully" };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "failed" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Views the bid history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}</exception>
        public async Task<List<ViewBidHistoryDTO>> ViewBidHistory(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                join b in context.Bets on r.RAID equals b.RAID
                                where r.ListAuctionID == id
                                select new ViewBidHistoryDTO
                                {
                                    ID = b.PlacingABidID,
                                    userId = r.AccountID,
                                    Price = b.PriceBit,
                                    DateAndTime = b.BidTime.ToString("dd/MM/yyyy HH:mm")
                                };
                    return await query.OrderByDescending(o => o.ID).ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<ResponseDTO> CheckPayMent(Payment payment, int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var check = context.Payments.Where(x => x.RAID == payment.RAID).ToList();
                    var RAID = context.RegistAuctioneers.Where(a => a.ListAuctionID == id).FirstOrDefault();
                    if (check.Any())
                    {
                        return new ResponseDTO { IsSucceed = false, Message = "You have paid." };
                    }
                    payment.RAID = RAID.RAID;
                    payment.PaymentDate = DateTime.Now.ToString();
                    context.Payments.Add(payment);
                    await context.SaveChangesAsync();
                    return new ResponseDTO { IsSucceed = true, Message = "You have paid successfully." };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "payment failed" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}</exception>
        public async Task<InforPayMentDTO> TotalPay(int acutionId, string uid)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = await (from a in context.ListAuctions
                                       join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                       join b in context.Bets on r.RAID equals b.RAID
                                       where r.ListAuctionID == acutionId && r.AuctionStatus == true && r.AccountID == uid
                                       select new InforPayMentDTO
                                       {
                                           IdResgiter = r.RAID,
                                           nameAuction = a.NameAuction,
                                           priceAuction = Convert.ToInt32(a.MoneyDeposit)
                                       }).FirstOrDefaultAsync();
                    return query;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Totals the pay deposit.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<InforPayMentDTO> TotalPayDeposit(int acutionId, string uid)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var query = await (from a in context.ListAuctions
                                       join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                       where a.ListAuctionID == acutionId && r.AccountID == uid
                                       select new InforPayMentDTO
                                       {
                                           IdResgiter = r.RAID,
                                           nameAuction = a.NameAuction,
                                           priceAuction = Convert.ToInt32(a.MoneyDeposit)
                                       }).FirstOrDefaultAsync();
                    return query;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Updates the infor payment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.Exception">An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}</exception>
        public void UpdateInforPayment(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = (from r in context.RegistAuctioneers
                                 where r.ListAuctionID == id
                                 select r).FirstOrDefault();
                    query.PaymentTerm = DateTime.UtcNow.AddDays(1).ToString();
                    context.Entry(query).State = EntityState.Modified;
                    context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Checkusertopayments the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<SetTimeForBatchDTO> checkusertopayment(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var query = (from r in context.RegistAuctioneers
                                 join p in context.Payments on r.RAID equals p.RAID
                                 where r.RAID == id && p.Status == true
                                 select r).FirstOrDefault();
                    if (query == null)
                    {
                        var change = await (from r in context.RegistAuctioneers
                                            join a in context.ListAuctions on r.ListAuctionID equals a.ListAuctionID
                                            join b in context.Bets on r.RAID equals b.RAID
                                            join ad in context.AuctionDetails on r.ListAuctionID equals ad.ListAuctionID
                                            join adm in context.Accounts on a.Manager equals adm.Id
                                            join c in context.Accounts on a.Creator equals c.Id
                                            join u in context.Accounts on r.AccountID equals u.Id into userGroup // sử dụng into để tạo nhóm
                                            from u in userGroup.DefaultIfEmpty() // left join
                                            where r.ListAuctionID == query.ListAuctionID && r.RAID == id
                                            orderby b.PriceBit descending
                                            select new SetTimeForBatchDTO
                                            {
                                                EmailAdmin = adm.Email,
                                                AuctioneerEmail = c.Email,
                                                BidderEmail = (from b in context.Bets
                                                               join ra in context.RegistAuctioneers on b.RAID equals ra.RAID
                                                               join acc in context.Accounts on ra.AccountID equals acc.Id
                                                               where ra.ListAuctionID == a.ListAuctionID
                                                               orderby b.PriceBit descending
                                                               select acc.Email).FirstOrDefault(),
                                                Price = b.PriceBit,
                                                RegistAuctioneer = r,
                                                AccountId = u.Id,
                                                Title = a.NameAuction,
                                                AccountAdminId = adm.Id,
                                                AccountAuctionId = c.Id
                                            }).FirstOrDefaultAsync();
                        var account = await (from a in context.Accounts
                                             where a.Id == change.RegistAuctioneer.AccountID
                                             select a).FirstOrDefaultAsync();
                        account.Warning = account.Warning + 1;
                        context.Entry(account).State = EntityState.Modified;
                        query.AuctionStatus = false;
                        context.Entry(query).State = EntityState.Modified;
                        var search = context.Bets.Where(a => a.RAID == id).ToList();
                        foreach (var item in search)
                        {
                            var plac = context.Bets.FirstOrDefault(a => a.PlacingABidID == item.PlacingABidID);
                            context.Bets.Remove(plac);
                        }
                        await context.SaveChangesAsync();
                        if (account.Warning >= 3)
                        {
                            account.Status = true;
                            context.Entry(account).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                        }
                        return change;
                    }
                    else
                    {
                        return new SetTimeForBatchDTO { status = true };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Seconds the check userto payment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<bool> SecondCheckUsertoPayment(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Tìm kiếm người dùng dựa trên RAID
                    var query = await (from r in context.RegistAuctioneers
                                       where r.RAID == id
                                       select r).FirstOrDefaultAsync();

                    if (query == null)
                    {
                        return false; // Không tìm thấy người dùng
                    }

                    // Tìm tài khoản của người dùng
                    var account = await context.Accounts.FindAsync(query.AccountID);
                    if (account == null)
                    {
                        return false; // Không tìm thấy tài khoản
                    }

                    // Cập nhật cảnh báo
                    account.Warning += 1;
                    context.Entry(account).State = EntityState.Modified;

                    // Cập nhật trạng thái của người dự thầu
                    query.AuctionStatus = false;
                    context.Entry(query).State = EntityState.Modified;

                    await context.SaveChangesAsync();

                    // Kiểm tra nếu cảnh báo đạt 3 lần
                    if (account.Warning >= 3)
                    {
                        account.Status = true;
                        context.Entry(account).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }

                    return false; // Trả về false vì người dùng không thanh toán
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }

        }
        /// <summary>
        /// Sends the mail after paymet.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public SetTimeForBatchDTO sendMailAfterPaymet(int id, string uid)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var change = (from r in context.RegistAuctioneers
                                  join a in context.ListAuctions on r.ListAuctionID equals a.ListAuctionID
                                  join b in context.Bets on r.RAID equals b.RAID
                                  join ad in context.AuctionDetails on r.ListAuctionID equals ad.ListAuctionID
                                  join adm in context.Accounts on a.Manager equals adm.Id
                                  join c in context.Accounts on a.Creator equals c.Id
                                  join u in context.Accounts on r.AccountID equals u.Id
                                  where r.ListAuctionID == id && r.AccountID == uid
                                  select new SetTimeForBatchDTO
                                  {
                                      EmailAdmin = adm.Email,
                                      AuctioneerEmail = c.Email,
                                      BidderEmail = u.Email,
                                      Price = b.PriceBit,
                                      RegistAuctioneer = r,
                                      AccountId = u.Id,
                                      Title = a.NameAuction,
                                      AccountAdminId = adm.Id,
                                      AccountAuctionId = c.Id
                                  }).FirstOrDefault();
                    return change;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Payments for deposit.
        /// </summary>
        /// <param name="deposit">The deposit.</param>
        /// <returns></returns>
        public async Task<bool> PaymentForDeposit(Deposit deposit)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.Deposits.Add(deposit);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Payment(Payment deposit)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.Payments.Add(deposit);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the identifier register auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<int> getIdRegisterAuction(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = await (from a in context.RegistAuctioneers
                                        where a.ListAuctionID == id
                                        select a.RAID).FirstOrDefaultAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
    }
}
