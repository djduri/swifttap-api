using FluentValidation;

namespace SWIFTTAP.Application.Common;

public sealed class TranslationValidator : AbstractValidator<Translation>
{
	public TranslationValidator()
	{
		RuleFor(x => x.Language).MaximumLength(4).NotEmpty();
		RuleFor(x => x.Text).NotNull();
	}
}
