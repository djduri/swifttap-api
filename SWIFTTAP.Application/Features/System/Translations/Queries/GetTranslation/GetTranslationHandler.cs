using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.System.Translations.DTOs;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetTranslation;
internal sealed class GetTranslationHandler : IQueryHandler<GetTranslationQuery, TranslationDetailsDTO>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public GetTranslationHandler(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TranslationDetailsDTO> Handle(GetTranslationQuery request, CancellationToken cancellationToken)
    {
        var translation = await _dbContext.Translations.AsNoTracking()
                                                       .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return translation is null
            ? throw EntityNotFoundException.FromErrorCode(ErrorCodes.Translation.NotFound)
            : _mapper.Map<TranslationDetailsDTO>(translation);
    }
}
