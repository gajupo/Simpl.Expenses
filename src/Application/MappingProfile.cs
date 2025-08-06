
using AutoMapper;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
