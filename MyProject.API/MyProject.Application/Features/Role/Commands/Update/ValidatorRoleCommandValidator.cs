using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Role.Commands.Update
{
    public class ValidatorRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public ValidatorRoleCommandValidator()
        {
            RuleFor(x => x.data.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
            RuleFor(x => x.data.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }
    }
}
