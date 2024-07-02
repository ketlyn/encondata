using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechsysLog.Api.Entities;
using TechsysLog.Core.Data;

namespace TechsysLog.Api.Data
{
    public class TechsysLogDBContext : DbContext, IUnitOfWork
    {
        public TechsysLogDBContext(DbContextOptions<TechsysLogDBContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(50)");
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull; // desabilitando delete em cascata
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechsysLogDBContext).Assembly);

            // Configuração para a entidade Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number)
                      .IsRequired()
                      .HasColumnType("varchar(50)");
                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasColumnType("varchar(100)");
                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");
                entity.Property(e => e.UpdateDate)
                      .IsRequired()
                      .HasColumnType("datetime");

                entity.HasOne(e => e.Address)
                      .WithMany()
                      .HasForeignKey("AddressId");
            });

            // Configuração para a entidade Delivery
            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number)
                      .IsRequired()
                      .HasColumnType("varchar(50)");
                entity.Property(e => e.DeliveryDate)
                      .IsRequired()
                      .HasColumnType("datetime");
                entity.HasOne(e => e.Order)
                      .WithMany()
                      .HasForeignKey("OrderNumberId");
            });

            // Configuração para a entidade Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CEP)
                      .IsRequired()
                      .HasColumnType("varchar(20)");
                entity.Property(e => e.Street)
                      .IsRequired()
                      .HasColumnType("varchar(100)");
                entity.Property(e => e.Number)
                      .IsRequired()
                      .HasColumnType("int");
                entity.Property(e => e.Neighborhood)
                      .IsRequired()
                      .HasColumnType("varchar(50)");
                entity.Property(e => e.City)
                      .IsRequired()
                      .HasColumnType("varchar(50)");
                entity.Property(e => e.State)
                      .IsRequired()
                      .HasColumnType("varchar(50)");
            });
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
