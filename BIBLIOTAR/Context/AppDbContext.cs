using BiblioTar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BiblioTar.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Fine>()
            .HasOne(f => f.Borrow)
            .WithOne(b => b.Fine)
            .HasForeignKey<Fine>(f => f.BorrowId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
