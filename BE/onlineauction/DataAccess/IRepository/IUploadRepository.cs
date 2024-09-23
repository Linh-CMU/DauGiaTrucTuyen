using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUploadRepository
    {
        /// <summary>
        /// Saves the file asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Task<string> SaveFileAsync(IFormFile file, string folder, string userId);
        /// <summary>
        /// Reads the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Task<Stream> ReadFileAsync(string fileName);
    }
}
