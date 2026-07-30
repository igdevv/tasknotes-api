using Microsoft.EntityFrameworkCore;
using TaskNotesAPI.Data;
using TaskNotesAPI.DTOs.Notas;
using TaskNotesAPI.Entities;
using TaskNotesAPI.Helpers;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Services
{
    public class NotaService: INotaService
    {
        private readonly ApplicationDbContext _context;

        public NotaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotaDTO> CrearAsync(
        CrearNotaDTO crearNotaDTO,
        string usuarioId,
        CancellationToken cancellationToken)
        {
            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    categoria =>
                        categoria.Id == crearNotaDTO.CategoriaId &&
                        categoria.UsuarioId == usuarioId,
                    cancellationToken);

            if (categoria is null)
            {
                throw new InvalidOperationException(
                    "La categoría no existe o no te pertenece.");
            }

            var nota = new Nota
            {
                Titulo = crearNotaDTO.Titulo.Trim(),
                Contenido = crearNotaDTO.Contenido.Trim(),
                Prioridad = crearNotaDTO.Prioridad,
                EsImportante = false,
                FechaCreacion = DateTime.UtcNow,
                CategoriaId = categoria.Id,
                UsuarioId = usuarioId
            };

            _context.Notas.Add(nota);

            await _context.SaveChangesAsync(cancellationToken);

            return new NotaDTO
            {
                Id = nota.Id,
                Titulo = nota.Titulo,
                Contenido = nota.Contenido,
                Prioridad = nota.Prioridad,
                EsImportante = nota.EsImportante,
                FechaCreacion = nota.FechaCreacion,
                CategoriaId = categoria.Id,
                CategoriaNombre = categoria.Nombre
            };
        }

        public async Task<RespuestaPaginada<NotaDTO>> ObtenerTodasAsync(
        FiltroNotasDTO filtros,
        string usuarioId,
        CancellationToken cancellationToken)
        {
            var query = _context.Notas
                .AsNoTracking()
                .Where(nota => nota.UsuarioId == usuarioId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtros.Buscar))
            {
                var texto = filtros.Buscar.Trim();

                query = query.Where(nota =>
                    nota.Titulo.Contains(texto) ||
                    nota.Contenido.Contains(texto));
            }

            if (filtros.Prioridad.HasValue)
            {
                query = query.Where(nota =>
                    nota.Prioridad == filtros.Prioridad.Value);
            }

            if (filtros.CategoriaId.HasValue)
            {
                query = query.Where(nota =>
                    nota.CategoriaId == filtros.CategoriaId.Value);
            }

            if (filtros.EsImportante.HasValue)
            {
                query = query.Where(nota =>
                    nota.EsImportante == filtros.EsImportante.Value);
            }

            var totalRegistros = await query.CountAsync(cancellationToken);

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)filtros.CantidadPorPagina);

            var notas = await query
                .OrderByDescending(nota => nota.FechaCreacion)
                .Skip(
                    (filtros.Pagina - 1) *
                    filtros.CantidadPorPagina)
                .Take(filtros.CantidadPorPagina)
                .Select(nota => new NotaDTO
                {
                    Id = nota.Id,
                    Titulo = nota.Titulo,
                    Contenido = nota.Contenido,
                    Prioridad = nota.Prioridad,
                    EsImportante = nota.EsImportante,
                    FechaCreacion = nota.FechaCreacion,
                    CategoriaId = nota.CategoriaId,
                    CategoriaNombre = nota.Categoria.Nombre
                })
                .ToListAsync(cancellationToken);

            return new RespuestaPaginada<NotaDTO>
            {
                Pagina = filtros.Pagina,
                CantidadPorPagina = filtros.CantidadPorPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas,
                Datos = notas
            };
        }

        public async Task<NotaDTO?> ObtenerPorIdAsync(
            int notaId,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            return await _context.Notas
                .AsNoTracking()
                .Where(nota =>
                    nota.Id == notaId &&
                    nota.UsuarioId == usuarioId)
                .Select(nota => new NotaDTO
                {
                    Id = nota.Id,
                    Titulo = nota.Titulo,
                    Contenido = nota.Contenido,
                    Prioridad = nota.Prioridad,
                    EsImportante = nota.EsImportante,
                    FechaCreacion = nota.FechaCreacion,
                    CategoriaId = nota.CategoriaId,
                    CategoriaNombre = nota.Categoria.Nombre
                })
                .FirstOrDefaultAsync(cancellationToken);
        }


        public async Task<NotaDTO?> ActualizarAsync(
            int notaId,
            ActualizarNotaDTO actualizarNotaDTO,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            var nota = await _context.Notas
                .FirstOrDefaultAsync(
                    nota =>
                        nota.Id == notaId &&
                        nota.UsuarioId == usuarioId,
                    cancellationToken);

            if (nota is null)
            {
                return null;
            }

            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    categoria =>
                        categoria.Id == actualizarNotaDTO.CategoriaId &&
                        categoria.UsuarioId == usuarioId,
                    cancellationToken);

            if (categoria is null)
            {
                throw new InvalidOperationException(
                    "La categoría no existe o no te pertenece.");
            }

            nota.Titulo = actualizarNotaDTO.Titulo.Trim();
            nota.Contenido = actualizarNotaDTO.Contenido.Trim();
            nota.Prioridad = actualizarNotaDTO.Prioridad;
            nota.CategoriaId = categoria.Id;
            nota.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new NotaDTO
            {
                Id = nota.Id,
                Titulo = nota.Titulo,
                Contenido = nota.Contenido,
                Prioridad = nota.Prioridad,
                EsImportante = nota.EsImportante,
                FechaCreacion = nota.FechaCreacion,
                CategoriaId = nota.CategoriaId,
                CategoriaNombre = categoria.Nombre
            };
        }

        public async Task<bool> EliminarAsync(
            int notaId,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            var nota = await _context.Notas
                .FirstOrDefaultAsync(
                    nota =>
                        nota.Id == notaId &&
                        nota.UsuarioId == usuarioId,
                    cancellationToken);

            if (nota is null)
            {
                return false;
            }

            _context.Notas.Remove(nota);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<NotaDTO?> CambiarImportanteAsync(
            int notaId,
            string usuarioId,
            CancellationToken cancellationToken)
        {
            var nota = await _context.Notas
                .Include(nota => nota.Categoria)
                .FirstOrDefaultAsync(
                    nota =>
                        nota.Id == notaId &&
                        nota.UsuarioId == usuarioId,
                    cancellationToken);

            if (nota is null)
            {
                return null;
            }

            nota.EsImportante = !nota.EsImportante;

            await _context.SaveChangesAsync(cancellationToken);

            return new NotaDTO
            {
                Id = nota.Id,
                Titulo = nota.Titulo,
                Contenido = nota.Contenido,
                Prioridad = nota.Prioridad,
                EsImportante = nota.EsImportante,
                FechaCreacion = nota.FechaCreacion,
                CategoriaId = nota.CategoriaId,
                CategoriaNombre = nota.Categoria.Nombre
            };
        }
    }
}
