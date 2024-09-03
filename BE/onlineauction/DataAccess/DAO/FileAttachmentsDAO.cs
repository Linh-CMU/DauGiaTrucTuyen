using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class FileAttachmentsDAO
    {
        private static FileAttachmentsDAO _instance = null;
        private static readonly object _instanceLock = new object();

        private FileAttachmentsDAO() { }

        public static FileAttachmentsDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new FileAttachmentsDAO();
                    }
                    return _instance;
                }
            }
        }
        public async Task<bool> AddFileAttachment(FileAttachments fileAttachments)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.FileAttachments.Add(fileAttachments);
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
        public async Task<FileAttachments> GetFileAttachments(int AuctioneerID)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var file = await context.FileAttachments.FirstOrDefaultAsync(f => f.ListAuctioneerID == AuctioneerID);
                    return file;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddImage(TImage image)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.TImages.Add(image);
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
    }
}
