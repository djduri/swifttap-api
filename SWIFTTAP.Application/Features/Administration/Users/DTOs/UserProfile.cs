using AutoMapper;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Features.Administration.Users.DTOs;

internal sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserSimpleDTO>();
        CreateMap<User, UserDetailsDTO>();
    }
}
