using BiblioTar.ConnectionTables;
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
        public DbSet<UserRoles> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           //Otelem sincs, hogy ez most jo-e vagy nem //optionsBuilder.UseSqlServer("Server=adatb-mssql.mik.uni-pannon.hu;Database=h13_rd7nam;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
           

            optionsBuilder.UseSqlServer("Server=(local);Database=FishingShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=(local);Database=FishingShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=(local);Database=FishingShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

             //User 
            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);


            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(u => u.User)
                .HasForeignKey<User>(u => u.AddressId)
                .OnDelete(DeleteBehavior.Restrict); 

            //Userroles kapcsolotabla 
            modelBuilder.Entity<UserRoles>()
                .HasKey(u => new { u.UserEmail, u.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(u => u.UserEmail)
                .OnDelete(DeleteBehavior.Restrict); 



            //Ha nem mukodik akkor konvertalni kell

            modelBuilder.Entity<Borrow>()
                .HasOne(u => u.User)
                .WithMany(u => u.Borrows)
                .HasForeignKey(u => u.UserEmail)
                .OnDelete(DeleteBehavior.Restrict); 


            //Fine

            modelBuilder.Entity<Fine>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Fine>()
                .HasOne(u => u.Borrow)
                .WithOne(u => u.Fine)
                .HasForeignKey<Fine>(u => u.BorrowId)
                .OnDelete(DeleteBehavior.Restrict); 





            //modelBuilder.Entity<Loan>()
            //.HasOne(l => l.User)
            //.WithMany(u => u.Loans)
            //.HasForeignKey(l => l.UserEmail)
            //.HasPrincipalKey(u => u.Email);

            modelBuilder.Entity<Reservation>()
                .HasKey(u => u.Email);

            modelBuilder.Entity<Reservation>()
               .HasOne(u => u.User)
               .WithMany(u => u.Reservations)
               .HasForeignKey(u => u.Email)
               .HasPrincipalKey(u => u.Email)
               .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}
