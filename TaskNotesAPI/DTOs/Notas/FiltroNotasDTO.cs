using TaskNotesAPI.Entities;

namespace TaskNotesAPI.DTOs.Notas
{
    public class FiltroNotasDTO
    {
        public string? Buscar { get; set; }
        public PrioridadNota? Prioridad { get; set; }
        public int? CategoriaId { get; set; }
        public bool? EsImportante { get; set; }
    }
}
