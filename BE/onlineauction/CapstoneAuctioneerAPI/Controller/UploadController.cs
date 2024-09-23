using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// The upload repository
        /// </summary>
        private readonly IUploadRepository _uploadRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadController"/> class.
        /// </summary>
        /// <param name="uploadRepository">The upload repository.</param>
        public UploadController(IUploadRepository uploadRepository)
        {
            _uploadRepository = uploadRepository;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        [HttpGet("read")]
        public async Task<IActionResult> ReadFile(string filePath)
        {
            try
            {
                // Đọc tệp từ đường dẫn
                var fileStream = await _uploadRepository.ReadFileAsync(filePath);

                // Xác định loại MIME của tệp dựa trên phần mở rộng
                var contentType = "application/octet-stream";
                var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

                contentType = fileExtension switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => contentType
                };

                return File(fileStream, contentType, filePath);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi khi đọc file.");
            }
        }

    }
}
