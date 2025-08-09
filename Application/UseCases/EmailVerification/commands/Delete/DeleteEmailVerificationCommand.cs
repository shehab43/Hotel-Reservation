using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EmailVerification.commands.Delete
{
    public sealed record  DeleteEmailVerificationCommand(Guid EmailVerificationId):IRequest;
  
}
