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
        /// Prevents a default instance of the <see cref="RegistAuctionDAO"/> class from being created.
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
                    var check = context.RegistAuctioneers.Where(x => x.ListAuctionID == registAuction.ListAuctionID).ToList();
                    if (check.Any())
                    {
                        return new ResponseDTO { IsSucceed = false, Message = "You have registered for this auction." };
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
        public async Task<List<ListAuctioneerDTO>> Listofregisteredbidders(string userid, int status, bool? statusauction)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                            where r.AccountID == userid && (statusauction == true || statusauction == false ? r.AuctionStatus == statusauction : a.StatusAuction == true)
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice
                                            }).ToListAsync();


                return FilterAuctioneersByStatus(auctioneerList, status);
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
        /// <param name="userid">The userid.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Bet> BetAsync(int id)
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
                                    join b in context.Bets on r.RAID equals b.RAID
                                    where r.AccountID == userid && r.ListAuctionID == id
                                    select r.ListAuctionID)
                                .FirstOrDefaultAsync();  // Get the top 1 record

                return result;
            }
        }

        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="bet">The bet.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<ResponseDTO> PlaceBid(Bet bet)
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
                    if (bet.BetID != 0)
                    {
                        // Existing bet found, so update it
                        bet.PriceBit = check.ListAuction.StartingPrice + check.AuctionDetail.PriceStep ?? 0;
                        bet.BidTime = DateTime.Now.ToString("dd/MM/yyyy : HH:mm:ss");
                        context.Bets.Add(bet);  // Add the new bet
                    }
                    else
                    {
                        bet.PriceBit = bet.PriceBit + check.AuctionDetail.PriceStep ?? 0;
                        bet.BidTime = DateTime.Now.ToString("dd/MM/yyyy : HH:mm:ss");
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
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
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
                                    ID = b.BetID,
                                    Price = b.PriceBit,
                                    DateAndTime = b.BidTime
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
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public decimal TotalPay(int acutionId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                join b in context.Bets on r.RAID equals b.RAID
                                where r.ListAuctionID == acutionId && r.AuctionStatus == true
                                select new ViewBidHistoryDTO
                                {
                                    ID = b.BetID,
                                    Price = b.PriceBit,
                                    DateAndTime = b.BidTime
                                };
                    var result = query.OrderByDescending(o => o.ID).FirstOrDefault();
                    return result.Price == null ? 0m : result.Price;
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
                    query.PaymentTerm = DateTime.UtcNow.AddDays(2).ToString();
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
        public async Task<SetTimeForBatchDTO> checkusertopayment(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var query = (from r in context.RegistAuctioneers
                                 join p in context.Payments on r.RAID equals p.RAID
                                 where r.RAID == id
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
                                            where r.ListAuctionID == id && r.RAID != id
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
                                                Title = a.NameAuction
                                            }).FirstOrDefaultAsync();
                        var account = await (from a in context.Accounts
                                             where a.Id == change.RegistAuctioneer.AccountID
                                             select a).FirstOrDefaultAsync();
                        account.Warning = account.Warning + 1;
                        context.Entry(account).State = EntityState.Modified;
                        query.AuctionStatus = false;
                        context.Entry(query).State = EntityState.Modified;
                        change.RegistAuctioneer.AuctionStatus = true;
                        change.RegistAuctioneer.PaymentTerm = DateTime.Now.AddDays(2).ToString();
                        context.Entry(change).State = EntityState.Modified;
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
    }
}
