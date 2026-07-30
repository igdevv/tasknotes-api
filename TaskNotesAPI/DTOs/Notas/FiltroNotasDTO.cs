using TaskNotesAPI.Entities;

namespace TaskNotesAPI.DTOs.Notas
{
    public class FiltroNotasDTO
    {
        public string? Buscar { get; set; }
        public PrioridadNota? Prioridad { get; set; }
        public int? CategoriaId { get; set; }
        public bool? EsImportante { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int Pagina { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Range(1, 100)]
        public int CantidadPorPagina { get; set; } = 10;
    }
}
