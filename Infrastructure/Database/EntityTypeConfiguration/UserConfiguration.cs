using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.EntityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email)
                   .IsUnique();
            builder.Property(u => u.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(u => u.LastName)
                   .HasMaxLength(100);
        }
    }

}
