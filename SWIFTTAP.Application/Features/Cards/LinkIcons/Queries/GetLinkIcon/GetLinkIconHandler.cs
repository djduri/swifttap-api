using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Helpers;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Queries.GetLinkIcon;

internal sealed class GetLinkIconHandler : IQueryHandler<GetLinkIconQuery, FileInMemory>
{
    private readonly DatabaseContext _dbContext;

    public GetLinkIconHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FileInMemory> Handle(GetLinkIconQuery request, CancellationToken cancellationToken)
    {
        var linkIcon = await _dbContext.Links.AsNoTracking()
                                             .Include(x => x.LinkIcon)
                                             .Select(x => new { x.Id, x.LinkIcon.Content, x.LinkIcon.ContentType })
                                             .SingleOrDefaultAsync(x => x.Id == request.LinkId, cancellationToken);

        if (linkIcon is null || linkIcon.Content is null)
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.LinkIcon.NotFound);

        var extension = linkIcon.ContentType switch
        {
            "image/svg+xml" => "svg", // SVG
            "image/jpeg" => "jpg",    // JPG
            "image/png" => "png",     // PNG
            "image/bmp" => "bmp",     // BMP
            _ => throw new InvalidOperationException("Unsupported image type.")
        };
        var linkIconFile = FileInMemory.CreateFromBytes(linkIcon.Content, $"link-icon-{request.LinkId}.{extension}", linkIcon.ContentType);

        return FileInMemory.CreateFromFileInMemory(linkIconFile, linkIconFile.Name);
    }
}

