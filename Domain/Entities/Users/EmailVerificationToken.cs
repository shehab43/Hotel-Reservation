using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{

    public class EmailVerificationToken
    {
        public Guid Token { get; set; }
        public DateTime CreateOnUtc { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int UserId { get; set; }
        public  User User { get; set; }
    }
}
