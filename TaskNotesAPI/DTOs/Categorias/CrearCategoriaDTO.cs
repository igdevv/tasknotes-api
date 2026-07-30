using System.ComponentModel.DataAnnotations;

namespace TaskNotesAPI.DTOs.Categorias
{
    public class CrearCategoriaDTO
    {
        [Required]
        [MaxLength(60)]
        public string Nombre { get; set; } = null!;
    }
}
