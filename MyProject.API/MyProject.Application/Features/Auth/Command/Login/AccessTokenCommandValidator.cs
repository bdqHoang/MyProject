using FluentValidation;

namespace MyProject.Application.Features.Auth.Command.Login
{
    public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
    {
        public AccessTokenCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is require")
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Passwork is require");
        }
    }
}
