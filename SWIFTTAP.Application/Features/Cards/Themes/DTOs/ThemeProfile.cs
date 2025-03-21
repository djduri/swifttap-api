using AutoMapper;
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Application.Features.Cards.Themes.DTOs;
internal sealed class ThemeProfile : Profile
{
    public ThemeProfile()
    {
        CreateMap<Theme, ThemeDetailsDTO>();
        CreateMap<Theme, ThemeCardDTO>();
    }
}