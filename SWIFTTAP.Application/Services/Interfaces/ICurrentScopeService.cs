using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Services.Interfaces;

internal interface ICurrentScopeService : ISingletonAppService
{
    Language GetLanguage(Language fallback = Language.PL);
    string? GetIpAddress();
    string? GetUserAgent();
}
