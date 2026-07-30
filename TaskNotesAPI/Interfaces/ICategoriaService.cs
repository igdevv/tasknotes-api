using TaskNotesAPI.DTOs.Categorias;

namespace TaskNotesAPI.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaDTO> CrearAsync(CrearCategoriaDTO crearCategoriaDTO, 
            string usuarioId, 
            CancellationToken cancellationToken);

        Task<List<CategoriaDTO>> ObtenerTodasAsync(
            string usuarioId,
            CancellationToken cancellationToken);

        Task<CategoriaDTO?> ObtenerPorIdAsync(
            int categoriaId,
            string usuarioId,
            CancellationToken cancellationToken);

        Task<CategoriaDTO?> ActualizarAsync(
            int categoriaId,
            ActualizarCategoriaDTO actualizarCategoriaDTO,
            string usuarioId,
            CancellationToken cancellationToken);

        Task<bool> EliminarAsync(
            int categoriaId,
            string usuarioId,
            CancellationToken cancellationToken);
    }
}
