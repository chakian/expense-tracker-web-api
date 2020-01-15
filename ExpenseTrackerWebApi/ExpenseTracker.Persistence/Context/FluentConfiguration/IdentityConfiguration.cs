using ExpenseTracker.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class IdentityConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasOne(s => s.InsertUser)
                .WithMany()
                .HasForeignKey(e => e.InsertUserId);

            modelBuilder.Entity<User>()
                .HasOne(s => s.UpdateUser)
                .WithMany()
                .HasForeignKey(e => e.UpdateUserId);
        }
    }
}
