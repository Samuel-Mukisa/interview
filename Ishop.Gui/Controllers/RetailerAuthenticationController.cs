using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Ishop.Gui.Controllers
{
    [Route("api/v1/RetailerAuthenication")]
    [ApiController]
    public class RetailerAuthenticationController : ControllerBase
    {
       
        private readonly IRetailerAuthenticationService _authenticationService;
        private readonly ILogger<RetailerAuthenticationController> _logger;
        private readonly IConfiguration _config;

        public RetailerAuthenticationController(IRetailerAuthenticationService authenticationService, ILogger<RetailerAuthenticationController> logger,IConfiguration config)
        {
            _config = config;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("Retailerregistration")]
        public async Task<IActionResult> registration([FromBody] RetailerRegistration registration)
        {
            try
            {

                var status = await _authenticationService.CreateRetailer(registration);
                if (status == 1)
                {
                    return Ok(status);
                }
                else
                {
                    _logger.LogInformation("No retailer is registered");
                    return NotFound("Retailer is not registered");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }



        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        [HttpPost("RetailerLogin/{username}/{password}")]
        public async Task<IActionResult> login([FromBody] UserDto registration)
        {
            try
            {
                var retailerlogin = await _authenticationService.LoginRetailer(registration.Username, registration.Password);
                if (retailerlogin == 1 )
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      null,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Username or password is incorrect.");
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error logging in user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }



    }
}
