using AutoMapper;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Application.Features.Cards.Links.DTOs;
internal sealed class LinkProfile : Profile
{
    public LinkProfile()
    {
        CreateMap<Link, LinkDetailsDTO>();
        CreateMap<Link, LinkCardDTO>();
    }
}

