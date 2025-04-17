using AutoMapper;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.DTOs;
internal class DeletedUserProfile : Profile
{
    public DeletedUserProfile()
    {
        CreateMap<DeletedUser, DeletedUserDTO>();
    }
}
