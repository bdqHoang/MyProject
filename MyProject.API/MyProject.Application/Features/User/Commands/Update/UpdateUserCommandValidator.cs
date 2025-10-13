using FluentValidation;
using MyProject.Application.Interface;

namespace MyProject.Application.Features.User.Commands.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UpdateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(x => x.data.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters");

            RuleFor(x => x.data.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(async (email, cancellation) =>
                (await _userRepository.GetUserByEmailAsync(email)) == null)
                .WithMessage("Email already exists");

            RuleFor(x => x.data.Phone)
                .Matches(@"^(03|05|07|08|09|01[2|6|8|9])[0-9]{8}$")
                .WithMessage("'{PropertyName}' must be a valid phone number.")
                .MustAsync(async (phone, cancellation) =>
                    (await _userRepository.GetUserByPhoneAsync(phone)) == null)
                .WithMessage("Phone already exists.");

            RuleFor(x => x.data.RoleId)
                .NotEmpty().WithMessage("Role is required")
                .MustAsync(async (roleId, cancellation) => (await _roleRepository.GetRoleByIdAsync(roleId)) != null).WithMessage("Role invalid");
        }
    }
}
