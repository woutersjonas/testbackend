using AutoMapper;
using jonas.Domain.Entities.Entities;
using jonas.Models.Role;
using jonas.Models.User;

namespace jonas.MappingProfiles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, GetRoleDTO>().ReverseMap();
        CreateMap<Role, PostRoleDTO>().ReverseMap();
        CreateMap<Role, DeleteRoleDTO>().ReverseMap();
    }
}
