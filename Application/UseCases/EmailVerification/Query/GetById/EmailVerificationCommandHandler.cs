using Application.UseCases.EmailVerification.commands.Delete;
using Application.UseCases.Users.Command.UpdateUser;
using Domain.Abstractions.Contracts;
using Domain.Entities.EmaiVerification;
using FluentValidation.Validators;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EmailVerification.Query.GetById
{
    public class EmailVerificationCommandHandler : IRequestHandler<EmailVerificationCommand, Result<bool>>
    {
        private readonly IEmailVerfication _emailVerfication;
        private readonly IMediator _mediator;

        public EmailVerificationCommandHandler(
               IEmailVerfication emailVerfication,
               IMediator mediator
            )
        {
            _emailVerfication = emailVerfication;
            _mediator = mediator;
        }
        public async Task<Result<bool>> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
        {
            var verification = await _emailVerfication.Get(request.Id);

            if (verification is null || verification.ExpirationDate < DateTime.UtcNow || verification.User.EmailVerified)
            {
                return Result.Failure<bool>(EmailVerificationErrors.TokenIsExpire);
            }
            var updateUserVerification  = new UserEmailVerificationCommand(verification.UserId);
            var deleteEmailVerification = new DeleteEmailVerificationCommand(request.Id);

           await _mediator.Send(updateUserVerification);
           await _mediator.Send(deleteEmailVerification);
           await _emailVerfication.SaveChanges();
           return Result.Success(true);
        }
    }
}
