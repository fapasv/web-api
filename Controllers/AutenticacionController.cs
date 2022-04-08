using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/autenticar")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly ISrvRol srvRol;
        private readonly ISrvUsuario srvUsuario;
        private readonly JwtConfig ojwt;

        public AutenticacionController(ISrvRol srvr, ISrvUsuario srvu, IConfiguration configuration)
        {
            srvRol = srvr;
            srvUsuario = srvu;
            ojwt = configuration.GetSection("Jwt").Get<JwtConfig>();
        }

        /// <summary>
        /// Autenticación de usuario
        /// </summary>
        /// <param name="userModel">Esquema de usuario</param>
        /// <returns>Autorizado</returns>
        [HttpPost]
        public async Task<IActionResult> LogInAsync([Bind("Nombre,Password")] Usuario userModel) {

            var userDto = await srvUsuario.EncontrarPorNombre(userModel.Nombre); 
            if (userDto == null)
            {
                return NotFound();
            }

            // validando
            if (!BCrypt.Net.BCrypt.Verify(userModel.Password, userDto.Password))
            {
                return Unauthorized(new { message = "Credenciales no válidas." });
            }

            var authClaims = new List<Claim>{                    
                    new Claim(ClaimTypes.Name, userDto.Nombre)
            };

            var roles = await srvUsuario.ObtenerRoles(userDto); 
            if (roles.Any()) {
                foreach (var r in roles) {
                    authClaims.Add(new Claim(ClaimTypes.Role, r));
                    var rol = await srvRol.EncontrarPorNombre(r);
                    if (rol != null)
                    {
                        var permisos = await srvRol.ObtenerPermisos(rol);
                        var RolPermisoClaims = permisos.Select(p => new Claim("RolPermiso", p.Regla)).ToList();
                        if (RolPermisoClaims.Any()) { RolPermisoClaims.ForEach(p => authClaims.Add(p)); }
                    }
                }
            }

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                issuer: ojwt.Issuer,
                audience: ojwt.Audience,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ojwt.Key)),
                SecurityAlgorithms.HmacSha256Signature)
                );

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_in = token.ValidTo                
            });
        }


    }
}
