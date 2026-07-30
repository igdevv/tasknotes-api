using TaskNotesAPI.Entities;

namespace TaskNotesAPI.DTOs.Notas
{
    public class NotaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public PrioridadNota Prioridad { get; set; }
        public bool EsImportante { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = null!;    
    }
}
