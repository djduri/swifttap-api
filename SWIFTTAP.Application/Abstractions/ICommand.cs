using MediatR;

namespace SWIFTTAP.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
