using ExpenseTracker.Persistence.Identity;
using System.Data.Entity;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class IdentityConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasOptional(s => s.InsertUser)
                .WithMany()
                .HasForeignKey(e => e.InsertUserId);

            modelBuilder.Entity<User>()
                .HasOptional(s => s.UpdateUser)
                .WithMany()
                .HasForeignKey(e => e.UpdateUserId);
        }
    }
}
