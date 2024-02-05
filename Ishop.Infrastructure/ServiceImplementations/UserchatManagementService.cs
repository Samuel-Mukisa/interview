using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class UserchatManagementService : UserchatService
    {
        private readonly IConfiguration _configuration;
        public  UserchatManagementService(IConfiguration configuration)
        {
             _configuration = configuration;
        }


        Task<int> UserchatService.AddUser(User user)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<User>> UserchatService.GetAllUsers()
        {
            throw new NotImplementedException();
        }

        Task<User> UserchatService.GetUserById(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
