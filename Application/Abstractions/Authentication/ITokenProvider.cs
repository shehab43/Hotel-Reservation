using Domain.Entities.Users;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Authentication
{
    public interface ITokenProvider
    {
        string Create(User user);
    }
}
