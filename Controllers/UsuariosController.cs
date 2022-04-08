
namespace webapi.Controllers
{
    [Authorize]
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ISrvUsuario srvUsuario;

        public UsuariosController(ISrvUsuario srvu)
        {
            srvUsuario = srvu;
        }

        /// <summary>
        /// Obtiene todos los Usuarios en la biblioteca
        /// </summary>
        /// <returns>Listado de Usuarios</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Usuario>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync() => Ok(await srvUsuario.Usuarios());

        /// <summary>
        /// Obtiene un Usuario que corresponda con el Identificador.
        /// </summary>
        /// <param name="id">Identificador de Usuario</param>
        /// <returns>Usuario</returns>
        [HttpGet("{usuario}")]
        [Authorize]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByID(string usuario)
        {
            return await srvUsuario.EncontrarPorNombre(usuario)
                 is Usuario usr
                     ? Ok(usr)
                     : NotFound();

        }

        /// <summary>
        /// Obtiene todos los roles por usuario especificado
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("usuario_roles")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RolesInUserAsync(string usuario)
        {
            var usr = await srvUsuario.EncontrarPorNombre(usuario);
            if (usr == null)
                return NotFound(new { message = $"Usuario {usuario} no encontrado" });

            var roles = await srvUsuario.ObtenerRoles(usr);
            return Ok(roles);
        }

        
        /// <summary>
        /// Agrega un nuevo Usuario
        /// </summary>
        /// <param name="Usuario">Objecto de tipo Usuario</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(Usuario usuario)
        {
            // validando usuario
            if (await srvUsuario.EncontrarPorNombre(usuario.Nombre) != null)
                return BadRequest(new { messaje = $"El usuario {usuario.Nombre} ya existe" });

            // hash password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            try
            {
                await srvUsuario.CrearUsuario(usuario);

                return Created($"/usuarios/{usuario.Id}", usuario);
            }
            catch (Exception ex) {
                return BadRequest(new { message = $"No se pudo registrar el usuario. Error: {ex.Message}" });
            }
        }
          
        /// <summary>
        /// Actualize el listado de roles de un usuario según la lista parametrizada.
        /// </summary>
        /// <param name="usuarioroles">Modelo con la información requerida (UserToRole)</param>
        /// <returns>Respuesta de la operación esperada (OK)</returns>        
        [HttpPut]
        [Authorize]
        [Route("actualizar_roles")]
        public async Task<IActionResult> UpdateRolesAsync(UsuarioRoles usuarioroles)
        {

            var _user = await srvUsuario.EncontrarPorNombre(usuarioroles.Usuario);

            if (_user == null)
            {
                return NotFound();
            }

            var roles = await srvUsuario.ObtenerRoles(_user);

            if (roles.Any())
            {                
                try
                {
                   await srvUsuario.RemoverDeRoles(_user, roles);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = $"No se pudo desvincular el usuario de sus roles actuales. Error: {ex.Message}" });
                }                
            }

            try
            {
                await srvUsuario.AgregarARoles(_user, usuarioroles.Roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"No se puedo agregar el usuario a los role seleccionados. Error: {ex.Message}" });
            }

            return Ok(new { message = $"{usuarioroles.Usuario} agregado a los roles seleccionados exitosamente!" });
        }              
    }    
}
