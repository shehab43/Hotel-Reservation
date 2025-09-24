using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Query
{
    internal sealed class GetUserIdCommandValidator :AbstractValidator<GetUserIdCommand>
    {
        public GetUserIdCommandValidator()
        {
            RuleFor(u => u.UserId)
                .NotNull()
                .WithMessage("User ID is required.");
        }
    }
}
