namespace TaskNotesAPI.DTOs.Auth
{
    public class AuthRespuestaDTO
    {
        public string UsuarioId { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiracionToken { get; set; }
    }
}
