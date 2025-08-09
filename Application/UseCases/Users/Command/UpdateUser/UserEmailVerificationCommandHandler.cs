using Domain.Abstractions.Contracts;
using Domain.Entities.Users;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Command.UpdateUser
{
    public class UserEmailVerificationCommandHandler : IRequestHandler<UserEmailVerificationCommand, Result>
    {
        private readonly IGenericRepository<User> _genericRepository;

        public UserEmailVerificationCommandHandler(IGenericRepository<User> genericRepository)
        {
         _genericRepository = genericRepository;
        }

        public  async Task<Result> Handle(UserEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            var emailVerification = new User
            {
                Id = request.UserId,
                EmailVerified = true
            };

            _genericRepository.UpdateInclude(emailVerification , nameof(User.EmailVerified));
            var result =  await _genericRepository.SaveChangesAsync(cancellationToken) > 0;

            return result ?
                  Result.Success(true) :
                  Result.Failure(UserErrors.FaildToUpdateEmailVerification);
        }
    }
}
