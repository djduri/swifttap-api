using AutoMapper;
using SWIFTTAP.Application.Features.Cards.Cards.DTOs;
using SWIFTTAP.Domain.Cards;

public class CardProfile : Profile
{
    public CardProfile()
    {
        // Mapowanie Card -> CardDTO
        CreateMap<Card, CardShortDTO>();
        CreateMap<Card, CardDetailsDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
    }
}
