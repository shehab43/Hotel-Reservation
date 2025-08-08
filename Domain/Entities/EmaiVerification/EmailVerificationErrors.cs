using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.EmaiVerification
{
    public class EmailVerificationErrors
    {

        public static readonly Error TokenIsExpire = Error.Confilct(
            "Email.VerificationNotFound",
            "Email Verification is Expire Or Not Found");

    }
}
