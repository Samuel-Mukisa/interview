using Ishop.Domain.Entities;

namespace Ishop.Application.Interfaces;

public interface IAuthentication
{ 
    Task<int> CreateUser(Registration registration);
    Task<int> LoginUser(Registration registration);

}