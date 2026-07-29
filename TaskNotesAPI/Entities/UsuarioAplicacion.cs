using Microsoft.AspNetCore.Identity;

namespace TaskNotesAPI.Entities
{
    public class UsuarioAplicacion: IdentityUser
    {
        public string Nombre { get; set; } = null!;

        public ICollection<Categoria> Categorias { get; set; } = [];
        public ICollection<Nota> Notas { get; set; } = [];
    }
}
