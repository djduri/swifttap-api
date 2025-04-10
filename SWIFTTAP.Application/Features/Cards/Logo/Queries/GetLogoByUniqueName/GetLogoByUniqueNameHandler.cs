using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Helpers;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.Logo.Queries.GetLogoByUniqueName;
internal sealed class GetLogoByUniqueNameHandler : IQueryHandler<GetLogoByUniqueNameQuery, FileInMemory>
{
    private readonly DatabaseContext _dbContext;

    public GetLogoByUniqueNameHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FileInMemory> Handle(GetLogoByUniqueNameQuery request, CancellationToken cancellationToken)
    {
        var logo = await _dbContext.Cards.AsNoTracking()
                                         .Include(x => x.Logo)
                                         .Select(x => new { x.Id, x.UniqueName, x.Logo.Content, x.Logo.ContentType })
                                         .SingleOrDefaultAsync(x => x.UniqueName == request.UniqueName, cancellationToken);

        if (logo is null)
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Logo.NotFound);

        if (logo.Content == null || logo.Content.Length == 0 || logo.ContentType is null)
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.Logo.NotDefined);

        var extension = logo.ContentType switch
        {
            "image/svg+xml" => "svg", // SVG
            "image/jpeg" => "jpg",    // JPG
            "image/png" => "png",     // PNG
            "image/bmp" => "bmp",     // BMP
            _ => throw new InvalidOperationException("Unsupported image type.")
        };
        var logoFile = FileInMemory.CreateFromBytes(logo.Content, $"user-logo-{logo.Id}.{extension}", logo.ContentType);

        return FileInMemory.CreateFromFileInMemory(logoFile, logoFile.Name);
    }
}
