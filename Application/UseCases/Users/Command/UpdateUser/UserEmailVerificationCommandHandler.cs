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
    public class UserEmailVerificationCommandHandler : IRequestHandler<UserEmailVerificationCommand, Result<bool>>
    {
        private readonly IGenericRepository<User> _genericRepository;

        public UserEmailVerificationCommandHandler(IGenericRepository<User> genericRepository)
        {
         _genericRepository = genericRepository;
        }

        public  async Task<Result<bool>> Handle(UserEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            var emailVerification = new User
            {
                Id = request.UserId,
                EmailVerified = true
            };

            _genericRepository.UpdateInclude(emailVerification , e => e.EmailVerified);
            var result =  await _genericRepository.SaveChangesAsync(cancellationToken) > 0;

            return result ?
                  Result.Success<bool>(true) :
                  Result.Failure<bool>(UserErrors.FaildToUpdateEmailVerification);
        }
    }
}
