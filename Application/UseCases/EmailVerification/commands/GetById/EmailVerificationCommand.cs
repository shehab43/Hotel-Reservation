using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EmailVerification.commands.GetById
{
    public sealed record EmailVerificationCommand(Guid Id): IRequest<Result<bool>>;
   
}
