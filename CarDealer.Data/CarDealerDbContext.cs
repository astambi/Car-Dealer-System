namespace CarDealer.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CarDealerDbContext : IdentityDbContext
    {
        public CarDealerDbContext(DbContextOptions<CarDealerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Many-to-Many
            builder
                .Entity<PartCar>()
                .HasKey(pc => new { pc.PartId, pc.CarId });

            builder
                .Entity<PartCar>()
                .HasOne(pc => pc.Car)
                .WithMany(c => c.Parts)
                .HasForeignKey(pc => pc.CarId);

            builder
                .Entity<PartCar>()
                .HasOne(pc => pc.Part)
                .WithMany(p => p.Cars)
                .HasForeignKey(pc => pc.PartId);

            // One-to-Many
            builder
               .Entity<Part>()
               .HasOne(p => p.Supplier)
               .WithMany(s => s.Parts)
               .HasForeignKey(p => p.SupplierId);

            builder
                .Entity<Sale>()
                .HasOne(s => s.Car)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CarId);

            builder
               .Entity<Sale>()
               .HasOne(s => s.Customer)
               .WithMany(c => c.Sales)
               .HasForeignKey(s => s.CustomerId);

            builder.Entity<Part>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // decimal
        }
    }
}
