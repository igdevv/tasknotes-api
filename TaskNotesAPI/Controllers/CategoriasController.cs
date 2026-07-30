using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskNotesAPI.DTOs.Categorias;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Crear(
        CrearCategoriaDTO crearCategoriaDTO,
        CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var categoria = await _categoriaService.CrearAsync(
                crearCategoriaDTO,
                usuarioId,
                cancellationToken);

            return Ok(categoria);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaDTO>>> ObtenerTodas(CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var categorias = await _categoriaService.ObtenerTodasAsync(
                usuarioId,
                cancellationToken);

            return Ok(categorias);
        }

        [HttpGet("{categoriaId:int}")]
        public async Task<ActionResult<CategoriaDTO>> ObtenerPorId(
            int categoriaId,
            CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var categoria = await _categoriaService.ObtenerPorIdAsync(
                categoriaId,
                usuarioId,
                cancellationToken);

            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPut("{categoriaId:int}")]
        public async Task<ActionResult<CategoriaDTO>> Actualizar(
             int categoriaId,
             ActualizarCategoriaDTO actualizarCategoriaDTO,
             CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var categoria = await _categoriaService.ActualizarAsync(
                categoriaId,
                actualizarCategoriaDTO,
                usuarioId,
                cancellationToken);

            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpDelete("{categoriaId:int}")]
        public async Task<IActionResult> Eliminar(
            int categoriaId,
            CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var eliminada = await _categoriaService.EliminarAsync(
                categoriaId,
                usuarioId,
                cancellationToken);

            if (!eliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
