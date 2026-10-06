using Domain.CustomErrors;
using FluentValidation;

namespace Application.Accounts.Update;

internal sealed class ChangeAccountNameCommandValidator : AbstractValidator<ChangeAccountNameCommand>
{
    public ChangeAccountNameCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage(ValidationErrors.NullOrWhiteSpace.Description);
    }
}
