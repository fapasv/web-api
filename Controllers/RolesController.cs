
namespace webapi.Controllers
{
    [Authorize]
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ISrvRol srvRol;

        public RolesController(ISrvRol sr)
        {
            srvRol = sr;
        }

        /// <summary>
        /// Lista de roles
        /// </summary>
        /// <returns>Lista de roles</returns>
        [HttpGet]        
        [ProducesResponseType(typeof(List<Rol>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Rol>> GetAsync() => await srvRol.Roles();


        /// <summary>
        /// Agregar nuevo rol
        /// </summary>
        /// <param name="role">nombre del rol</param>
        /// <returns>rol creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Rol), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync([FromBody] string role)
        {
            if (await srvRol.ExisteRol(role))
                return BadRequest(new { message = $"El rol {role} ya existe" });

            try
            {
                await srvRol.CrearRol(role);
                return Ok(new { message = $"Rol {role} agregado exitosamente!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"No se pudo agegar el rol especificado. Error: {ex.Message}" });
            }            
        }

        /// <summary>
        /// Borrar Rol
        /// </summary>
        /// <param name="role">nombre del Rol</param>
        /// <returns>Rol eliminado</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<IActionResult> DeleteAsync([FromBody] string role)
        {
            
            //si no existe 
            if (!await srvRol.ExisteRol(role))
            {
                return NotFound(new { message = "No se encontró el rol especificado." });
            }            
            try
            {
                var usrs = await srvRol.UsuariosEnRol(role); // await db.UsuarioRoles.Where(ur => ur.IdRol == rol.Id).ToListAsync();
                if (usrs.Any())
                {            
                    foreach (var usr in usrs) {
                        await srvRol.EliminarDeRol(usr, role); //db.UsuarioRoles.RemoveRange(usrs);
                    }                    
                }

                var rol = await srvRol.EncontrarPorNombre(role);
                if (rol == null) return NotFound();
                
                await srvRol.Eliminar(rol);
                                
                return Ok(new { message = $"El rol {role} se eliminó satisfactoriamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"El rol {role} no se pudo eliminar. Mensaje: {ex.Message}" });
            }

        }

        /// <summary>
        /// Obtiene todos los usuarios por rol especificado
        /// </summary>
        /// <param name="role">nombre del rl</param>
        /// <returns>lista de usuarios por rol</returns>
        [HttpGet]
        [Route("rol_usuarios")]
        [ProducesResponseType(typeof(List<Usuario>),StatusCodes.Status200OK)]
        public async Task<IActionResult> UsersInRoleAsync(string role)
        {
            return Ok(await srvRol.UsuariosEnRol(role));

        }

        /// <summary>
        /// Obtiene todos los permisos por usuario especificado
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("rol_permisos")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PermissionInRoleAsync(string role)
        {
            var rol = await srvRol.EncontrarPorNombre(role);
            if (rol == null)
            {
                return NotFound(new { message = $"Rol {role} no encontrado" });
            }

            var claims = await srvRol.ObtenerPermisos(rol);
            return Ok(claims.Where(c => c.Tipo == "rol_permiso").Select(c => c.Regla));

        }
    }
}
