using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class DigitalSignatureDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static DigitalSignatureDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="DigitalSignatureDAO"/> class from being created.
        /// </summary>
        private DigitalSignatureDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DigitalSignatureDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DigitalSignatureDAO();
                    }
                    return _instance;
                }
            }
        }
        public async Task<bool> SignData(DigitalSignature request)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.FileAttachments.Add(request);
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
        public async Task<DigitalSignature> VerifyData(string PublicKey, string Signature)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var digitalSignature = await context.FileAttachments
                        .FirstOrDefaultAsync(ds => ds.PublicKey == PublicKey && ds.Signature == Signature);
                    return digitalSignature;
                }
            }catch(DbUpdateException ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
