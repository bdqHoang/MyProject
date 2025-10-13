using FluentValidation;

namespace MyProject.Application.Features.Role.Commands.Delete
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Role ID is required.");
        }
    }
}
