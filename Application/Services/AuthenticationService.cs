using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jonas.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AuthenticationService(IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<JwtSecurityToken> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = AddClaims(user, userRoles);
            var token = GetToken(authClaims);

            return token;
        }
        throw new Exception("User not found");
    }

    public async Task Register(User userGiven, string password, bool isAdmin)
    {
        var userExists = await _userManager.FindByNameAsync(userGiven.UserName);
        if (userExists != null)
        {
            throw new Exception("User already exists");
        }

        User user = new()
        {
            FirstName = userGiven.FirstName,
            LastName = userGiven.LastName,
            Email = userGiven.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = userGiven.UserName,
            PhoneNumber = userGiven.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception("Problem with creating user");
        }

        if (isAdmin)
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new Role { Name = UserRoles.Admin });
            }
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        {
            await _roleManager.CreateAsync(new Role { Name = UserRoles.User });
        }

        await _userManager.AddToRoleAsync(user, UserRoles.User);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    private List<Claim> AddClaims(User user, IList<string> roles)
    {
        var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        foreach (var userRole in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        return authClaims;
    }
}
