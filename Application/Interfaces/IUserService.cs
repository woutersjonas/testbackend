using jonas.Domain.Entities.Entities;

namespace jonas.Application.Interfaces;

public interface IUserService
{
    Task<User> AddUser(User entity, string password);
    Task<User> DeleteUser(string password);
    Task<User> GetUser(string id);
}
