using FluentValidation;
using System;

namespace Application.UseCases.EmailVerification.commands.Delete
{
    internal sealed class DeleteEmailVerificationCommandValidator : AbstractValidator<DeleteEmailVerificationCommand>
    {
        public DeleteEmailVerificationCommandValidator()
        {
            RuleFor(c => c.EmailVerificationId)
                .NotEmpty()
                .WithMessage("Email verification ID is required");
        }
    }
}