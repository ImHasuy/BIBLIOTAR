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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=FishingShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Borrow>()
                .HasOne(u => u.User)
                .WithMany(u => u.Borrows)
                .HasForeignKey(u => u.ReferenceEmail);

            modelBuilder.Entity<Reservation>()
                .HasOne(u => u.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(u => u.ReferenceEmail);
            
        }
    }
}
