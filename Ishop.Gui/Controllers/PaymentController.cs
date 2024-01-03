using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
       _paymentService = paymentService;
    }

    [HttpPost("charge")]
    public async Task<IActionResult> Charge([FromBody] Payment request)
    {
        try
        {
            var response = await _paymentService.MakeChargeRequest(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Handle exceptions, log, or return an error response
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
