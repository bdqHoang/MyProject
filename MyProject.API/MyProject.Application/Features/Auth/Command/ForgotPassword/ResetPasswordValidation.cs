using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Auth.Command.FogotPassword
{
    public class ResetPasswordValidation : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidation()
        {
            RuleFor(x => x.data.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(p => p.data.NewPassword)
                .NotEmpty().WithMessage("Your new password cannot be empty")
                .MinimumLength(8).WithMessage("Your new password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your new password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your new password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your new password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your new password must contain at least one number.")
                .Matches(@"[!@#$%^&*()_+\-=\[\]{};:'"",.<>?/~]").WithMessage("Your new password must contain at least one (!@#$%^&*()_+\\-=\\[\\]{};:'\",.<>?/~).");
            RuleFor(x => x.data.ConfirmNewPassword)
                .Equal(x => x.data.NewPassword)
                .WithMessage("New password and confirm new password do not match");
        }
    }
}
