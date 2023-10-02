using jonas.Domain.Entities.Entities;

namespace jonas.Application.Interfaces;

public interface IRoleService
{
    Task<Role> AddRole(Role entity);
    Task<Role> DeleteRole(string password);

    Task<Role> GetRole(string password);

    Task<bool> DoesRoleExist(string role);
}
