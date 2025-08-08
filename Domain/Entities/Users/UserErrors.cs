using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound(Guid Id) => Error.NotFound(
            "Users.NotFound",
            $"The User with Id = {Id} was Not Found"
            );

        public static  Error FaildToUpdateEmailVerification = Error.Failure(
            "Users.FaildToUpdate",
            "Faild Email Verification "
            );

        public static Error Unauthorized() => Error.NotFound(
            "Users.Unauthorized",
            "You are not authorized to perform this action."
            );


        public static readonly Error NotFoundByEmail = Error.NotFound(
          "Users.NotFoundByEmail",
          "The user with the specified email was not found");


        public static readonly Error EmailNotUnique = Error.Confilct(
            "Users.EmailNotUnique",
            "The provided email is not unique");



    }
}
