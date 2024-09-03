using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserReponsitory : IUserReponsitory
    {
        private readonly UserManager<Account> _accountManager;
        private readonly IUploadRepository _upload;
        public UserReponsitory(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }

        public async Task<ResponseDTO> RegiterAuctioneer(string userID,RegisterAuctioneerDTO register)
        {
            var account = await _accountManager.FindByIdAsync(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Auctioneer not found" };
            }
            var auctioneer = new ListAuctioneer
            {
                Creator = account.Id,
                Image =  await _upload.SaveFileAsync(register.Image, "ListAuctioneer", userID),
                NameAuctioneer = register.NameAuctioneer,
                Description = register.Description,
                StartingPrice = register.StartingPrice
            };
            try
            {
                var result = await AuctionDAO.Instance.AddAuction(auctioneer);
                if (result)
                {
                    var id = await AuctionDAO.Instance.GetAuctioneer(userID);
                    var detailauctioneer = new AuctioneerDetail
                    {
                        ListAuctioneerID = id,
                        CategoryID = register.CategoryID,
                        StartDay = register.StartDay,
                        StartTime = register.StartTime,
                        EndDay = register.EndTime,
                        EndTime = register.EndTime,
                        NumberofAuctionRounds = 1,
                        TimePerLap = "1",
                        PaymentMethod = "bid up"
                    };
                    var resultdetail = await AuctionDAO.Instance.AddAuctionDetail(detailauctioneer);
                    if(resultdetail)
                    {
                        var fileAttach = new FileAttachments
                        {
                            ListAuctioneerID = id,
                            FileAuctioneer = await _upload.SaveFileAsync(register.file, "FileAttachments", userID),
                            SignatureImg = await _upload.SaveFileAsync(register.signatureImg, "FileAttachments", userID),
                        };
                        var resultfile = await FileAttachmentsDAO.Instance.AddFileAttachment(fileAttach);
                        if (resultfile)
                        {
                            var file = await FileAttachmentsDAO.Instance.GetFileAttachments(id);
                            if(file is not null)
                            {
                                var img = new TImage
                                {
                                    FileAID = file.FileAID,
                                    Imange =  await _upload.SaveFileAsync(register.image, "TImage", userID)
                                };
                                var resultimg = await FileAttachmentsDAO.Instance.AddImage(img);
                                if (resultimg)
                                {
                                    return new ResponseDTO { IsSucceed = true, Message = "Add AddAuction successfully" };
                                }
                                else
                                {
                                    return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                                }
                            }
                            else
                            {
                                return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                            }
                        }
                        else
                        {
                            return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                        }

                    }
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                }
            }
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Add Auctioneer failed: " + ex.Message };
            }
            throw new NotImplementedException();
        }
    }
}
