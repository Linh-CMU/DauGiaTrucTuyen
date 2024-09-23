using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        /// <summary>
        /// The admin service
        /// </summary>
        private readonly AdminService _adminService;
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="adminService">The admin service.</param>
        public CategoryController(AdminService adminService)
        {
            _adminService = adminService;
        }
        /// <summary>
        /// Lists the category.
        /// </summary>
        /// <returns></returns>
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
