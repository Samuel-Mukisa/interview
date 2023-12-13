using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Ishop.Infrastructure.ServiceImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ishop.Gui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly ILogger<ShoppingCartAPIController> _logger;   
       private readonly IShoppingCartService _shoppingcartmanagement;
        
        public ShoppingCartAPIController(IShoppingCartService shoppingcartmanagement,ILogger<ShoppingCartAPIController> logger)
        {
           _shoppingcartmanagement = shoppingcartmanagement;
            _logger = logger;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartproducts()
        {
            try
            {
                var items = await _shoppingcartmanagement.GetCartItems();

                if (items != null)
                {
                    return Ok(items);
                }
                else
                {
                    _logger.LogInformation("No cart Items found"); // Log a more specific message
                    return NotFound("No cart Items found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving cart Items: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteCartItems")]
        public async Task<ActionResult<int>> DeleteCartItems(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            await _shoppingcartmanagement.DeleteCartItem(id);
            return Ok();
              
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItem>> GetCartItems(int id)
        {

            _logger.LogError("Get CartItem Error with Id" + id);
            var categories = await _shoppingcartmanagement.GetCartItem(id);

            if (categories != null)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest("Error retrieving categories");
            }
            }
        [HttpPost("AddCartItem")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItem cartItem)
        {
            try
            {
                bool success = await _shoppingcartmanagement.PostCartItem(cartItem);

                if (success)
                {
                    return Ok("CartItem added successfully");
                }
                else
                {
                    return BadRequest("Failed to add CartItem");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error($"Error adding CartItem: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }

}
