using Domain.Entities.Users;

namespace Domain.Entities.EmaiVerification
{

    public class EmailVerificationToken 
    {
        public Guid Id { get; set; }
        public DateTime CreateOnUtc { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
