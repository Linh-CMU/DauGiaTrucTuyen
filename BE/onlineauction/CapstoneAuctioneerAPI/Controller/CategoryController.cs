using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AdminService _adminService;
        public CategoryController(AdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        [Route("listCategory")]
        public async Task<ActionResult> ListCategory()
        {
            try
            {
                var result = await _adminService.ListCategoryAsync();
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
