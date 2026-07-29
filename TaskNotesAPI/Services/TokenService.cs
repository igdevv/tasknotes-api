using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskNotesAPI.DTOs.Auth;
using TaskNotesAPI.Entities;
using TaskNotesAPI.Interfaces;
using TaskNotesAPI.Settings;

namespace TaskNotesAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<UsuarioAplicacion> _userManager;

        public TokenService(
            IOptions<JwtSettings> jwtOptions,
            UserManager<UsuarioAplicacion> userManager)
        {
            _jwtSettings = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<TokenRespuestaDTO> GenerarTokenAsync(
            UsuarioAplicacion usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id),
            new(JwtRegisteredClaimNames.Email, usuario.Email!),
            new(ClaimTypes.NameIdentifier, usuario.Id),
            new(ClaimTypes.Name, usuario.Nombre),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credenciales = new SigningCredentials(
                clave,
                SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpirationMinutes);

            var descriptorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiracion,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credenciales
            };

            var manejadorToken = new JwtSecurityTokenHandler();

            var token = manejadorToken.CreateToken(descriptorToken);

            return new TokenRespuestaDTO
            {
                Token = manejadorToken.WriteToken(token),
                Expiracion = expiracion
            };
        }
    }
}
