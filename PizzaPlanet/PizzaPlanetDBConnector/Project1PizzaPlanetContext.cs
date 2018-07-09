using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzaPlanet.DBData
{
    public partial class Project1PizzaPlanetContext : DbContext
    {
        public Project1PizzaPlanetContext()
        {
        }

        public Project1PizzaPlanetContext(DbContextOptions<Project1PizzaPlanetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pizza> Pizza { get; set; }
        public virtual DbSet<PizzaOrder> PizzaOrder { get; set; }
        public virtual DbSet<PizzaUser> PizzaUser { get; set; }
        public virtual DbSet<Store> Store { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //JSON
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.Code });

                entity.Property(e => e.OrderId)
                    .HasColumnName("OrderID")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Pizza)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pizza__OrderID__619B8048");
            });

            modelBuilder.Entity<PizzaOrder>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PizzaOrder)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PizzaOrde__Store__5CD6CB2B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PizzaOrder)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PizzaOrde__UserI__5DCAEF64");
            });

            modelBuilder.Entity<PizzaUser>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__PizzaUse__536C85E4653DC477")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bacon)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Beef)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BlackOlive)
                    .HasColumnName("Black_Olive")
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Cheese)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Dough)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GreenPepper)
                    .HasColumnName("Green_Pepper")
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ham)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Income).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mushroom)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NextOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.Onion)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Pepperoni)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sauce)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sausage)
                    .HasColumnType("decimal(7, 3)")
                    .HasDefaultValueSql("((0))");
            });
        }
    }
}
