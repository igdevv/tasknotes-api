using Microsoft.AspNetCore.Mvc;
using TaskNotesAPI.DTOs.Auth;
using TaskNotesAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [HttpPost("login")]
        public async Task<ActionResult<AuthRespuestaDTO>> Login(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var respuesta = await _authService.LoginAsync(loginDTO, cancellationToken);
            return Ok(respuesta);
        }

        [Authorize]
        [HttpGet("perfil")]
        public ActionResult ObtenerPerfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new
            {
                usuarioId,
                nombre,
                email
            });
        }
    }
}
