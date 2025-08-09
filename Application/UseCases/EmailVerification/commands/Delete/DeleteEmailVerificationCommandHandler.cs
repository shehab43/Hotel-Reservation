using Domain.Abstractions.Contracts;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.EmailVerification.commands.Delete
{
    public class DeleteEmailVerificationCommandHandler : IRequestHandler<DeleteEmailVerificationCommand>
    {
        private readonly IEmailVerfication _emailVerfication;

        public DeleteEmailVerificationCommandHandler(IEmailVerfication emailVerfication)
        {
            _emailVerfication = emailVerfication;
        }

        public async Task<Unit> Handle(DeleteEmailVerificationCommand request, CancellationToken cancellationToken)
        {
          await  _emailVerfication.Delete(request.EmailVerificationId);
          return Unit.Value;
        }
    }
}
