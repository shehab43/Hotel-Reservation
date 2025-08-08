using Domain.Entities;
using Domain.Entities.EmaiVerification;
using Domain.Entities.Users;
using Infrastructure.Excetension;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database
{
    public class ApplicationDbContext(
                 DbContextOptions<ApplicationDbContext> options, 
                 IHttpContextAccessor httpContextAccessor) 
                 : DbContext(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entityentry in entries)
            {
                if (entityentry.State == EntityState.Added)
                {
                    // For new entities, if no current user, use a default value or skip
                    //?.Entity.CreatedById = currentUserId! ; // Use Guid.Empty as default
                    entityentry.Entity.CreatedById = currentUserId! ?? string.Empty ; 
                    entityentry.Entity.CreatedOn = DateTime.UtcNow;
                }
                else if (entityentry.State == EntityState.Modified)
                {
                    // Only update if there's a current user
                    if (currentUserId != null)
                    {
                        entityentry.Entity.UpdatedById = currentUserId;
                        entityentry.Entity.UpdatedOn = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
   
}
