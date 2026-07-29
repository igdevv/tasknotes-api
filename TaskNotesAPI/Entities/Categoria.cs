using TaskNotesAPI.Entities;

public class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string UsuarioId { get; set; } = null!;
    public UsuarioAplicacion Usuario { get; set; } = null!;

    public ICollection<Nota> Notas { get; set; } = [];
}
