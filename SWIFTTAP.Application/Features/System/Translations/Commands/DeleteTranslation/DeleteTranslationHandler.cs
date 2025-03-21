using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.System;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.DeleteTranslation;
internal sealed class DeleteTranslationHandler : ICommandHandler<DeleteTranslationCommand, long>
{
    private readonly IRepository<Translation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTranslationHandler(IRepository<Translation> repository,
                             IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(DeleteTranslationCommand request, CancellationToken cancellationToken)
    {
        var translation = await _repository.GetAsync(request.Id, cancellationToken) ??
                   throw EntityDeleteException.FromErrorCode(ErrorCodes.Translation.NotFound);
        
        _repository.Delete(translation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}