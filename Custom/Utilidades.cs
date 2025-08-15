using app_restaurante_backend.Models.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace app_restaurante_backend.Custom
{
    public class Utilidades
    {

        private readonly IConfiguration _configuration;

        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncriptarSHA256(string s)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public string GenerarJWT(Usuario modelo)
        {
            Claim[] userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.Id.ToString()),
                new Claim(ClaimTypes.Email, modelo.Correo),
                new Claim(ClaimTypes.Role, modelo.Rol.ToString())
            };

            SymmetricSecurityKey? securityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials? credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken? jwtConfig = new(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

    }
}
