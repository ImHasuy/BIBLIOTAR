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
        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //user config

            modelBuilder.Entity<User>()
                .HasMany(x => x.Reservations)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Borrows)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //reservation config

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Book)
                .WithOne()
                .HasForeignKey<Reservation>(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            //borrow config

            modelBuilder.Entity<Borrow>()
                .HasOne(x => x.Book)
                .WithOne()
                .HasForeignKey<Borrow>(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            //fine config 

            modelBuilder.Entity<Fine>()
                .HasOne(x => x.Borrow)
                .WithOne(x => x.Fine)
                .HasForeignKey<Fine>(x => x.BorrowId)
                .OnDelete(DeleteBehavior.Restrict);
                

        }
    }
}
