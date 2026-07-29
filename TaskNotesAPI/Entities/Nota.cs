namespace TaskNotesAPI.Entities
{
    public class Nota
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Contenido { get; set; } = null!;

        public PrioridadNota Prioridad { get; set; }

        public bool EsImportante { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public string UsuarioId { get; set; } = null!;
        public UsuarioAplicacion Usuario { get; set; } = null!;
    }
}
