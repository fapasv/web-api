

namespace webapi.Services
{
    public interface ISrvToken
    {
        string GenerarToken(JwtConfig configToken, IEnumerable<Claim> claims, DateTime expiration);
        string GenerarRefreshToken();

        ClaimsPrincipal GetPrincipalDesdeTokenExpirado(string token, JwtConfig configToken);


    }
    public class SrvToken : ISrvToken
    {
        public string GenerarToken(JwtConfig configToken, IEnumerable<Claim> claims, DateTime expiration)
        {
            var token = new JwtSecurityToken(
               expires: expiration,
               claims: claims,
               issuer: configToken.Issuer,
               audience: configToken.Audience,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configToken.Key)),
                                                                                        SecurityAlgorithms.HmacSha256)
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalDesdeTokenExpirado(string token, JwtConfig configToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configToken.Issuer,
                ValidAudience = configToken.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configToken.Key))
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();


            var principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    throw new SecurityTokenException("Token invalido");
                }
            }

            return principal;
        }
    }

}
