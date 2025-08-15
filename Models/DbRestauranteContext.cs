using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace app_restaurante_backend.Models;

public partial class DbRestauranteContext : DbContext
{
    public DbRestauranteContext()
    {
    }

    public DbRestauranteContext(DbContextOptions<DbRestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriasItem> CategoriasItems { get; set; }

    public virtual DbSet<DetalleOrdene> DetalleOrdenes { get; set; }

    public virtual DbSet<ItemsMenu> ItemsMenus { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-SNNEQG6;database=db_restaurante;integrated security=true;TrustServerCertificate=false;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriasItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_categorias_items");

            entity.ToTable("categorias_items");

            entity.HasIndex(e => e.Nombre, "uc_categorias_items_nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioMinimo).HasColumnName("precio_minimo");
        });

        modelBuilder.Entity<DetalleOrdene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_detalle_ordenes");

            entity.ToTable("detalle_ordenes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Igv).HasColumnName("igv");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");
            entity.Property(e => e.PlatoId).HasColumnName("plato_id");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.Orden).WithMany(p => p.DetalleOrdenes)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_ORDENES_ON_ORDEN");

            entity.HasOne(d => d.Plato).WithMany(p => p.DetalleOrdenes)
                .HasForeignKey(d => d.PlatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_ORDENES_ON_PLATO");
        });

        modelBuilder.Entity<ItemsMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_items_menu");

            entity.ToTable("items_menu");

            entity.HasIndex(e => e.Nombre, "uc_items_menu_nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EnlaceImagen)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("enlace_imagen");
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.ItemsMenus)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ITEMS_MENU_ON_CATEGORIA");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_mesas");

            entity.ToTable("mesas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Numero)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("numero");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_ordenes");

            entity.ToTable("ordenes");

            entity.HasIndex(e => e.CodigoOrden, "uc_ordenes_codigoorden").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CodigoOrden)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("codigo_orden");
            entity.Property(e => e.Estado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.HoraCreacion).HasColumnName("hora_creacion");
            entity.Property(e => e.MesaId).HasColumnName("mesa_id");
            entity.Property(e => e.MontoSubtotal).HasColumnName("monto_subtotal");
            entity.Property(e => e.MontoTotal).HasColumnName("monto_total");

            entity.HasOne(d => d.Mesa).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.MesaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDENES_ON_MESA");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83FC531BAF7");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "UQ__usuarios__2A586E0BE5BA34A4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Clave)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
