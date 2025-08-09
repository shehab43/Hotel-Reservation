using Domain.Entities.Users;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Command.UpdateUser
{
    public sealed record UserEmailVerificationCommand(Guid  UserId):IRequest<Result>;
  
}
