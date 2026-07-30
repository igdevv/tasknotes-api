using Microsoft.EntityFrameworkCore;
using TaskNotesAPI.Data;
using TaskNotesAPI.DTOs.Categorias;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Services
{
    public class CategoriaService: ICategoriaService
    {
        private readonly ApplicationDbContext _context;
        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoriaDTO> CrearAsync(
        CrearCategoriaDTO crearCategoriaDTO,
        string usuarioId,
        CancellationToken cancellationToken)
        {
            var categoriaExistente = await _context.Categorias
                .AnyAsync(
                    categoria =>
                        categoria.UsuarioId == usuarioId &&
                        categoria.Nombre == crearCategoriaDTO.Nombre,
                    cancellationToken);

            if (categoriaExistente)
            {
                throw new InvalidOperationException(
                    "Ya tienes una categoría con ese nombre.");
            }

            var categoria = new Categoria
            {
                Nombre = crearCategoriaDTO.Nombre.Trim(),
                UsuarioId = usuarioId
            };

            _context.Categorias.Add(categoria);

            await _context.SaveChangesAsync(cancellationToken);

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<List<CategoriaDTO>> ObtenerTodasAsync(
            string usuarioId,
            CancellationToken cancellationToken)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(categoria => categoria.UsuarioId == usuarioId)
                .OrderBy(categoria => categoria.Nombre)
                .Select(categoria => new CategoriaDTO
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoriaDTO?> ObtenerPorIdAsync(
            int categoriaId,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(categoria =>
                    categoria.Id == categoriaId &&
                    categoria.UsuarioId == usuarioId)
                .Select(categoria => new CategoriaDTO
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CategoriaDTO?> ActualizarAsync(
            int categoriaId,
            ActualizarCategoriaDTO actualizarCategoriaDTO,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(
                    categoria =>
                        categoria.Id == categoriaId &&
                        categoria.UsuarioId == usuarioId,
                    cancellationToken);

            if (categoria is null)
            {
                return null;
            }

            var nombreNormalizado = actualizarCategoriaDTO.Nombre.Trim();

            var nombreDuplicado = await _context.Categorias
                .AnyAsync(
                    otraCategoria =>
                        otraCategoria.UsuarioId == usuarioId &&
                        otraCategoria.Id != categoriaId &&
                        otraCategoria.Nombre == nombreNormalizado,
                    cancellationToken);

            if (nombreDuplicado)
            {
                throw new InvalidOperationException(
                    "Ya tienes otra categoría con ese nombre.");
            }

            categoria.Nombre = nombreNormalizado;

            await _context.SaveChangesAsync(cancellationToken);

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<bool> EliminarAsync(
            int categoriaId,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(
                    categoria =>
                        categoria.Id == categoriaId &&
                        categoria.UsuarioId == usuarioId,
                    cancellationToken);

            if (categoria is null)
            {
                return false;
            }

            _context.Categorias.Remove(categoria);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
