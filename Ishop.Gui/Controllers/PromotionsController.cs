using Ishop.Application.Interfaces;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly ILogger<PromotionsController> _logger;
        private readonly IPromotionManagement _promotionManagement;

        public PromotionsController(ILogger<PromotionsController> logger, IPromotionManagement promotionManagement)
        {
            _logger = logger;
            _promotionManagement = promotionManagement;
        }

        [HttpGet("getpromotions")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions()
        {
            _logger.LogInformation("Getting all Promotions");
            var promotions = await _promotionManagement.GetAllPromotions();

            if (promotions != null)
            {
                return Ok(promotions);
            }
            else
            {
                return BadRequest("Error retrieving promotions");
            }
        }

        [HttpGet("getpromotion/{id:int}")]
        public async Task<ActionResult<Promotion>> GetPromotion(int id)
        {
            _logger.LogError("Get Promotion Error with Id" + id);
            var promotion = await _promotionManagement.GetPromotion(id);

            if (promotion != null)
            {
                return Ok(promotion);
            }
            else
            {
                return BadRequest("Error retrieving promotion");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreatePromotion([FromBody] Promotion promotionDto)
        {
            if (promotionDto == null)
            {
                return BadRequest(promotionDto);
            }

            if (promotionDto.PromotionId < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            await _promotionManagement.CreatePromotion(promotionDto);

            return 1;
        }

        [HttpDelete("delete/{id:int}", Name = "DeletePromotion")]
        public async Task<ActionResult<int>> DeletePromotion(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            await _promotionManagement.DeletePromotion(id);

            return 1;
        }

        [HttpPut("update/{id:int}", Name = "UpdatePromotion")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] Promotion promotionDto)
        {
            if (promotionDto == null || id != promotionDto.PromotionId)
            {
                return BadRequest();
            }

            await _promotionManagement.UpdatePromotion(id, promotionDto);

            return NoContent();
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchPromotions([FromQuery] string searchTerm)
        {
            try
            {
                var promotions = await _promotionManagement.SearchPromotions(searchTerm);

                if (promotions != null && promotions.Count > 0)
                {
                    return Ok(promotions);
                }
                else
                {
                    return NotFound("No promotions found matching the search term.");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Log.Error($"Error searching promotions: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
