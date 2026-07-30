namespace TaskNotesAPI.Helpers
{
    public class RespuestaPaginada<T>
    {
        public int Pagina { get; set; }
        public int CantidadPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Datos { get; set; } = [];
    }
}
