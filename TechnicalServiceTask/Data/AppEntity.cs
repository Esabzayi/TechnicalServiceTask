using Microsoft.EntityFrameworkCore;

namespace TechnicalServiceTask.Data
{
    public class AppEntity : DbContext
    {
        public DbSet<TechnicalServiceBlock> TechnicalServiceBlocks { get; set; }
        public DbSet<TechnicalServiceSystem> TechnicalServiceSystems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TechnicalService> TechnicalServices { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<System> Systems { get; set; }
        public DbSet<Employee> Employees { get; set; }


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

            modelBuilder.Entity<TechnicalServiceBlock>()
       .HasKey(tsb => new { tsb.TechnicalServiceId, tsb.BlockId });

            modelBuilder.Entity<TechnicalServiceSystem>()
                .HasKey(tss => new { tss.TechnicalServiceId, tss.SystemId });

            modelBuilder.Entity<TechnicalServiceBlock>()
                .HasOne(tsb => tsb.TechnicalService)
                .WithMany(ts => ts.TechnicalServiceBlocks)
                .HasForeignKey(tsb => tsb.TechnicalServiceId);

            modelBuilder.Entity<TechnicalServiceSystem>()
                .HasOne(tss => tss.TechnicalService)
                .WithMany(ts => ts.TechnicalServiceSystems)
                .HasForeignKey(tss => tss.TechnicalServiceId);


            modelBuilder.Entity<TechnicalService>()
               .HasOne(ts => ts.CreatePerson)
               .WithMany(emp => emp.CreatedTechnicalServices)
               .HasForeignKey(ts => ts.CreatePersonId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TechnicalService>()
                .HasOne(ts => ts.ConfirmPerson)
                .WithMany(emp => emp.ConfirmedTechnicalServices)
                .HasForeignKey(ts => ts.ConfirmPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TechnicalService>()
                .HasOne(ts => ts.ApprovePerson)
                .WithMany(emp => emp.ApprovedTechnicalServices)
                .HasForeignKey(ts => ts.ApprovePersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TechnicalService>()
                .HasOne(ts => ts.VerifyPerson)
                .WithMany(emp => emp.VerifiedTechnicalServices)
                .HasForeignKey(ts => ts.VerifyPersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TechnicalService>()
                 .Property(ts => ts.CreatePersonNames)
                 .HasMaxLength(300);

            modelBuilder.Entity<TechnicalService>()
                .Property(ts => ts.ConfirmPersonNames)
                .HasMaxLength(300);

            modelBuilder.Entity<TechnicalService>()
                .Property(ts => ts.ApprovePersonNames)
                .HasMaxLength(300);

            modelBuilder.Entity<TechnicalService>()
                .Property(ts => ts.VerifyPersonNames)
                .HasMaxLength(300);

            modelBuilder.Entity<Employee>()
                .Property(ts => ts.PIN)
                .HasMaxLength(10);

            modelBuilder.Entity<Employee>()
               .Property(ts => ts.FirstName)
               .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
              .Property(ts => ts.LastName)
              .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
              .Property(ts => ts.Surname)
              .HasMaxLength(100);


        }
    }
}
