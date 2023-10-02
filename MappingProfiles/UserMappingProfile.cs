using AutoMapper;
using jonas.Domain.Entities.Entities;
using jonas.Models.User;

namespace jonas.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<User, PostUserDTO>().ReverseMap();
            CreateMap<User, DeleteUserDTO>().ReverseMap();
        }
    }
}
