using System.ComponentModel.DataAnnotations;

namespace TaskNotesAPI.DTOs.Auth
{
    public class RegistroDTO
    {
        [Required]
        [MaxLength(80)]
        public string Nombre { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
