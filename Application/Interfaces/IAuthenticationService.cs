using jonas.Domain.Entities.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace jonas.Application.Interfaces;

public interface IAuthenticationService
{
    Task<JwtSecurityToken> Login(string username, string password);
    Task Register(User user, string password, bool isAdmin);
}
