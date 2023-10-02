using AutoMapper;
using jonas.Application.Interfaces;
using jonas.Domain.Entities.Entities;
using jonas.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jonas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

    public RolesController(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _roleService.GetRole(id.ToString());
        var resultDTO = _mapper.Map<GetRoleDTO>(result);
        return Ok(resultDTO);
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpPost]
    public async Task<IActionResult> Post(PostRoleDTO roleDTO)
    {
        Role role = _mapper.Map<Role>(roleDTO);
        var result = await _roleService.AddRole(role);
        var resultDTO = _mapper.Map<GetRoleDTO>(result);
        return Ok(resultDTO);
    }

    [Authorize(Policy = "AdminAccess")]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roleService.DeleteRole(id.ToString());
        return Ok();
    }
}
