using FluentValidation;
using MediatR;

namespace SWIFTTAP.Application.Pipelines;

public sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) =>
		_validators = validators;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken _)
	{
		if (!_validators.Any())
			return await next();

		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(
			_validators.Select(validator => validator.ValidateAsync(context)));

		var validationErrors = validationResults
			.Where(result => !result.IsValid)
			.SelectMany(result => result.Errors)
			.Where(x => x is not null)
			.OrderBy(x => x.PropertyName);

		if (validationErrors.Any())
			throw new ValidationException(validationErrors);

		return await next();
	}
}
