using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalSignatureController : ControllerBase
    {
        private readonly DigitalSignatureHelper _signatureHelper;

        public DigitalSignatureController()
        {
            _signatureHelper = new DigitalSignatureHelper();
        }

        [HttpPost("generate-keys")]
        public IActionResult GenerateKeys()
        {
            var keys = _signatureHelper.GenerateKeys();
            return Ok(new { PublicKey = keys.publicKey, PrivateKey = keys.privateKey });
        }

        [HttpPost("sign")]
        public async Task<IActionResult> SignData([FromForm] SignRequest request)
        {
            if (request.SignatureImage == null || request.SignatureImage.Length == 0)
            {
                return BadRequest("Hình ảnh chữ ký không được để trống.");
            }

            // Chuyển đổi IFormFile sang Base64
            string base64SignatureImage;

            using (var memoryStream = new MemoryStream())
            {
                await request.SignatureImage.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                base64SignatureImage = Convert.ToBase64String(imageBytes);
            }

            // Ký dữ liệu hình ảnh
            var signature = _signatureHelper.SignData(base64SignatureImage, request.PrivateKey);
            return Ok(new { Signature = signature });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyData([FromForm] VerifyRequest request)
        {
            if (request.SignatureImage == null || request.SignatureImage.Length == 0)
            {
                return BadRequest("Hình ảnh chữ ký không được để trống.");
            }

            // Chuyển đổi IFormFile sang Base64
            string base64SignatureImage;

            using (var memoryStream = new MemoryStream())
            {
                await request.SignatureImage.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                base64SignatureImage = Convert.ToBase64String(imageBytes);
            }

            // Xác minh dữ liệu hình ảnh
            bool isValid = _signatureHelper.VerifyData(base64SignatureImage, request.Signature, request.PublicKey);
            return Ok(new { IsValid = isValid });
        }
    }
}
