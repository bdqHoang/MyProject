using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Auth.Command.Loggin
{
    public class AccessTokenCommandValidator : AbstractValidator<AccessTokenCommand>
    {
        public AccessTokenCommandValidator() {
            RuleFor(x => x.data.Email)
                .NotEmpty().WithMessage("Email is require")
                .EmailAddress();
            RuleFor(x => x.data.Password)
                .NotEmpty().WithMessage("Passwork is require");
        }
    }
}
