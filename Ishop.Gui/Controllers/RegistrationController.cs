using System.Data;
using Ishop.Application.Interfaces;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Ishop.Gui.Controllers
{
    [Route("api/v2/Registration")]
    public class RegistrationController
    {
        private readonly IConfiguration _configuration;
        //private readonly ICategoryManagementService _categoryManagementService;
        private readonly IAuthenticationService _authenticationService;

        public RegistrationController(IConfiguration configuration,IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<int> registration(Registration registration)
        {
            var status = await _authenticationService.CreateUser(registration);
            return status;
        }

        [HttpPost]
        [Route("login")]
        public async Task<int> login(Registration registration)
        {

            var status =await _authenticationService.LoginUser(registration);
            return status;
        }
                
        

    }
    
}

