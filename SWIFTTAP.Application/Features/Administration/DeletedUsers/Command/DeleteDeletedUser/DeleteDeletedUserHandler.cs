using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Command.DeleteDeletedUser;
internal sealed class DeleteDeletedUserHandler : ICommandHandler<DeleteDeletedUserCommand, long>
{
    private readonly IRepository<DeletedUser> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeletedUserHandler(IRepository<DeletedUser> repository,
              IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(DeleteDeletedUserCommand request, CancellationToken cancellationToken)
    {
        var deletedUser = await _repository.GetAsync(request.Id, cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.DeletedUser.NotFound);

        _repository.Delete(deletedUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}