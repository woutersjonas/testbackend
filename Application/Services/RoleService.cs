using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Infrastructure.Database.Transactions;
using Microsoft.AspNetCore.Identity;

namespace jonas.Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Role> AddRole(Role role)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            Role newRole = new Role
            {
                Name = role.Name,
            };

            IdentityResult result = await _roleManager.CreateAsync(newRole);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return newRole;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<Role> DeleteRole(string id)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            Role role = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return role;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<Role> GetRole(string id)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            Role role = await _roleManager.FindByIdAsync(id);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return role;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<bool> DoesRoleExist(string role)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            var result = await _roleManager.RoleExistsAsync(role);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return result;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
