using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskNotesAPI.Entities;

namespace TaskNotesAPI.Data;

public class ApplicationDbContext
    : IdentityDbContext<UsuarioAplicacion>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Nota> Notas => Set<Nota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarCategoria(modelBuilder);
        ConfigurarNota(modelBuilder);
    }

    private static void ConfigurarCategoria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(categoria => categoria.Id);

            entity.Property(categoria => categoria.Nombre)
                .HasMaxLength(60)
                .IsRequired();

            entity.HasOne(categoria => categoria.Usuario)
                .WithMany(usuario => usuario.Categorias)
                .HasForeignKey(categoria => categoria.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurarNota(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(nota => nota.Id);

            entity.Property(nota => nota.Titulo)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(nota => nota.Contenido)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(nota => nota.Prioridad)
                .IsRequired();

            entity.Property(nota => nota.FechaCreacion)
                .IsRequired();

            entity.HasOne(nota => nota.Categoria)
                .WithMany(categoria => categoria.Notas)
                .HasForeignKey(nota => nota.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(nota => nota.Usuario)
                .WithMany(usuario => usuario.Notas)
                .HasForeignKey(nota => nota.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}