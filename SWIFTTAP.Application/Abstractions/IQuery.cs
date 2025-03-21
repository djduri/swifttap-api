using MediatR;

namespace SWIFTTAP.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
