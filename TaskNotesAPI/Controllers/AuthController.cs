using Microsoft.AspNetCore.Mvc;
using TaskNotesAPI.DTOs.Auth;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<AuthRespuestaDTO>> Registrar(RegistroDTO registroDTO, CancellationToken cancellationToken)
        {
            var respuesta = await _authService.RegistrarAsync(registroDTO, cancellationToken);
            return Ok(respuesta);
        }

    }
}
