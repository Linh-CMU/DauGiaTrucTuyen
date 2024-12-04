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
    /// <summary>
    /// 
    /// </summary>
    public class FileAttachmentsDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static FileAttachmentsDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="FileAttachmentsDAO"/> class from being created.
        /// </summary>
        private FileAttachmentsDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
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
        /// <summary>
        /// Adds the file attachment.
        /// </summary>
        /// <param name="fileAttachments">The file attachments.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddFileAttachment(DigitalSignature fileAttachments)
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
        /// <summary>
        /// Gets the file attachments.
        /// </summary>
        /// <param name="AuctioneerID">The auctioneer identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        //public async Task<DigitalSignature> GetFileAttachments(int AuctioneerID)
        //{
        //    try
        //    {
        //        using (var context = new ConnectDB())
        //        {
        //            var file = await context.FileAttachments.FirstOrDefaultAsync(f => f.ListAuctionID == AuctioneerID);
        //            return file;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        /// <summary>
        /// Adds the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
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
