using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;



namespace Ishop.Gui.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRegistrationService _customerRegistrationService;
        public CustomerController(ICustomerRegistrationService customerRegistrationService)
        {
            _customerRegistrationService= customerRegistrationService;
            
        }

        [HttpPost("users")]
        public async Task<IActionResult> RegisterCustomer(CustomerSignUp customerSignUp)
        {
            var addCustomer = await _customerRegistrationService.RegisterCustomer(customerSignUp);
            if (addCustomer == 1)
            {
            return Ok("Customer Added Successfully");
            }
            else
            {
                return BadRequest("Customer Not Added");
        }
        

        }

       
    }
}
