using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Application.Services
{
    public interface UserchatService
    {
            Task<User> GetUserById(int userId);
            Task<IEnumerable<User>> GetAllUsers();
            Task<int> AddUser(User user);
        
    }
}
