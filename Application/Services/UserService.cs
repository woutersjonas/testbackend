using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Infrastructure.Database.Transactions;
using Microsoft.AspNetCore.Identity;

namespace jonas.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UserService (UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> AddUser(User entity, string password)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            User user = new User
            {
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return user;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<User> DeleteUser(string id)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            User user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.DeleteAsync(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return user;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<User> GetUser(string id)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            User user = await _userManager.FindByIdAsync(id);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return user;
        }
        catch(Exception e)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
