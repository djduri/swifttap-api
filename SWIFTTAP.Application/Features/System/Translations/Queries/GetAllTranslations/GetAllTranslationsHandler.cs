using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Features.System.Translations.DTOs;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.System.Translations.Queries.GetAllTranslations;

internal sealed class GetAllTranslationsHandler : IQueryHandler<GetAllTranslationsQuery, RangedDTO<TranslationSimpleDTO>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllTranslationsHandler(DatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<RangedDTO<TranslationSimpleDTO>> Handle(GetAllTranslationsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Translations.AsNoTracking();

        if (request.Filter is not null)
            query = query.Filter(x => x.Name, request.Filter);

        if (request.Language is not null)
            query = query.Where(x => x.Language == request.Language);

        if (request.SortingArguments is not null)
        {
            query = query.Sort(request.SortingArguments.SortBy, request.SortingArguments.Desc)
                         .With(x => x.Id, x => x.Name, x => x.Language)
                         .AsQueryable();
        }

        var result = await query.ToPagedListAsync(request.PaginationArguments, cancellationToken);
        return new RangedDTO<TranslationSimpleDTO>(_mapper.Map<IEnumerable<TranslationSimpleDTO>>(result),
                                                   result.TotalCount);
    }
}
