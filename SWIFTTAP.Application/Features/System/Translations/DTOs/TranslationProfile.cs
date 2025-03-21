using AutoMapper;
using SWIFTTAP.Domain.System;

namespace SWIFTTAP.Application.Features.System.Translations.DTOs;
internal sealed class TranslationProfile : Profile
{
    public TranslationProfile()
    {
        CreateMap<Translation, TranslationSimpleDTO>();

        CreateMap<Translation, TranslationDetailsDTO>();
    }
}

