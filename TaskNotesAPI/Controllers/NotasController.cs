using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskNotesAPI.DTOs.Notas;
using TaskNotesAPI.Helpers;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Controllers
{
    [ApiController]
    [Route("api/notas")]
    [Authorize]
    public class NotasController : ControllerBase
    {
        private readonly INotaService _notaService;

        public NotasController(INotaService notaService)
        {
            _notaService = notaService;
        }

        [HttpPost]
        public async Task<ActionResult<NotaDTO>> Crear(
            CrearNotaDTO crearNotaDTO,
            CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var nota = await _notaService.CrearAsync(
                crearNotaDTO,
                usuarioId,
                cancellationToken);

            return Ok(nota);
        }

        [HttpGet]
        public async Task<ActionResult<RespuestaPaginada<NotaDTO>>> ObtenerTodas(
        [FromQuery] FiltroNotasDTO filtros,
        CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var resultado = await _notaService.ObtenerTodasAsync(
                filtros,
                usuarioId,
                cancellationToken);

            return Ok(resultado);
        }

        [HttpGet("{notaId:int}")]
        public async Task<ActionResult<NotaDTO>> ObtenerPorId(
            int notaId,
            CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var nota = await _notaService.ObtenerPorIdAsync(
                notaId,
                usuarioId,
                cancellationToken);

            if (nota is null)
            {
                return NotFound();
            }

            return Ok(nota);
        }

        [HttpPut("{notaId:int}")]
        public async Task<ActionResult<NotaDTO>> Actualizar(
        int notaId,
        ActualizarNotaDTO actualizarNotaDTO,
        CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var nota = await _notaService.ActualizarAsync(
                notaId,
                actualizarNotaDTO,
                usuarioId,
                cancellationToken);

            if (nota is null)
            {
                return NotFound();
            }

            return Ok(nota);
        }

        [HttpDelete("{notaId:int}")]
        public async Task<IActionResult> Eliminar(
            int notaId,
            CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var eliminada = await _notaService.EliminarAsync(
                notaId,
                usuarioId,
                cancellationToken);

            if (!eliminada)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{notaId:int}/importante")]
        public async Task<ActionResult<NotaDTO>> CambiarImportante(
        int notaId,
        CancellationToken cancellationToken)
        {
            var usuarioId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                return Unauthorized();
            }

            var nota = await _notaService.CambiarImportanteAsync(
                notaId,
                usuarioId,
                cancellationToken);

            if (nota is null)
            {
                return NotFound();
            }

            return Ok(nota);
        }
    }
}
