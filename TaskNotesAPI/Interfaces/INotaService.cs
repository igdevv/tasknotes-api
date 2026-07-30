using TaskNotesAPI.DTOs.Notas;

namespace TaskNotesAPI.Interfaces
{
    public interface INotaService
    {
        Task<NotaDTO> CrearAsync(
        CrearNotaDTO crearNotaDTO,
        string usuarioId,
        CancellationToken cancellationToken);

        Task<List<NotaDTO>> ObtenerTodasAsync(
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
    }
}
