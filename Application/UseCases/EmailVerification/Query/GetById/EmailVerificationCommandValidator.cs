using FluentValidation;
using System;

namespace Application.UseCases.EmailVerification.Query.GetById
{
    internal sealed class EmailVerificationCommandValidator : AbstractValidator<EmailVerificationCommand>
    {
        public EmailVerificationCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Email verification ID is required");
        }
    }
}