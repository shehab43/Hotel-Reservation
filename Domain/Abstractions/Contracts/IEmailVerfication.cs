using Domain.Dtos;
using Domain.Entities.EmaiVerification;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Contracts
{
    public interface IEmailVerfication
    {
        public Task<EmailVerificationToken?> Get(Guid id);
        public Task add(EmailVerificationToken emailVerificationToken, CancellationToken cancellationToken = default);
        public Task Delete(Guid Id);
        public Task<int> SaveChanges();

    }
}
