using Domain.Abstractions.Contracts;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers.EmailVerificationLink
{
    public class EmailVerificationLinkFactory: IEmailVerificationLinkFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }
        public string Create(Guid Token)
        {
            var verificationLink = _linkGenerator.GetUriByAction(
                _httpContextAccessor.HttpContext!,
                action: "VerifyEmail",
                controller: "Users",
                values: new { Token = Token.ToString() }
            );
            return verificationLink ??
                   throw new Exception("could not Create email verification link");
        }
    }
}
