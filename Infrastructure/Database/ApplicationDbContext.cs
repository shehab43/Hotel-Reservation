using Application.Abstractions;
using Domain.Entities;
using Domain.Entities.EmaiVerification;
using Domain.Entities.Users;
using Infrastructure.Excetension;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel;

namespace Infrastructure.Database
{
    public class ApplicationDbContext(
                 DbContextOptions<ApplicationDbContext> options,
                 IHttpContextAccessor httpContextAccessor
               )
                 : DbContext(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var entries = ChangeTracker.Entries<BaseEntity>();

            // Collect domain events before saving


            // Update audit fields
            foreach (var entityentry in entries)
            {
                if (entityentry.State == EntityState.Added)
                {
                    entityentry.Entity.CreatedById = currentUserId ?? string.Empty;
                    entityentry.Entity.CreatedOn = DateTime.UtcNow;
                }
                else if (entityentry.State == EntityState.Modified)
                {
                    if (currentUserId != null)
                    {
                        entityentry.Entity.UpdatedById = currentUserId;
                        entityentry.Entity.UpdatedOn = DateTime.UtcNow;
                    }
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;

        }

    }
}