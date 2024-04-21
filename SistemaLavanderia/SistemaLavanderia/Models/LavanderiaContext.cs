using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaLavanderia.Models;

public partial class LavanderiaContext : DbContext
{
    public LavanderiaContext()
    {
    }

    public LavanderiaContext(DbContextOptions<LavanderiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatProducto> CatProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Lavadora> Lavadoras { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Abastecimiento> Abastecimientos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.;Database=Lavanderia;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatProducto>(entity =>
        {
            entity.HasKey(e => e.IdCatProducto).HasName("PK__CatProdu__09CE2F0E39576A6F");

            entity.ToTable("CatProducto");

            entity.Property(e => e.IdCatProducto).HasColumnName("idCatProducto");
            entity.Property(e => e.DescatProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descatProducto");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__885457EE8EF5432F");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__62EA894AF2B9DA0D");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.DesEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desEstado");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdProductos).HasName("PK__Inventar__A26E462DC07B508E");

            entity.ToTable("Inventario");

            entity.Property(e => e.IdProductos).HasColumnName("idProductos");
            entity.Property(e => e.CantidadProducto).HasColumnName("cantidadProducto");
            entity.Property(e => e.DesProductos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desProductos");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.Categoria)
                .HasConstraintName("FK__Inventari__Categ__3F466844");

            entity.HasOne(d => d.ServicioNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.Servicio)
                .HasConstraintName("FK__Inventari__Servi__5535A963");
        });

        modelBuilder.Entity<Lavadora>(entity =>
        {
            entity.HasKey(e => e.IdLavadoras).HasName("PK__Lavadora__169AC75E9E871D2B");

            entity.Property(e => e.IdLavadoras).HasColumnName("idLavadoras");
            entity.Property(e => e.DesLavadoras)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desLavadoras");
            entity.Property(e => e.Disponible).HasColumnName("disponible");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedidos__A9F619B7749B0CEE");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.CantPrendas).HasColumnName("cant_prendas");
            entity.Property(e => e.EstadoPedido).HasColumnName("estadoPedido");
            entity.Property(e => e.FechaPedido)
                .HasColumnType("datetime")
                .HasColumnName("fechaPedido");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio_total");

            entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Cliente)
                .HasConstraintName("FK__Pedidos__Cliente__47DBAE45");

            entity.HasOne(d => d.EstadoPedidoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.EstadoPedido)
                .HasConstraintName("FK__Pedidos__estadoP__4AB81AF0");

            entity.HasOne(d => d.LavadoraNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Lavadora)
                .HasConstraintName("FK__Pedidos__Lavador__49C3F6B7");

            entity.HasOne(d => d.ServicioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Servicio)
                .HasConstraintName("FK__Pedidos__Servici__48CFD27E");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F76825D7963");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.DesRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desRol");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__CEB98119B292C887");

            entity.Property(e => e.IdServicio).HasColumnName("idServicio");
            entity.Property(e => e.DesServicio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("desServicio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__645723A6CD184C8C");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Rol).HasColumnName("rol");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.RolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rol)
                .HasConstraintName("FK__Usuarios__rol__45F365D3");
        });

        modelBuilder.Entity<Abastecimiento>(entity =>
        {
            entity.HasKey(e => e.IdAbastecimiento);

            entity.Property(e => e.IdAbastecimiento).HasColumnName("IdAbastecimiento");
            entity.Property(e => e.Categoria).HasColumnName("Categoria");
            entity.Property(e => e.cantidadaIngresar).HasColumnName("cantidadaIngresar");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Abastecimientos)
                .HasForeignKey(d => d.Categoria);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
