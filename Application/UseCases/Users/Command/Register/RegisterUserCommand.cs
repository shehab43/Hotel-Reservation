using Domain.Entities.Users;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Command.Register
{
    public sealed record RegisterUserCommand(
                           string Email, 
                           string FirstName,
                           string LastName,
                           string Password):
                           IRequest<Result<User>>;
    
}
