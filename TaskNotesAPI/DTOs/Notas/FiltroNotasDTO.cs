using TaskNotesAPI.Entities;

namespace TaskNotesAPI.DTOs.Notas
{
    public class FiltroNotasDTO
    {
        public string? Buscar { get; set; }
        public PrioridadNota? Prioridad { get; set; }
        public int? CategoriaId { get; set; }
        public bool? EsImportante { get; set; }

        public int Pagina { get; set; } = 1;
        public int CantidadPorPagina { get; set; } = 10;
    }
}
