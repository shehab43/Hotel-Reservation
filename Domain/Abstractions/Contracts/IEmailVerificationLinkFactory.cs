using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Contracts
{
    public interface IEmailVerificationLinkFactory
    {
        public string Create(Guid Token);
    }
}
