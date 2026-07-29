using TaskNotesAPI.DTOs.Auth;

namespace TaskNotesAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthRespuestaDTO> RegistrarAsync(
        RegistroDTO registroDTO ,
        CancellationToken cancellationToken);
    }
}
