using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Excetension
{
    public static class UserExcetension
    {
        public static string? GetUserId(this ClaimsPrincipal user) =>

             user.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}
