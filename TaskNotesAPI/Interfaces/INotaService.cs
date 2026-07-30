using TaskNotesAPI.DTOs.Notas;
using TaskNotesAPI.Helpers;

namespace TaskNotesAPI.Interfaces
{
    public interface INotaService
    {
        Task<NotaDTO> CrearAsync(
        CrearNotaDTO crearNotaDTO,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<RespuestaPaginada<NotaDTO>> ObtenerTodasAsync(
        FiltroNotasDTO filtros,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<NotaDTO?> ObtenerPorIdAsync(
        int notaId,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<NotaDTO?> ActualizarAsync(
        int notaId,
        ActualizarNotaDTO actualizarNotaDTO,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<bool> EliminarAsync(
        int notaId,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<NotaDTO?> CambiarImportanteAsync(
        int notaId,
        string usuarioId,
        CancellationToken cancellationToken);
    }
}
