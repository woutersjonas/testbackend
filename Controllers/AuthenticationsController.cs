using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace jonas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationsController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationsController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        try
        {
            var token = await _authenticationService.Login(model.Username, model.Password);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        } catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterDTO model)
    {
        try
        {
            User user = new User
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            await _authenticationService.Register(user, model.Password, model.IsAdmin);
            return Ok("User is created");
        } catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}

