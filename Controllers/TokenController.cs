
namespace webapi.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        readonly ISrvUsuario srvUsuario;
        readonly ISrvToken srvToken;
        private readonly JwtConfig ojwt;

        public TokenController(ISrvToken srvt, ISrvUsuario srvu, IConfiguration configuration)
        {
            srvToken = srvt;
            srvUsuario = srvu;
            ojwt = configuration.GetSection("Jwt").Get<JwtConfig>();
        }

        [HttpPost]
        [Route("refrescar")]
        public async Task<IActionResult> RefreshAsync(TokenApi tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            try
            {
                var principal = srvToken.GetPrincipalDesdeTokenExpirado(accessToken, ojwt);

                var username = principal.Identity.Name; 

                var usuario = await srvUsuario.EncontrarPorNombre(username); 

                if (usuario == null || usuario.RefreshToken != refreshToken || usuario.RefreshTokenExpiracion <= DateTime.Now)
                {
                    return BadRequest("Petición de cliente no valido");
                }
                var tokenExp = DateTime.Now.AddMinutes(30);
                var newAccessToken = srvToken.GenerarToken(ojwt, principal.Claims, tokenExp);

                var newRefreshToken = srvToken.GenerarRefreshToken();
                var refreshTokenExp = DateTime.Now.AddDays(7);
                await srvUsuario.GuardarRefreshToken(usuario, newRefreshToken, refreshTokenExp);
                
                
                return new ObjectResult(new
                {
                    access_token = newAccessToken,
                    expires_in = tokenExp,
                    refresh_token = newRefreshToken
                });
            }
            catch (Exception ex) {
                return BadRequest($"Error en refrescar. Mensaje: {ex.Message}");
            }
        }

        [HttpPost, Authorize]
        [Route("revocar")]
        public async Task<IActionResult> RevokeAsync()
        {            
            try
            {
                var usuarioNombre = User.Identity.Name;
                await srvUsuario.Revocar(usuarioNombre);
                return NoContent();
            }
            catch {
                return BadRequest();
            }
           
        }
    }
}
