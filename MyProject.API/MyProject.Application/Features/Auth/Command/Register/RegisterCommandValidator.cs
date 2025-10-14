using FluentValidation;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.Auth.Command.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IUserRepository _userRepository;
        public RegisterCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(x => x.Data.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, cancellation) =>
                    (await _userRepository.GetUserByEmailAsync(email)) == null)
                .WithMessage("Email already exists.");

            RuleFor(x => x.Data.Phone)
                .Matches(@"^(03|05|07|08|09|01[2|6|8|9])[0-9]{8}$")
                .WithMessage("'{PropertyName}' must be a valid phone number.")
                .MustAsync(async (phone, cancellation) =>
                    (await _userRepository.GetUserByPhoneAsync(phone)) == null)
                .WithMessage("Phone already exists.");

            RuleFor(p => p.Data.Password)
                .NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[!@#$%^&*()_+\-=\[\]{};:'"",.<>?/~]").WithMessage("Your password must contain at least one (!@#$%^&*()_+\\-=\\[\\]{};:'\",.<>?/~).");

            RuleFor(x => x.Data.ConfirmPassword)
                .Equal(x => x.Data.Password)
                .WithMessage("Passwords do not match.");
        }
    }
}
