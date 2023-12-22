using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductManagementService _productManagementService;

        public ProductsController(ILogger<ProductsController> logger, IProductManagementService productManagementService)
        {
            _logger = logger;
            _productManagementService = productManagementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Getting all Products");
            var products = await _productManagementService.GetAllProducts();

            if (products != null)
            {
                return Ok(products);
            }
            else
            {
                return BadRequest("Error retrieving products");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogError("Get Product Error with Id" + id);
            var product = await _productManagementService.GetProduct(id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest("Error retrieving product");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> CreateProduct([FromBody] Product productDto)
        {
            if (productDto == null)
            {
                return BadRequest(productDto);
            }

            if (productDto.ProductID < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _productManagementService.CreateProduct(
                productDto.ProductName,
                productDto.Description,
                productDto.Price,
                productDto.Rating,
                productDto.StockQuantity,
                productDto.InStock,
                productDto.Image,
                productDto.VideoURL,
                productDto.ReturnPolicy,
                productDto.WarrantyInformation,
                productDto.ManufacturerID,
                productDto.CategoryID
            );

            return 1;
        }

        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        public async Task<ActionResult<int>> DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            await _productManagementService.DeleteProduct(id);
            return 1;
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product productDto)
        {
            if (productDto == null || id != productDto.ProductID)
            {
                return BadRequest();
            }

            await _productManagementService.UpdateProduct(id, productDto);
            return NoContent();
        }
    }
}
