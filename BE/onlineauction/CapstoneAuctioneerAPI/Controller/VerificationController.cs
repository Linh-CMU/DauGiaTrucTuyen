using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public VerificationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCCCD(IFormFile cccd)
        {
            var apiUrl = "https://api.fpt.ai/vision/idr/vnm"; // URL của API
            var apiKey = "dcQN6rrxNscN8fkcm2BCz3duFMBgkK1y"; // API key của bạn

            // Kiểm tra file CCCD
            if (cccd == null || cccd.Length == 0)
            {
                return BadRequest("File CCCD không hợp lệ.");
            }

            // Tạo nội dung gửi dạng multipart/form-data
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(cccd.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(cccd.ContentType);

            // Đặt tên cho form-data là "image" và file name
            content.Add(fileContent, "image", cccd.FileName);

            // Đặt API key vào headers nếu chưa có
            if (!_httpClient.DefaultRequestHeaders.Contains("api-key"))
            {
                _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
            }

            try
            {
                // Gửi yêu cầu POST tới API
                var response = await _httpClient.PostAsync(apiUrl, content);

                // Kiểm tra kết quả trả về
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    // Ghi log phản hồi để kiểm tra
                    Console.WriteLine("API Response: " + jsonString);

                    // Chuyển đổi JSON thành đối tượng C#
                    var verificationResponse = JsonSerializer.Deserialize<VerificationResponse>(jsonString);

                    return Ok(verificationResponse);
                }
                else
                {
                    return BadRequest("Không thể xác thực số CCCD. Mã lỗi: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi gửi yêu cầu tới API: " + ex.Message);
            }
        }

    }
}
