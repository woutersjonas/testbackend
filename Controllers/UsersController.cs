using AutoMapper;
using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jonas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [Authorize(Policy = "UserAccess")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _userService.GetUser(id.ToString());
        var resultDTO = _mapper.Map<GetUserDTO>(result);
        return Ok(resultDTO);
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPost]
    public async Task<IActionResult> Post(PostUserDTO userDTO)
    {
        User user = _mapper.Map<User>(userDTO);
        var result = await _userService.AddUser(user, userDTO.Password);
        var resultDTO = _mapper.Map<GetUserDTO>(result);
        return Ok(resultDTO);
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteUser(id.ToString());
        return Ok();
    }
}
