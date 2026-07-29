namespace TaskNotesAPI.DTOs.Auth
{
    public class TokenRespuestaDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
    }
}
