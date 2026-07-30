using Microsoft.AspNetCore.Identity;
using TaskNotesAPI.DTOs.Auth;
using TaskNotesAPI.Entities;
using TaskNotesAPI.Interfaces;

namespace TaskNotesAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<UsuarioAplicacion> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthRespuestaDTO> RegistrarAsync(
            RegistroDTO registroDTO,
            CancellationToken cancellationToken)
        {
            var usuarioExistente = await _userManager
                .FindByEmailAsync(registroDTO.Email);

            if (usuarioExistente is not null)
            {
                throw new InvalidOperationException(
                    "Ya existe un usuario registrado con ese correo.");
            }

            var usuario = new UsuarioAplicacion
            {
                Nombre = registroDTO.Nombre.Trim(),
                Email = registroDTO.Email.Trim(),
                UserName = registroDTO.Email.Trim()
            };

            var resultado = await _userManager.CreateAsync(
                usuario,
                registroDTO.Password);

            if (!resultado.Succeeded)
            {
                var errores = string.Join(
                    ", ",
                    resultado.Errors.Select(error => error.Description));

                throw new InvalidOperationException(errores);
            }

            var tokenRespuesta = await _tokenService
                .GenerarTokenAsync(usuario);

            return new AuthRespuestaDTO
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email!,
                Token = tokenRespuesta.Token,
                ExpiracionToken = tokenRespuesta.Expiracion
            };
        }
        public async Task<AuthRespuestaDTO> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var usuario = await _userManager
                .FindByEmailAsync(loginDTO.Email);

            if (usuario is null)
            {
                throw new InvalidOperationException(
                    "Correo o contraseña incorrectos.");
            }

            var passwordValida = await _userManager
                .CheckPasswordAsync(usuario, loginDTO.Password);

            if (!passwordValida)
            {
                throw new InvalidOperationException(
                    "Correo o contraseña incorrectos.");
            }

            var tokenRespuesta = await _tokenService
                .GenerarTokenAsync(usuario);

            return new AuthRespuestaDTO
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email!,
                Token = tokenRespuesta.Token,
                ExpiracionToken = tokenRespuesta.Expiracion
            };
        }
    }
}