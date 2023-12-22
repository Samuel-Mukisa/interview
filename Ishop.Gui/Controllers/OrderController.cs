
// OrdersController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Ishop.Application.Interfaces;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderManagementService _orderManagementService;

        public OrderController(ILogger<OrderController> logger, IOrderManagementService orderManagementService)
        {
            _logger = logger;
            _orderManagementService = orderManagementService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateOrder([FromBody] OrderRequestModel orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order request");
            }

            var products = orderRequest.Products; // Assuming you have a model for order request

            if (products == null || products.Count == 0)
            {
                return BadRequest("No products in the order");
            }

            int userID = orderRequest.UserID; // Assuming you have the user ID

            // Calculate total amount based on the products (you might have a more sophisticated logic)
            decimal totalAmount = CalculateTotalAmount(products);

            var orderID = await _orderManagementService.CreateOrder(userID, products, totalAmount);

            if (orderID > 0)
            {
                return Ok(orderID);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create the order");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderManagementService.GetOrder(id);

            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound("Order not found");
            }
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateModel statusUpdate)
        {
            if (statusUpdate == null || string.IsNullOrWhiteSpace(statusUpdate.Status))
            {
                return BadRequest("Invalid status update request");
            }

            var success = await _orderManagementService.UpdateOrderStatus(id, statusUpdate.Status);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound("Order not found or failed to update status");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderManagementService.GetAllOrders();

            if (orders != null)
            {
                return Ok(orders);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve orders");
            }
        }

        // Helper method to calculate total amount (replace with your logic)
        private decimal CalculateTotalAmount(List<Product> products)
        {
            decimal totalAmount = 0;

            foreach (var product in products)
            {
                totalAmount += product.Price;
            }

            return totalAmount;
        }
    }

    // Models used in the controller
    public class OrderRequestModel
    {
        public int UserID { get; set; }
        public List<Product> Products { get; set; }
    }

    public class OrderStatusUpdateModel
    {
        public string Status { get; set; }
    }
}
