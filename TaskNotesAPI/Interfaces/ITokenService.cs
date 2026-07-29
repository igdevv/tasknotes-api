using TaskNotesAPI.DTOs.Auth;
using TaskNotesAPI.Entities;

namespace TaskNotesAPI.Interfaces
{
    public interface ITokenService
    {
        Task<TokenRespuestaDTO> GenerarTokenAsync(
        UsuarioAplicacion usuario);
    }
}
