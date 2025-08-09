using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Command.Register
{
    public class RegisterViewModel()
    {
        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
    };
    
}
