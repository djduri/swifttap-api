using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.System;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.UpdateTranslation;
internal sealed class UpdateTranslationHandler : ICommandHandler<UpdateTranslationCommand, long>
{
    private readonly IRepository<Translation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTranslationHandler(IRepository<Translation> repository,
                             IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(UpdateTranslationCommand request, CancellationToken cancellationToken)
    {
        var translation = await _repository.GetAsync(request.Id, cancellationToken) ??
            throw EntityUpdateException.FromErrorCode(ErrorCodes.Translation.NotFound);

        translation.SetName(request.Name)
                   .SetLanguage(request.Language!.Value)
                   .SetContent(request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return translation.Id;
    }
}