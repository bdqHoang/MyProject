using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Features.Message.Command.Create
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator() {
            RuleFor(x=> x.ReciverId)
                .NotEmpty().WithMessage("ReciverId is required");
            RuleFor(x=>x.SenderId)
                .NotEmpty().WithMessage("SenderId is required");
            RuleFor(x=>x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(1000).WithMessage("Content must be less than 1000 characters");
            RuleFor(x=>x.Type)
                .IsInEnum().WithMessage("Type is invalid");
        }
    }
}
