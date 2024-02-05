using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Ishop.Infrastructure.ServiceImplementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryManagementService _categoryManagementService;
        //private readonly CategoriesService _connectionFactory;
        public CategoriesController(ILogger<CategoriesController> logger
           , ICategoryManagementService categoryManagementService
        
        )
        {
            _logger = logger;
            _categoryManagementService = categoryManagementService;
           

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpGet("getcategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            _logger.LogInformation("Getting all Categories");
            var categories = await _categoryManagementService.GetAllCategories();

            if (categories != null)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest("Error retrieving categories");
            }

        }
        [HttpGet("getcategory/{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
           
                _logger.LogError("Get Category Error with Id"+ id);
                var categories = await _categoryManagementService.GetCategory(id);

                if (categories != null)
                {
                    return Ok(categories);
                }
                else
                {
                    return BadRequest("Error retrieving categories");
                }
            
            
           
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> CreateCategory([FromBody] Category categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(categoryDto);
            }

            if (categoryDto.CategoryId < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }



            _categoryManagementService.CreateCategory(categoryDto);
            
            return 1;
        }

        [HttpDelete("delete/{id:int}", Name = "DeleteCategory")]
        public async Task<ActionResult<int>> DeleteCategory(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            await _categoryManagementService.DeleteCategory(id);

            

           
            return 1;
        }

        [HttpPut("update/{id:int}", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
            {
                return BadRequest();
            }

            await _categoryManagementService.UpdateCategory(id, categoryDto);
            //var Category = CategoryStore.CategoryList.FirstOrDefault(u => u.CategoryId == id);
           // Category.CategoryName = categoryDto.CategoryName;
            return NoContent();
        }
        
        
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchCategories([FromQuery] string searchTerm)
        {
            try
            {
                var categories = await _categoryManagementService.SearchCategories(searchTerm);

                if (categories != null && categories.Count > 0)
                {
                    return Ok(categories);
                }
                else
                {
                    return NotFound("No categories found matching the search term.");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Log.Error($"Error searching categories: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}