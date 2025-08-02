using Domain.Entities.Users;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Contracts
{
    public interface IEmailVerfication
    {
        Task<Result> Get (Guid token,CancellationToken cancellationToken = default);
    }
}
