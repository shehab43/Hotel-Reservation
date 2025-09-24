using FluentValidation;
using System;

namespace Application.UseCases.Users.Command.UpdateUser
{
    internal sealed class UserEmailVerificationCommandValidator : AbstractValidator<UserEmailVerificationCommand>
    {
        public UserEmailVerificationCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}