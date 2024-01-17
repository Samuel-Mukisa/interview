using System.Collections.Generic;
using System.Threading.Tasks;
using Ishop.Application.Interfaces;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/deliveryagents")]
    [ApiController]
    public class DeliveryAgentsController : ControllerBase
    {
        private readonly ILogger<DeliveryAgentsController> _logger;
        private readonly IDeliveryAgentManagement _deliveryAgentManagement;

        public DeliveryAgentsController(ILogger<DeliveryAgentsController> logger, IDeliveryAgentManagement deliveryAgentManagement)
        {
            _logger = logger;
            _deliveryAgentManagement = deliveryAgentManagement;
        }

        [HttpGet("getdeliveryagents")]
        public async Task<ActionResult<IEnumerable<DeliveryAgent>>> GetDeliveryAgents()
        {
            _logger.LogInformation("Getting all Delivery Agents");
            var deliveryAgents = await _deliveryAgentManagement.GetAllDeliveryAgents();

            if (deliveryAgents != null)
            {
                return Ok(deliveryAgents);
            }
            else
            {
                return BadRequest("Error retrieving delivery agents");
            }
        }

        [HttpGet("getdeliveryagent/{id:int}")]
        public async Task<ActionResult<DeliveryAgent>> GetDeliveryAgent(int id)
        {
            _logger.LogError("Get Delivery Agent Error with Id" + id);
            var deliveryAgent = await _deliveryAgentManagement.GetDeliveryAgent(id);

            if (deliveryAgent != null)
            {
                return Ok(deliveryAgent);
            }
            else
            {
                return BadRequest("Error retrieving delivery agent");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateDeliveryAgent([FromBody] DeliveryAgent deliveryAgentDto)
        {
            if (deliveryAgentDto == null)
            {
                return BadRequest(deliveryAgentDto);
            }

            if (deliveryAgentDto.AgentID < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var agentID = await _deliveryAgentManagement.CreateDeliveryAgent(deliveryAgentDto);

            return Ok(agentID);
        }

        [HttpDelete("delete/{id:int}", Name = "DeleteDeliveryAgent")]
        public async Task<ActionResult<bool>> DeleteDeliveryAgent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var isDeleted = await _deliveryAgentManagement.DeleteDeliveryAgent(id);

            return Ok(isDeleted);
        }

        [HttpPut("update/{id:int}", Name = "UpdateDeliveryAgent")]
        public async Task<IActionResult> UpdateDeliveryAgent(int id, [FromBody] DeliveryAgent deliveryAgentDto)
        {
            if (deliveryAgentDto == null || id != deliveryAgentDto.AgentID)
            {
                return BadRequest();
            }

            var isUpdated = await _deliveryAgentManagement.UpdateDeliveryAgent(id, deliveryAgentDto);

            return Ok(isUpdated);
        }
    }
}
