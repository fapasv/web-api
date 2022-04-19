using System;
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
        readonly ISrvToken srvToken;
        private readonly JwtConfig ojwt;

        public AutenticacionController(ISrvRol srvr, ISrvUsuario srvu, ISrvToken srvt, IConfiguration configuration)
        {
            srvRol = srvr;
            srvUsuario = srvu;
            srvToken = srvt;
            ojwt = configuration.GetSection("Jwt").Get<JwtConfig>();
        }

        /// <summary>
        /// Autenticación de usuario
        /// </summary>
        /// <param name="userModel">Esquema de usuario</param>
        /// <returns>Autorizado</returns>
        [HttpPost]
        public async Task<IActionResult> LogInAsync([Bind("Nombre,Password")] Autenticar userModel) {

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
                        var RolPermisoClaims = permisos.Select(p => new Claim("rol_permiso", p.Regla)).ToList();
                        if (RolPermisoClaims.Any()) { RolPermisoClaims.ForEach(p => authClaims.Add(p)); }
                    }
                }
            }
            var tokenExp = DateTime.Now.AddMinutes(30);
            var token = srvToken.GenerarToken(ojwt, authClaims, tokenExp);

            var refreshToken = srvToken.GenerarRefreshToken();
            var refreshTokenExp = DateTime.Now.AddDays(7);
            await srvUsuario.GuardarRefreshToken(userDto, refreshToken, refreshTokenExp);

            return Ok(new
            {
                access_token = token,
                expires_in = tokenExp,
                refresh_token = refreshToken             
            });
        }


    }
}
