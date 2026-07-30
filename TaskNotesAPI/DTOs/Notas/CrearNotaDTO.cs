using System.ComponentModel.DataAnnotations;
using TaskNotesAPI.Entities;

namespace TaskNotesAPI.DTOs.Notas
{
    public class CrearNotaDTO
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Contenido { get; set; } = null!;

        public PrioridadNota Prioridad { get; set; }

        [Required]
        public int CategoriaId { get; set; }
    }
}
