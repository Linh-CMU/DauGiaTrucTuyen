using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AuctionDAO
    {
        private static AuctionDAO _instance = null;
        private static readonly object _instanceLock = new object();

        private AuctionDAO() { }

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
        public async Task<bool> AddAuction(ListAuctioneer listAuctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.ListAuctioneers.Add(listAuctioneer);
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
        public async Task<int> GetAuctioneer(string accountID)
        {
            int id = 0;
            try
            {
                using (var context = new ConnectDB())
                {
                    // Left join to get all auctioneers, even those without matching details
                    var auctioneer = await (from a in context.ListAuctioneers
                                            join ad in context.AuctioneerDetails
                                            on a.ListAuctioneerID equals ad.ListAuctioneerID into adGroup
                                            from ad in adGroup.DefaultIfEmpty() // Left join, keep ListAuctioneers data if no match in AuctioneerDetails
                                            where a.Creator == accountID && ad == null // ad == null means no match in AuctioneerDetails
                                            select a).FirstOrDefaultAsync();

                    if (auctioneer != null)
                    {
                        id = auctioneer.ListAuctioneerID;
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ListAuctioneerDTO>> ListAuctioneer(int status)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctioneers
                                            join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                            where a.StatusAuction == true
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctioneerID,
                                                Img = a.Image,
                                                Name = a.NameAuctioneer,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice
                                            }).ToListAsync();

                return FilterAuctioneersByStatus(auctioneerList, status);
            }
        }
        public async Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctioneers
                                            join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                            where a.StatusAuction == true && a.NameAuctioneer.ToLower().Contains(content.ToLower())
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctioneerID,
                                                Img = a.Image,
                                                Name = a.NameAuctioneer,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice
                                            }).ToListAsync();

                return auctioneerList;
            }
        }

        public async Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, int status)
        {
            try
            {
                // Nếu category == 0, trả về tất cả
                if (category == 0)
                {
                    return await ListAuctioneer(status);
                }

                using (var context = new ConnectDB())
                {
                    var auctioneerList = await (from a in context.ListAuctioneers
                                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                                where a.StatusAuction == true && ad.CategoryID == category
                                                select new ListAuctioneerDTO
                                                {
                                                    Id = a.ListAuctioneerID,
                                                    Img = a.Image,
                                                    Name = a.NameAuctioneer,
                                                    StartDay = ad.StartDay,
                                                    StartTime = ad.StartTime,
                                                    EndDay = ad.EndDay,
                                                    EndTime = ad.EndTime,
                                                    PriceStart = a.StartingPrice
                                                }).ToListAsync();

                    return FilterAuctioneersByStatus(auctioneerList, status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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


        public async Task<DAuctioneerDTO> AuctioneerDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var winner = await (from ad in context.AccountDetails
                                        join ac in context.Accounts on ad.AccountID equals ac.Id
                                        join r in context.RegistAuctioneers on ac.Id equals r.AccountID
                                        join b in context.Bets on r.RAID equals b.RAID
                                        join a in context.ListAuctioneers on r.ListAuctioneerID equals a.ListAuctioneerID
                                        where a.ListAuctioneerID == id
                                        orderby b.PriceBit descending // Order by PriceBit in descending order
                                        select new
                                        {
                                            AccountDetails = ad,
                                            Account = ac
                                        }).FirstOrDefaultAsync(); // Take only the first entry with the highest PriceBit


                    var auctioneerList = await (from a in context.ListAuctioneers
                                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                                join us in context.Accounts on a.Creator equals us.Id
                                                join ct in context.Categorys on ad.CategoryID equals ct.CategoryID
                                                join f in context.FileAttachments on ad.ListAuctioneerID equals f.ListAuctioneerID
                                                join i in context.TImages on f.FileAID equals i.FileAID
                                                join ud in context.AccountDetails on us.Id equals ud.AccountID
                                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                                from m in adGroup.DefaultIfEmpty()
                                                where a.StatusAuction == true
                                                select new DAuctioneerDTO
                                                {
                                                    ID = a.ListAuctioneerID,
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
                                                    NameAuctioneer = a.NameAuctioneer,
                                                    Description = a.Description,
                                                    StartDay = ad.StartDay,
                                                    StartTime = ad.StartTime,
                                                    EndDay = ad.EndDay,
                                                    EndTime = ad.EndTime,
                                                    StartingPrice = a.StartingPrice,
                                                    categoryName = ct.NameCategory,
                                                    NumberofAuctionRounds = ad.NumberofAuctionRounds,
                                                    TimePerLap = ad.TimePerLap,
                                                    PriceStep = ad.PriceStep,
                                                    PaymentMethod = ad.PaymentMethod,
                                                    FileAuctioneer = f.FileAuctioneer,
                                                    SignatureImg = f.SignatureImg,
                                                    TImange = new TImage
                                                    {
                                                        TImageId = i.TImageId,
                                                        Imange = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={i.Imange}"
                                                    },
                                                    WinBidder = winner,
                                                    StatusAuction = a.StatusAuction == true ? "Approved" : a.StatusAuction == false ? "Reject" : "Not approved yet"
                                                }).FirstOrDefaultAsync();
                    return auctioneerList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAuctioneer(ListAuctioneer auctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAutioneer = await context.ListAuctioneers
                        .FirstOrDefaultAsync(ad => ad.ListAuctioneerID == auctioneer.ListAuctioneerID);

                    if (existingAutioneer == null)
                    {
                        throw new Exception("Account detail not found.");
                    }

                    // Cập nhật các thông tin của tài khoản
                    existingAutioneer.Image = auctioneer.Image;
                    existingAutioneer.NameAuctioneer = auctioneer.NameAuctioneer;
                    existingAutioneer.Description = auctioneer.Description;
                    existingAutioneer.StartingPrice = auctioneer.StartingPrice;

                    // Đánh dấu entity là đã sửa đổi và lưu các thay đổi
                    context.Entry(existingAutioneer).State = EntityState.Modified;
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
        public async Task<bool> AddAuctionDetail(AuctioneerDetail auctioneerDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.AuctioneerDetails.Add(auctioneerDetail);
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
        public async Task<bool> DeleteAuctioneer(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var lista = await context.ListAuctioneers.FirstOrDefaultAsync(a => a.ListAuctioneerID == id && a.StatusAuction == null);
                    if (lista == null) return false;

                    // Auctioneer details
                    var ad = await context.AuctioneerDetails.FirstOrDefaultAsync(a => a.ListAuctioneerID == id);
                    if (ad != null)
                    {
                        // File Attachments
                        var file = await context.FileAttachments.Where(a => a.ListAuctioneerID == id).ToListAsync();
                        if (file.Any())
                        {
                            foreach (var item in file)
                            {
                                // Images related to FileAttachment
                                var img = await context.TImages.Where(i => i.FileAID == item.FileAID).ToListAsync();
                                if (img.Any())
                                {
                                    context.TImages.RemoveRange(img); // Remove images in bulk
                                }
                                context.FileAttachments.Remove(item); // Remove the file attachment
                            }
                        }
                        context.AuctioneerDetails.Remove(ad); // Remove AuctioneerDetail
                    }

                    context.ListAuctioneers.Remove(lista); // Remove the ListAuctioneer

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
        public async Task AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idManager)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAutioneer = await context.ListAuctioneers
                        .FirstOrDefaultAsync(ad => ad.ListAuctioneerID == autioneer.AutioneerID);
                    var existingAutioneerDetail = await context.AuctioneerDetails
                        .FirstOrDefaultAsync(ad => ad.ListAuctioneerID == autioneer.AutioneerID);

                    if (existingAutioneer == null)
                    {
                        throw new Exception("Auctioneer not found.");
                    }

                    if (existingAutioneerDetail == null)
                    {
                        throw new Exception("Auctioneer details not found.");
                    }

                    // Update auctioneer status and details
                    existingAutioneer.StatusAuction = autioneer.Status;
                    existingAutioneer.Manager = idManager;
                    existingAutioneerDetail.PriceStep = autioneer.PriceStep == null ? 0 : autioneer.PriceStep;

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
        public async Task<List<AuctioneerDetailDTO>> ListYourAuctioneer(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctioneers
                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where a.Creator == id
                                select new AuctioneerDetailDTO
                                {
                                    ListAuctioneerID = a.ListAuctioneerID,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuctioneer = a.NameAuctioneer,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
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
                    return await query.ToListAsync();
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

        public async Task<AuctioneerDetailDTO> ListYourAutioneerDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = await (from a in context.ListAuctioneers
                                        join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                        join m in context.AccountDetails
                                        on a.Manager equals m.AccountID into adGroup
                                        from m in adGroup.DefaultIfEmpty()
                                        where a.ListAuctioneerID == id
                                        select new AuctioneerDetailDTO
                                        {
                                            ListAuctioneerID = a.ListAuctioneerID,
                                            Name = a.Manager == null ? "No management yet" : m.FullName,
                                            Image = a.Image,
                                            NameAuctioneer = a.NameAuctioneer,
                                            StartingPrice = a.StartingPrice,
                                            StartDay = ad.StartDay,
                                            StartTime = ad.StartTime,
                                            EndDay = ad.EndDay,
                                            EndTime = ad.EndTime,
                                            StatusAuction = a.StatusAuction == null ? "Not approved yet" : a.StatusAuction == false ? "Reject" : "Approved"
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
        public async Task<List<AuctioneerDetailDTO>> ListYourAuctioneerAdmin(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctioneers
                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where a.Manager == id || a.Manager == null
                                select new AuctioneerDetailDTO
                                {
                                    ListAuctioneerID = a.ListAuctioneerID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuctioneer = a.NameAuctioneer,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
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
                    return await query.ToListAsync();
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
        public async Task<List<AuctioneerDetailDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctioneers
                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && a.NameAuctioneer.ToLower().Contains(content.ToLower())
                                select new AuctioneerDetailDTO
                                {
                                    ListAuctioneerID = a.ListAuctioneerID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuctioneer = a.NameAuctioneer,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };
                    return await query.ToListAsync();
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
        public async Task<List<AuctioneerDetailDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctioneers
                                join ad in context.AuctioneerDetails on a.ListAuctioneerID equals ad.ListAuctioneerID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && ad.CategoryID == category
                                select new AuctioneerDetailDTO
                                {
                                    ListAuctioneerID = a.ListAuctioneerID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuctioneer = a.NameAuctioneer,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
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
                    return await query.ToListAsync();
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
    }
}
