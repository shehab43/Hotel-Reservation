using Domain.Abstractions.Contracts;
using Domain.Dtos;
using Domain.Entities.EmaiVerification;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EmailVerfication : IEmailVerfication
    {
        private readonly ApplicationDbContext _context;

        public EmailVerfication(ApplicationDbContext Context)
        {
            _context = Context;
        }
        public async Task<EmailVerificationToken?> Get(Guid id) =>
                   await _context.EmailVerificationTokens
                                 .Where(e => e.Id == id)
                                 .Select(e => new EmailVerificationToken
                                 {
                                     UserId = e.UserId,
                                     ExpirationDate = e.ExpirationDate,
                                     User = e.User 
                                   })
                                   .AsTracking()
                                 .FirstOrDefaultAsync();
                        
        public async Task add(EmailVerificationToken emailVerificationToken , CancellationToken cancellationToken = default)
        {
            await _context.EmailVerificationTokens.AddAsync(emailVerificationToken);

        }
        public async Task  Delete(Guid id)
        {
            var email = await _context.EmailVerificationTokens.FindAsync(id);

            if (email is null)
                return;
            
          _context.EmailVerificationTokens.Remove(email);  
        }

        public async Task<int> SaveChanges()
        {

            var count =  _context.SaveChanges();
            return count;
        }
    }

}
