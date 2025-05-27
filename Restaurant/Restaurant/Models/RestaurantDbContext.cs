using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Restaurant.Models;

public partial class RestaurantDbContext : DbContext
{
    public RestaurantDbContext()
    {
    }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Allergen> Allergens { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTable> ClientTables { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishOrder> DishOrders { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<MenuCategory> MenuCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=176.113.83.11;database=restaurant_db;user=mega_user;password=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.28-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.ToTable("admin");

            entity.Property(e => e.AdminId)
                .HasColumnType("int(11)")
                .HasColumnName("admin_id");
            entity.Property(e => e.AdminLogin)
                .HasMaxLength(30)
                .HasColumnName("admin_login");
            entity.Property(e => e.AdminPassword)
                .HasMaxLength(20)
                .HasColumnName("admin_password");
        });

        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasKey(e => e.AllergenId).HasName("PRIMARY");

            entity
                .ToTable("allergen")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.AllergenId)
                .HasColumnType("int(11)")
                .HasColumnName("allergen_id");
            entity.Property(e => e.AllergenName)
                .HasMaxLength(50)
                .HasColumnName("allergen_name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PRIMARY");

            entity
                .ToTable("client")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.ClientId)
                .HasColumnType("int(11)")
                .HasColumnName("client_id");
            entity.Property(e => e.ClientEmail)
                .HasMaxLength(30)
                .HasColumnName("client_email");
            entity.Property(e => e.ClientFirstname)
                .HasMaxLength(45)
                .HasColumnName("client_firstname");
            entity.Property(e => e.ClientName)
                .HasMaxLength(45)
                .HasColumnName("client_name");
            entity.Property(e => e.ClientPhone)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("client_phone");

            // Добавляем связь (если ещё не было)
            entity.HasMany(c => c.ClientTables)
                .WithOne(ct => ct.Client)
                .HasForeignKey(ct => ct.ClientId);
        });

        modelBuilder.Entity<ClientTable>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.TableId })
                .HasName("PRIMARY");

            entity
                .ToTable("client_table")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.ClientId, "client_fk_idx");

            entity.HasIndex(e => e.TableId, "table_fk_idx");

            entity.Property(e => e.ClientId)
                .HasColumnType("int(11)")
                .HasColumnName("client_id");

            entity.Property(e => e.TableId)
                .HasColumnType("int(11)")
                .HasColumnName("table_id");

            // Исправляем навигационные свойства
            entity.HasOne(d => d.Client)
                .WithMany(p => p.ClientTables) // Ссылка на коллекцию в Client
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("client_fk");

            entity.HasOne(d => d.Table)
                .WithMany(p => p.ClientTables) // Ссылка на коллекцию в Table
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("table_fk");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.DishId).HasName("PRIMARY");

            entity
                .ToTable("dish")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.MenuCategoryId, "menu_category_fk_idx");

            entity.Property(e => e.DishId)
                .HasColumnType("int(11)")
                .HasColumnName("dish_id");
            entity.Property(e => e.DishDescription)
                .HasColumnType("text")
                .HasColumnName("dish_description");
            entity.Property(e => e.DishImage)
                .HasMaxLength(45)
                .HasColumnName("dish_image");
            entity.Property(e => e.DishName)
                .HasMaxLength(50)
                .HasColumnName("dish_name");
            entity.Property(e => e.DishPrice).HasColumnName("dish_price");
            entity.Property(e => e.MenuCategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("menu_category_id");

            entity.HasOne(d => d.MenuCategory).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.MenuCategoryId)
                .HasConstraintName("menu_category_fk");

            entity.HasMany(d => d.Allergens).WithMany(p => p.Dishes)
                .UsingEntity<Dictionary<string, object>>(
                    "DishAllergen",
                    r => r.HasOne<Allergen>().WithMany()
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("allergen_fk"),
                    l => l.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("dish_fk"),
                    j =>
                    {
                        j.HasKey("DishId", "AllergenId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j
                            .ToTable("dish_allergen")
                            .UseCollation("utf8mb4_unicode_ci");
                        j.HasIndex(new[] { "AllergenId" }, "allergen_fk_idx");
                        j.IndexerProperty<int>("DishId")
                            .HasColumnType("int(11)")
                            .HasColumnName("dish_id");
                        j.IndexerProperty<int>("AllergenId")
                            .HasColumnType("int(11)")
                            .HasColumnName("allergen_id");
                    });
        });

        modelBuilder.Entity<DishOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("dish_order")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.DishId, "dish_order_fk_idx");

            entity.HasIndex(e => e.OrderId, "order_dish_fk_idx");

            entity.Property(e => e.DishId)
                .HasColumnType("int(11)")
                .HasColumnName("dish_id");
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("order_id");

            entity.HasOne(d => d.Dish).WithMany()
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dish_order_fk");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_dish_fk");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PRIMARY");

            entity
                .ToTable("feedback")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.ClientId, "client_fk_idx2");

            entity.Property(e => e.FeedbackId)
                .HasColumnType("int(11)")
                .HasColumnName("feedback_id");
            entity.Property(e => e.ClientId)
                .HasColumnType("int(11)")
                .HasColumnName("client_id");
            entity.Property(e => e.FeedbackDate).HasColumnName("feedback_date");
            entity.Property(e => e.FeedbackText)
                .HasColumnType("text")
                .HasColumnName("feedback_text");
            entity.Property(e => e.Rating)
                .HasColumnType("int(11)")
                .HasColumnName("rating");

            entity.HasOne(d => d.Client).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("freedback_cl");
        });

        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.HasKey(e => e.MenuCategoryId).HasName("PRIMARY");

            entity
                .ToTable("menu_category")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.MenuCategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("menu_category_id");
            entity.Property(e => e.MenuCategoryName)
                .HasMaxLength(40)
                .HasColumnName("menu_category_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity
                .ToTable("order")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.ClientId, "order_client_fk_idx");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("order_id");
            entity.Property(e => e.ClientId)
                .HasColumnType("int(11)")
                .HasColumnName("client_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(25)
                .HasColumnName("order_status");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("order_client_fk");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PRIMARY");

            entity
                .ToTable("table")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.TableId)
                .HasColumnType("int(11)")
                .HasColumnName("table_id");
            entity.Property(e => e.TableCapacity)
                .HasColumnType("int(11)")
                .HasColumnName("table_capacity");
            entity.Property(e => e.TableLocation)
                .HasMaxLength(30)
                .HasColumnName("table_location");
            entity.Property(e => e.TableStatus)
                .HasMaxLength(15)
                .HasColumnName("table_status");

            // Добавляем связь (если ещё не было)
            entity.HasMany(t => t.ClientTables)
                .WithOne(ct => ct.Table)
                .HasForeignKey(ct => ct.TableId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
