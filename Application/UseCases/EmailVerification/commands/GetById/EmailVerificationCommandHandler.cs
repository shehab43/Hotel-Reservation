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

namespace Application.UseCases.EmailVerification.commands.GetById
{
    public class EmailVerificationCommandHandler : IRequestHandler<EmailVerificationCommand, Result<bool>>
    {
        private readonly IEmailVerfication _emailVerfication;

        public EmailVerificationCommandHandler(IEmailVerfication emailVerfication)
        {
            _emailVerfication = emailVerfication;
        }
        public async Task<Result<bool>> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
        {
            var verification = await _emailVerfication.Get(request.Id);

            if (verification is null || verification.ExpirationDate < DateTime.UtcNow || verification.User.EmailVerified)
            {
                return Result.Failure<bool>(EmailVerificationErrors.TokenIsExpire);
            }
            
                   verification.User.EmailVerified = true;
            await _emailVerfication.Delete(request.Id);
            await _emailVerfication.SaveChanges();
            return Result.Success(true);
        }
    }
}
