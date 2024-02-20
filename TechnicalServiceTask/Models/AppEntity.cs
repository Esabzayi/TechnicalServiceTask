using Microsoft.EntityFrameworkCore;

namespace TechnicalServiceTask.Models
{
    public class AppEntity : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TechnicalService> TechnicalServices { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<System> Systems { get; set; }
        public DbSet<Employee> ResponsiblePersons { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<TechnicalRequest> TechnicalRequests { get; set; }

        public AppEntity(DbContextOptions<AppEntity> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TechnicalService>()
                .HasKey(ts => ts.Id);

            modelBuilder.Entity<TechnicalService>()
                .Property(ts => ts.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<TechnicalService>()
                .Property(ts => ts.Description)
                .HasMaxLength(4000);

            modelBuilder.Entity<Block>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Block>()
                .Property(b => b.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Block>()
         .HasAlternateKey(r => r.Code);

            modelBuilder.Entity<Block>()
                .Property(b => b.Code)
                .HasMaxLength(10);

            modelBuilder.Entity<System>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<System>()
                .Property(s => s.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<System>()
           .HasAlternateKey(r => r.Code);

            modelBuilder.Entity<System>()
                .Property(s => s.Code)
                .HasMaxLength(10);
               

            modelBuilder.Entity<Employee>()
                .HasKey(rp => rp.Id);

            modelBuilder.Entity<Employee>()
                .Property(rp => rp.FirstName)
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(rp => rp.Surname)
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(rp => rp.LastName)
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(rp => rp.PIN)
                .IsRequired();

            modelBuilder.Entity<Activity>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<TechnicalRequest>()
                .HasKey(tr => tr.Id);

            
        }
    }
}
