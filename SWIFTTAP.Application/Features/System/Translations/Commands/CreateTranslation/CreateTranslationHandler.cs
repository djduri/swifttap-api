using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Domain.System;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.System.Translations.Commands.CreateTranslation;
internal sealed class CreateTranslationHandler : ICommandHandler<CreateTranslationCommand, long>
{
    private readonly IRepository<Translation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTranslationHandler(IRepository<Translation> repository,
                             IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateTranslationCommand request, CancellationToken cancellationToken)
    {
        var translation = Translation.Factory.Create(request.Name, request.Language!.Value);

        _repository.Add(translation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return translation.Id;
    }
}