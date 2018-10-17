using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiJWT.Services
{
    public interface IAuthService
    {
        bool ValidateLogin(string user, string pass);
        string GenerateToken(DateTime date, string user, TimeSpan expiration);
    }

    public class AuthService : IAuthService
    {
        public AuthService()
        {

        }

        public bool ValidateLogin(string user, string pass)
        {
            return true;
        }

        public string GenerateToken(DateTime date, string user, TimeSpan expiration)
        {
            var expire = date.Add(expiration);

            // configuración de Claims
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(
                        JwtRegisteredClaimNames.Iat,
                        new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64
                    ),

                // Claim para rol opcional
                new Claim("roles","Cliente"),
                new Claim("roles","Administrador")
            };

            // Añadir las credenciales            
            var sigingCredenciales = new SigningCredentials(
                                                                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("aaAAbbBB12345sdsdsdsdsdsdsdfgfgfgfgfgfgfgfgfgfgxyz")),
                                                                SecurityAlgorithms.HmacSha256Signature
                                                            );

            // Configuración del token jwt
            var jwt = new JwtSecurityToken(
                issuer: "Ejemplo",
                audience: "Public",
                claims: claims,
                notBefore: date,
                expires: expire,
                signingCredentials: sigingCredenciales
            );

            try
            {
                var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return encodeJwt;
            }
            catch (Exception)
            {

            }

            return string.Empty;
        }
    }
}
