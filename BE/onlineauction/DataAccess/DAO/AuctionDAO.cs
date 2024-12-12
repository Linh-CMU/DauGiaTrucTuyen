using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static AuctionDAO _instance = null;

        private readonly IUploadRepository _upload;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        public AuctionDAO(IUploadRepository upload)
        {
            _upload = upload;
        }
        /// <summary>
        /// Prevents a default instance of the <see cref="AuctionDAO"/> class from being created.
        /// </summary>
        private AuctionDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static AuctionDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AuctionDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>
        /// Adds the auction.
        /// </summary>
        /// <param name="listAuctioneer">The list auctioneer.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddAuction(ListAuction listAuctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.ListAuctions.Add(listAuctioneer);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Gets the auctioneer.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<int> GetAuctioneer(string accountID)
        {
            int id = 0;
            try
            {
                using (var context = new ConnectDB())
                {
                    // Left join to get all auctioneers, even those without matching details
                    var auctioneer = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails
                                            on a.ListAuctionID equals ad.ListAuctionID into adGroup
                                            from ad in adGroup.DefaultIfEmpty() // Left join, keep ListAuctions data if no match in AuctionDetails
                                            where a.Creator == accountID && ad == null // ad == null means no match in AuctionDetails
                                            select a).FirstOrDefaultAsync();

                    if (auctioneer != null)
                    {
                        id = auctioneer.ListAuctionID;
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> ListAuctioneer(string uid)
        {
            using (var context = new ConnectDB())
            {
                List<ListAuctioneerDTO> auctioneerList = await (
                                            from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            where a.StatusAuction == true && a.Creator != uid // Lọc các điều kiện cần thiết
                                                  && !context.RegistAuctioneers
                                                      .Where(r => r.ListAuctionID == a.ListAuctionID)
                                                      .Any(r => r.AccountID == uid) // Loại bỏ các bản ghi chứa uid
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay ?? null,
                                                StartTime = ad.StartTime ?? null,
                                                EndDay = ad.EndDay ?? null,
                                                TimePerLap = ad.TimePerLap,
                                                EndTime = ad.EndTime ?? null,
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
                                            }
                                        ).OrderByDescending(o => o.Id).ToListAsync();

                return auctioneerList;
            }
        }


        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content, string uid)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID into adGroup
                                            from rg in adGroup.DefaultIfEmpty()
                                            where a.StatusAuction == true && a.NameAuction.ToLower().Contains(content.ToLower()) && (string.IsNullOrEmpty(uid) || a.Creator != uid)
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                CategoryId = ad.CategoryID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay,
                                                TimePerLap = ad.TimePerLap,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
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
                                            }).Distinct().OrderByDescending(o => o.Id).ToListAsync();

                return auctioneerList;
            }
        }

        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, string uid)
        {
            try
            {

                using (var context = new ConnectDB())
                {
                    List<ListAuctioneerDTO> auctioneerList = await (from a in context.ListAuctions
                                                                    join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                                                    join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID into adGroup
                                                                    from rg in adGroup.DefaultIfEmpty()
                                                                    where a.StatusAuction == true && ad.CategoryID == category && (string.IsNullOrEmpty(uid) || a.Creator != uid)
                                                                    select new ListAuctioneerDTO
                                                                    {
                                                                        Id = a.ListAuctionID,
                                                                        Img = a.Image,
                                                                        Name = a.NameAuction,
                                                                        StartDay = ad.StartDay ?? null,
                                                                        StartTime = ad.StartTime ?? null,
                                                                        EndDay = ad.EndDay ?? null,
                                                                        EndTime = ad.EndTime ?? null,
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
                                                                    }).OrderByDescending(o => o.Id).ToListAsync();
                    return auctioneerList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<DAuctioneerDTO> AuctionDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var winner = await (from ad in context.AccountDetails
                                        join ac in context.Accounts on ad.AccountID equals ac.Id
                                        join r in context.RegistAuctioneers on ac.Id equals r.AccountID
                                        join b in context.Bets on r.RAID equals b.RAID
                                        join a in context.ListAuctions on r.ListAuctionID equals a.ListAuctionID
                                        join aud in context.AuctionDetails on a.ListAuctionID equals aud.ListAuctionID
                                        where a.ListAuctionID == id
                                        orderby b.PriceBit descending // Order by PriceBit in descending order
                                        select new Winner
                                        {
                                            idUser = ac.Id,
                                            nameUser = ad.FullName,
                                            price = b.PriceBit
                                        }).FirstOrDefaultAsync(); // Take only the first entry with the highest PriceBit


                    var auctioneerList = await (from a in context.ListAuctions
                                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                                join ds in context.FileAttachments on a.Creator equals ds.AccountID
                                                join us in context.Accounts on a.Creator equals us.Id
                                                join ct in context.Categorys on ad.CategoryID equals ct.CategoryID
                                                join i in context.TImages on ad.ListAuctionID equals i.ListAuctionID
                                                join ud in context.AccountDetails on us.Id equals ud.AccountID
                                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                                from m in adGroup.DefaultIfEmpty() // Sử dụng LEFT JOIN
                                                where a.ListAuctionID == id
                                                select new DAuctioneerDTO
                                                {
                                                    ID = a.ListAuctionID,
                                                    User = new ProfileDTO
                                                    {
                                                        AccountId = us.Id,
                                                        UserName = us.UserName,
                                                        Avatar = ud.Avatar,
                                                        FrontCCCD = ud.FrontCCCD,
                                                        BacksideCCCD = ud.BacksideCCCD,
                                                        Email = us.Email,
                                                        FullName = ud.FullName,
                                                        Phone = ud.Phone,
                                                        City = ud.City,
                                                        Ward = ud.Ward,
                                                        District = ud.District,
                                                        Address = ud.Address,
                                                        Warning = us.Warning,
                                                        Status = us.Status,
                                                        Role = "User"
                                                    },
                                                    Manager = a.Manager == null ? "No management yet" : m.FullName,
                                                    Image = a.Image,
                                                    NameAuction = a.NameAuction,
                                                    Description = a.Description,
                                                    StartDay = ad.StartDay == null ? "" : ad.StartDay,
                                                    StartTime = ad.StartTime == null ? "" : ad.StartTime,
                                                    EndDay = ad.EndDay == null ? "" : ad.EndDay,
                                                    EndTime = ad.EndTime == null ? "" : ad.EndTime,
                                                    StartingPrice = a.StartingPrice,
                                                    categoryName = ct.NameCategory,
                                                    NumberofAuctionRounds = ad.NumberofAuctionRounds,
                                                    TimePerLap = ad.TimePerLap,
                                                    PriceStep = ad.PriceStep,
                                                    PaymentMethod = ad.PaymentMethod,
                                                    SignatureImg = ds.SignatureImg,
                                                    evidenceFile = ad.evidenceFile,
                                                    TImange = new TImage
                                                    {
                                                        TImageId = i.TImageId,
                                                        Imange = i.Imange
                                                    },
                                                    WinBidder = winner,
                                                    StatusAuction = a.StatusAuction == true ? "Approved" : a.StatusAuction == false ? "Reject" : "Not approved yet",
                                                    MoneyDeposit = a.MoneyDeposit,
                                                    images = new List<Image>
                                                        {
                                                            new Image { img = a.Image }
                                                        }.Concat(
                                                            context.TImages
                                                                .Where(t => t.ListAuctionID == a.ListAuctionID)
                                                                .Select(t => new Image { img = t.Imange })
                                                        ).ToList(),
                                                    createDate = ad.CreateDate.ToString("dd/MM/yyyy")
                                                }).FirstOrDefaultAsync();

                    return auctioneerList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuctionRoomAdminDTO> AuctionRoomAdmin(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var winner = await (from ad in context.AccountDetails
                                        join ac in context.Accounts on ad.AccountID equals ac.Id
                                        join r in context.RegistAuctioneers on ac.Id equals r.AccountID
                                        join b in context.Bets on r.RAID equals b.RAID
                                        join a in context.ListAuctions on r.ListAuctionID equals a.ListAuctionID
                                        join aud in context.AuctionDetails on a.ListAuctionID equals aud.ListAuctionID
                                        where a.ListAuctionID == id
                                        orderby b.PriceBit descending
                                        select new Winner
                                        {
                                            idUser = ac.Id,
                                            nameUser = ad.FullName,
                                            price = b.PriceBit
                                        }).FirstOrDefaultAsync();


                    var money = await (from b in context.Bets
                                       join r in context.RegistAuctioneers on b.RAID equals r.RAID
                                       where r.ListAuctionID == id
                                       select b).OrderByDescending(c => c.PriceBit).FirstOrDefaultAsync();
                    var auctioneerList = await (from a in context.ListAuctions
                                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                                join us in context.Accounts on a.Creator equals us.Id
                                                join ct in context.Categorys on ad.CategoryID equals ct.CategoryID
                                                join ud in context.AccountDetails on us.Id equals ud.AccountID
                                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                                from m in adGroup.DefaultIfEmpty()
                                                where a.StatusAuction == true
                                                select new AuctionRoomAdminDTO
                                                {
                                                    id = a.ListAuctionID,
                                                    user = new ProfileDTO
                                                    {
                                                        AccountId = us.Id,
                                                        UserName = us.UserName,
                                                        Avatar = ud.Avatar,
                                                        FrontCCCD = ud.FrontCCCD,
                                                        BacksideCCCD = ud.BacksideCCCD,
                                                        Email = us.Email,
                                                        FullName = ud.FullName,
                                                        Phone = ud.Phone,
                                                        City = ud.City,
                                                        Ward = ud.Ward,
                                                        District = ud.District,
                                                        Address = ud.Address,
                                                        Warning = us.Warning,
                                                        Status = us.Status,
                                                        Role = "User"
                                                    },
                                                    manager = a.Manager == null ? "No management yet" : m.FullName,
                                                    image = a.Image,
                                                    nameAuction = a.NameAuction,
                                                    startDay = ad.StartDay,
                                                    startTime = ad.StartTime,
                                                    endDay = ad.EndDay,
                                                    endTime = ad.EndTime,
                                                    startingPrice = a.StartingPrice,
                                                    categoryName = ct.NameCategory,
                                                    priceStep = ad.PriceStep,
                                                    paymentMethod = ad.PaymentMethod,
                                                    winBidder = winner,
                                                    StatusAuction = a.StatusAuction == true ? "Approved" : a.StatusAuction == false ? "Reject" : "Not approved yet",
                                                    moneyDeposit = a.MoneyDeposit,
                                                    bidMoney = money != null ? money.PriceBit : 0
                                                }).FirstOrDefaultAsync();
                    return auctioneerList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates the auctioneer.
        /// </summary>
        /// <param name="auctioneer">The auctioneer.</param>
        /// <exception cref="System.Exception">
        /// Account detail not found.
        /// or
        /// An error occurred while updating the account detail: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task UpdateAuctioneer(UDAuctionDTO auctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Retrieve existing records
                    var existingAuctioneer = await context.ListAuctions
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == auctioneer.auctionID);
                    var auctionDetail = await context.AuctionDetails
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == auctioneer.auctionID);
                    var img = await context.TImages.FirstOrDefaultAsync(a => a.ListAuctionID == auctionDetail.ListAuctionID);

                    if (existingAuctioneer == null || auctionDetail == null)
                    {
                        throw new Exception("Auctioneer or auction detail not found.");
                    }

                    // Update properties of the existing entities
                    if (!string.IsNullOrEmpty(auctioneer.imageAuction))
                    {
                        existingAuctioneer.Image = auctioneer.imageAuction;
                    }
                    if (!string.IsNullOrEmpty(auctioneer.imageEvidence))
                    {
                        img.Imange = auctioneer.imageEvidence;
                    }
                    existingAuctioneer.NameAuction = auctioneer.nameAuctionItem;
                    existingAuctioneer.Description = auctioneer.description;
                    existingAuctioneer.StartingPrice = auctioneer.startingPrice;
                    existingAuctioneer.MoneyDeposit = auctioneer.startingPrice * 0.1m; // Calculate 10% deposit
                    auctionDetail.CategoryID = auctioneer.category;

                    // Mark entities as modified and save changes
                    context.Entry(existingAuctioneer).State = EntityState.Modified;
                    if (auctioneer.imageAuction != null || auctioneer.imageEvidence != null)
                    {
                        context.Entry(img).State = EntityState.Modified;
                    }
                    context.Entry(auctionDetail).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log detailed error or handle as desired
                throw new Exception($"An error occurred while updating the auction details: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other non-DbUpdateException errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Adds the auction detail.
        /// </summary>
        /// <param name="AuctionDetail">The auction detail.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddAuctionDetail(AuctionDetail AuctionDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.AuctionDetails.Add(AuctionDetail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deletes the auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> DeleteAuctioneer(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var lista = await context.ListAuctions.FirstOrDefaultAsync(a => a.ListAuctionID == id && a.StatusAuction == null);
                    if (lista == null) return false;

                    // Auctioneer details
                    var ad = await context.AuctionDetails.FirstOrDefaultAsync(a => a.ListAuctionID == id);
                    if (ad != null)
                    {
                        context.AuctionDetails.Remove(ad); // Remove AuctionDetail
                    }

                    context.ListAuctions.Remove(lista); // Remove the ListAuctioneer

                    await context.SaveChangesAsync(); // Save all changes once after all deletions
                }
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Accepts the auctioneer for admin.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <param name="idManager">The identifier manager.</param>
        /// <exception cref="System.Exception">
        /// Auctioneer not found.
        /// or
        /// Auctioneer details not found.
        /// or
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task AcceptAuctioneerForAdmin(AcceptAutionDTO autioneer, string idManager)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAutioneer = await context.ListAuctions
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == autioneer.AutioneerID);
                    var existingAutioneerDetail = await context.AuctionDetails
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == autioneer.AutioneerID);

                    if (existingAutioneer == null)
                    {
                        throw new Exception("Auctioneer not found.");
                    }

                    if (existingAutioneerDetail == null)
                    {
                        throw new Exception("Auctioneer details not found.");
                    }

                    // Update auctioneer status and details
                    if (autioneer.Status == true)
                    {
                        existingAutioneerDetail.StartDay = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
                        existingAutioneerDetail.EndDay = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
                        existingAutioneerDetail.StartTime = DateTime.Now.AddDays(2).ToString("HH:mm");
                        existingAutioneerDetail.EndTime = DateTime.Now.AddDays(3).ToString("HH:mm");
                        existingAutioneerDetail.PriceStep = existingAutioneer.StartingPrice * 0.1m;
                        existingAutioneerDetail.evidenceFile = autioneer.evidenceFile;
                    }
                    existingAutioneer.StatusAuction = autioneer.Status;
                    existingAutioneer.Manager = idManager;
                    existingAutioneerDetail.TimePerLap = autioneer.TimeRoom;

                    // Mark entities as modified
                    context.Entry(existingAutioneer).State = EntityState.Modified;
                    context.Entry(existingAutioneerDetail).State = EntityState.Modified;

                    // Save changes
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Lists your auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneer(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where a.Creator == id
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    TimePerLap = ad.TimePerLap,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 0)
                    {
                        return await query.ToListAsync();
                    }
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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

        public async Task<List<AuctionDetailDTO>> SearchListYourAuctioneer(string id, int category, string content)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where a.Creator == id && a.NameAuction.Contains(content.ToLower()) && (category != 0 ? ad.CategoryID == category : 1 == 1)
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    TimePerLap = ad.TimePerLap,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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

        public async Task<List<AuctionDetailDTO>> ListAuctioneerByUser(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where a.Creator == id
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    PriceDeposit = a.MoneyDeposit,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 0)
                    {
                        return await query.ToListAsync();
                    }
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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

        public async Task<List<AuctionDetailDTO>> ListAuctioneerRegisterByUser(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join r in context.RegistAuctioneers on ad.ListAuctionID equals r.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where r.AccountID == id
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    PriceDeposit = a.MoneyDeposit,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 0)
                    {
                        return await query.ToListAsync();
                    }
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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
        /// Lists your autioneer detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<AuctionDetailDTO> ListYourAutioneerDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = await (from a in context.ListAuctions
                                        join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                        join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                        join i in context.TImages on a.ListAuctionID equals i.ListAuctionID
                                        join m in context.AccountDetails
                                        on a.Manager equals m.AccountID into adGroup
                                        from m in adGroup.DefaultIfEmpty()
                                        where a.ListAuctionID == id
                                        select new AuctionDetailDTO
                                        {
                                            ListAuctionID = a.ListAuctionID,
                                            Category = c.NameCategory,
                                            CategoryId = ad.CategoryID,
                                            Name = a.Manager == null ? "No management yet" : m.FullName,
                                            description = a.Description,
                                            Image = a.Image,
                                            NameAuction = a.NameAuction,
                                            StepPrice = ad.PriceStep,
                                            PriceDeposit = a.MoneyDeposit,
                                            ImageEvidence = i.Imange,
                                            StartingPrice = a.StartingPrice,
                                            StartDay = ad.StartDay,
                                            StartTime = ad.StartTime,
                                            EndDay = ad.EndDay,
                                            EndTime = ad.EndTime,
                                            TimePerLap = ad.TimePerLap,
                                            StatusAuction = a.StatusAuction == null ? "Not approved yet" : a.StatusAuction == false ? "Reject" : "Approved",
                                            countBidder = context.RegistAuctioneers.Where(e => e.ListAuctionID == a.ListAuctionID).Count(),
                                            images = new List<Image>
                                                        {
                                                            new Image { img = a.Image }
                                                        }.Concat(
                                                            context.TImages
                                                                .Where(t => t.ListAuctionID == a.ListAuctionID)
                                                                .Select(t => new Image { img = t.Imange })
                                                        ).ToList(),
                                            createDate = ad.CreateDate.ToString("dd/MM/yyyy"),
                                        }).FirstOrDefaultAsync();

                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<AuctionRoomDTO> Auctionroom(int id, string userId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Fetch data
                    var auctionData = await (from a in context.ListAuctions
                                             join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                             join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                             join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                             where a.ListAuctionID == id && r.AccountID == userId
                                             select new
                                             {
                                                 a.ListAuctionID,
                                                 c.NameCategory,
                                                 a.Image,
                                                 a.NameAuction,
                                                 a.StartingPrice,
                                                 ad.StartDay,
                                                 ad.StartTime,
                                                 ad.EndDay,
                                                 ad.PriceStep,
                                                 ad.EndTime,
                                                 ad.TimePerLap,
                                                 r.AuctionStatus
                                             }).FirstOrDefaultAsync();
                    var money = await (from b in context.Bets
                                       join r in context.RegistAuctioneers on b.RAID equals r.RAID
                                       where r.ListAuctionID == id
                                       select b).OrderByDescending(c => c.PriceBit).FirstOrDefaultAsync();

                    if (auctionData == null)
                    {
                        return null;
                    }

                    // Parse StartDay, StartTime, EndDay, EndTime outside of LINQ
                    DateTime startDate, endDate;
                    TimeSpan startTime, endTime;

                    var isStartDateValid = DateTime.TryParse(auctionData.StartDay, out startDate);
                    var isStartTimeValid = TimeSpan.TryParse(auctionData.StartTime, out startTime);
                    var isEndDateValid = DateTime.TryParse(auctionData.EndDay, out endDate);
                    var isEndTimeValid = TimeSpan.TryParse(auctionData.EndTime, out endTime);

                    bool statusBet = false; // Default value

                    if (isStartDateValid && isStartTimeValid && isEndDateValid && isEndTimeValid)
                    {
                        // Combine date and time
                        DateTime auctionStartDateTime = startDate.Date + startTime;
                        DateTime auctionEndDateTime = endDate.Date + endTime;
                        DateTime currentDateTime = DateTime.Now;

                        if (currentDateTime >= auctionStartDateTime && currentDateTime <= auctionEndDateTime)
                        {
                            // The auction is currently active
                            statusBet = true;
                        }
                        else
                        {
                            // The auction is not active
                            statusBet = false;
                        }
                    }
                    else
                    {
                        // Handle invalid date/time parsing
                        // You may choose to set statusBet to false or throw an exception
                        statusBet = false;
                    }

                    // Map the properties to AuctionRoomDTO and return
                    return new AuctionRoomDTO
                    {
                        ListAuctionID = auctionData.ListAuctionID,
                        Category = auctionData.NameAuction,
                        Image = auctionData.Image,
                        NameAuction = auctionData.NameAuction,
                        StartingPrice = auctionData.StartingPrice,
                        stepMoney = auctionData.PriceStep,
                        StartDay = auctionData.StartDay,
                        StartTime = auctionData.StartTime,
                        EndDay = auctionData.EndDay,
                        EndTime = auctionData.EndTime,
                        TimeRound = auctionData.TimePerLap,
                        bidStatus = auctionData.AuctionStatus == null ? "No bids yet" : auctionData.AuctionStatus == true ? "You have successfully bid" : "You have failed to bid",
                        statusBet = statusBet
                    };
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }



        /// <summary>
        /// Lists your auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneerAdmin(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                join ac in context.AccountDetails on a.Manager equals ac.AccountID into acGroup
                                from acd in acGroup.DefaultIfEmpty()
                                where (a.Manager == id && (acd.CategoryId == null || acd.CategoryId == ad.CategoryID))
                                || a.Manager == null
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    PriceDeposit = a.MoneyDeposit,
                                    StartDay = ad.StartDay == null ? "" : ad.StartDay,
                                    StartTime = ad.StartTime == null ? "" : ad.StartTime,
                                    EndDay = ad.EndDay == null ? "" : ad.EndDay,
                                    EndTime = ad.EndTime == null ? "" : ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && a.NameAuction.ToLower().Contains(content.ToLower())
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay == null ? "" : ad.StartDay,
                                    StartTime = ad.StartTime == null ? "" : ad.StartTime,
                                    EndDay = ad.EndDay == null ? "" : ad.EndDay,
                                    EndTime = ad.EndTime == null ? "" : ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && ad.CategoryID == category
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay == null ? "" : ad.StartDay,
                                    StartTime = ad.StartTime == null ? "" : ad.StartTime,
                                    EndDay = ad.EndDay == null ? "" : ad.EndDay,
                                    EndTime = ad.EndTime == null ? "" : ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
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
        /// <summary>Gets the infor send mail.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<SetTimeForBatch> GetInforSendMail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var query = await (from a in context.ListAuctions
                                       join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                       join adm in context.Accounts on a.Manager equals adm.Id
                                       join c in context.Accounts on a.Creator equals c.Id
                                       where a.ListAuctionID == Convert.ToInt32(id)
                                       select new
                                       {
                                           EmailAdmin = adm.Email,
                                           AuctioneerEmail = c.Email,

                                           // Lấy email của người đấu giá cao nhất
                                           BidderEmail = (from b in context.Bets
                                                          join ra in context.RegistAuctioneers on b.RAID equals ra.RAID
                                                          join acc in context.Accounts on ra.AccountID equals acc.Id
                                                          where ra.ListAuctionID == a.ListAuctionID
                                                          orderby b.PriceBit descending
                                                          select acc.Email).FirstOrDefault(),
                                           endTime = ad.EndTime,
                                           endDay = ad.EndDay,
                                           // Lấy giá đấu cao nhất
                                           Price = (from b in context.Bets
                                                    join r in context.RegistAuctioneers on b.RAID equals r.RAID
                                                    where r.ListAuctionID == id
                                                    select b.PriceBit).OrderByDescending(x => x).FirstOrDefault(),
                                           Account = (from b in context.Bets
                                                      join r in context.RegistAuctioneers on b.RAID equals r.RAID
                                                      join a in context.Accounts on r.AccountID equals a.Id
                                                      where r.ListAuctionID == id
                                                      orderby b.PriceBit descending
                                                      select a).FirstOrDefault(),
                                           Title = a,
                                           Admin = adm,
                                           RAID = (from b in context.Bets
                                                   join r in context.RegistAuctioneers on b.RAID equals r.RAID
                                                   where r.ListAuctionID == id
                                                   orderby b.PriceBit descending
                                                   select r.RAID).FirstOrDefault(),
                                           Auction = c
                                       }).FirstOrDefaultAsync();

                    if (query != null)
                    {
                        return new SetTimeForBatch
                        {
                            EmailAdmin = query.EmailAdmin,
                            AuctioneerEmail = query.AuctioneerEmail,
                            BidderEmail = query.BidderEmail,
                            endTime = ConvertToDateTime(query.endDay, query.endTime),
                            Price = query.Price,
                            AccountId = query.Account?.Id,
                            Title = query.Title.NameAuction,
                            AccountAdminId = query.Admin.Id,
                            AccountAuctionId = query.Auction.Id,
                            RAID = query.RAID,
                        };
                    }

                    return null; // Hoặc throw exception tùy theo logic của bạn
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        public async Task<List<string>> InformationOfTheLoser(int winnerId, int auctionId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var losers = await (from a in context.Accounts
                                        join r in context.RegistAuctioneers on a.Id equals r.AccountID
                                        where r.ListAuctionID == auctionId && r.RAID != winnerId
                                        select a.Id)
                                        .ToListAsync();

                    // Chuyển đổi List<string> sang List<int>
                    return losers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Payment> findPay(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = context.Payments.Where(a => a.RAID == id).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }

        /// <summary>Converts to date time.</summary>
        /// <param name="endDay">The end day.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.FormatException">Định dạng EndDay hoặc EndTime không hợp lệ.</exception>
        private DateTime ConvertToDateTime(string endDay, string endTime)
        {
            string combinedDateTime = $"{endDay} {endTime}";

            // Sử dụng định dạng phù hợp cho ngày và giờ
            if (DateTime.TryParseExact(combinedDateTime, "dd/MM/yyyy HH:mm",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime endDateTime))
            {
                return endDateTime;
            }
            else
            {
                throw new FormatException("Định dạng EndDay hoặc EndTime không hợp lệ.");
            }
        }
        /// <summary>Updates the winning status.</summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<int> UpdateWinningStatus(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var highestBidder = await (from r in context.RegistAuctioneers
                                               join b in context.Bets on r.RAID equals b.RAID
                                               where r.ListAuctionID == id
                                               orderby b.PriceBit descending
                                               select r).FirstOrDefaultAsync();
                    if (highestBidder != null)
                    {
                        var auctioneers = await context.RegistAuctioneers
                                        .Where(r => r.ListAuctionID == id)
                                        .ToListAsync();
                        foreach (var auctioneer in auctioneers)
                        {
                            if (auctioneer.RAID == highestBidder.RAID)
                            {
                                auctioneer.AuctionStatus = true;  // Highest bidder
                            }
                            else
                            {
                                auctioneer.AuctionStatus = false; // Other bidders
                            }
                            context.Entry(auctioneer).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                        }
                    }
                    return highestBidder.RAID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        public async Task<bool> ReUpAuction(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var auction = await (from a in context.AuctionDetails
                                         where a.ListAuctionID == id
                                         select a).FirstOrDefaultAsync();
                    if (auction != null)
                    {
                        auction.StartDay = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        auction.EndDay = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        context.Entry(auction).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<AuctionnerAdminDTO>> listBidderInAuction(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var user = await (from a in context.RegistAuctioneers
                                      join u in context.AccountDetails on a.AccountID equals u.AccountID
                                      join d in context.Deposits on a.RAID equals d.RAID
                                      where a.ListAuctionID == id && d.status == "success"
                                      select new AuctionnerAdminDTO
                                      {
                                          userID = u.AccountID,
                                          userName = u.FullName
                                      }).ToListAsync();
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<(string Day, int Count)>> Productstatistics()
        {
            using (var context = new ConnectDB())
            {
                var startOfWeek = StartOfWeek(DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                // Lấy dữ liệu từ cơ sở dữ liệu và chuyển đổi thành danh sách (sử dụng ToListAsync để tải lên phía client).
                var rawData = await context.AuctionDetails
                    .Where(a => a.CreateDate >= startOfWeek && a.CreateDate <= endOfWeek)
                    .ToListAsync(); // Chỉ lọc dữ liệu, chưa nhóm.

                // Nhóm dữ liệu trên phía client.
                var groupedData = rawData
                    .GroupBy(a => a.CreateDate.DayOfWeek)
                    .Select(g => new
                    {
                        Day = g.Key.ToString(), // Chuyển đổi DayOfWeek thành chuỗi.
                        Count = g.Count()
                    })
                    .ToList(); // Chuyển kết quả thành danh sách.

                // Chuyển đổi kết quả thành dạng tuple (Day, Count) để trả về.
                return groupedData.Select(r => (Day: r.Day.Substring(0, 3), Count: r.Count)).ToList();
            }
        }

        public async Task<List<(string Month, decimal Count)>> MonthlyIncomeStatistics()
        {
            using (var context = new ConnectDB())
            {
                // Lấy toàn bộ dữ liệu từ cơ sở dữ liệu, áp dụng AsEnumerable để thực hiện trên client.
                var rawData = await context.Bets
                    .Include(r => r.RegistAuctioneer)
                    .ToListAsync();

                // Lọc dữ liệu trên client để chỉ lấy các bản ghi trong khoảng từ đầu năm đến hiện tại.
                var filteredData = rawData
                    .Where(a => a.BidTime >= new DateTime(DateTime.Now.Year, 1, 1) && a.BidTime <= DateTime.Now)
                    .ToList();

                // Nhóm dữ liệu theo tháng và tính tổng thu nhập cho mỗi tháng.
                var groupedData = filteredData
                    .GroupBy(b => b.BidTime.Month) // Nhóm theo tháng.
                    .Select(g => new
                    {
                        Month = g.Key, // Lấy tháng từ nhóm.
                        TotalIncome = g.Sum(x => x.PriceBit) // Tính tổng thu nhập cho mỗi nhóm tháng.
                    })
                    .OrderBy(g => g.Month) // Sắp xếp theo thứ tự từ tháng 1 đến tháng 12.
                    .ToList();

                // Chuyển đổi kết quả thành dạng tuple (Month, TotalIncome) để trả về.
                var result = groupedData
                    .Select(r => (Month: GetMonthName(r.Month), Count: r.TotalIncome)) // Chuyển đổi tháng từ số thành tên tháng.
                    .ToList();

                return result;
            }
        }

        public async Task<TotalDTO> TotalAsync()
        {
            using (var context = new ConnectDB())
            {
                var highestPrice = await (from a in context.ListAuctions
                                          join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                          join d in context.Bets on r.RAID equals d.RAID
                                          select d.PriceBit)
                         .MaxAsync();
                var countAccount = await context.Accounts
                                .Where(a => a.Status == false) // Kiểm tra Status == false
                                .CountAsync();
                var totalProduct = await context.ListAuctions.Where(a => a.StatusAuction == true).CountAsync();
                var result = new TotalDTO
                {
                    TotalAccount = countAccount,
                    TotalMoney = highestPrice,
                    TotalProduct = totalProduct
                };
                return result;
            }
        }

        public DateTime StartOfWeek(DayOfWeek startOfWeek)
        {
            var today = DateTime.Now;
            int diff = today.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return today.AddDays(-diff).Date;
        }
        private string GetMonthName(int monthNumber)
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            return months[monthNumber - 1];
        }
    }
}
