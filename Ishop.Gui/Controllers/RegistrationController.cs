using System.Data;
using Ishop.Application.Interfaces;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;




namespace Ishop.Gui.Controllers
{
    [Route("api/v1/Registration")]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public RegistrationController(IConfiguration configuration, IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] Registration registration)
        {
            var status = await _authenticationService.CreateUser(registration);
            return Ok(status); // Returning Ok() for simplicity, you can customize based on your needs
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Registration registration)
        {
            var status = await _authenticationService.LoginUser(registration);
            return Ok(status); // Returning Ok() for simplicity, you can customize based on your needs
        }

        [HttpGet]
        [Route("getManufacturer/{id}")]
        public async Task<IActionResult> GetManufacturer(int id)
        {
            try
            {
                var manufacturer = await _authenticationService.GetManufacturerById(id);

                if (manufacturer != null)
                {
                    return Ok(manufacturer);
                }
                else
                {
                    return NotFound(); // You can customize the response for a not found manufacturer
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error"); // Return a generic 500 error response
            }
        }
    }
}
