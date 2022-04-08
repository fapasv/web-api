
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly SrvRutas srvRutas;
        private readonly ISrvRol srvRol;
        private readonly ISrvPermiso srvPermiso;

        public PermisosController(IActionDescriptorCollectionProvider proveedor, ISrvRol srvr,  ISrvPermiso srvp)
        {
            srvRutas = new SrvRutas(proveedor);
            srvRol = srvr;
            srvPermiso = srvp;            
        }

        

        // GET: api/<PermisosController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Permiso>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Permiso>> GetAsync()
        {            
            return await srvPermiso.Permisos(); ;
        }

        [HttpGet]
        [Route("controladores")]
        [ProducesResponseType(typeof(List<string?>), StatusCodes.Status200OK)]
        public IActionResult GetControllers()
        {
            var controllersList = srvRutas.Controladores().ToList();
            return Ok(controllersList);
        }

        [HttpGet]
        [Route("acciones")]
        [ProducesResponseType(typeof(List<string?>), StatusCodes.Status200OK)]
        public IActionResult GetAcciones(string controlador)
        {
            var controllersList = srvRutas.Acciones(controlador).ToList();
            return Ok(controllersList);
        }

        /// <summary>
        /// Agrega un permiso para entidades rol
        /// </summary>
        /// <param name="permiso"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Permiso), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([Bind("Controller, Rule")] PermisoBase permiso)
        {
            if (ModelState.IsValid)
            {
                var np = new Permiso()
                {
                    Controlador = permiso.Controlador,
                    Accion = permiso.Accion,
                    Tipo = "RolPermiso"
                };

                if (!srvRutas.ExisteRuta(np.Controlador, np.Accion)) {
                    return NotFound(new { message = $"La ruta especificada no exite ({np.Controlador}/{np.Accion})" });
                }

                try
                {
                    await srvPermiso.CrearPermiso(np);

                    return Ok(new { message = $"El permiso de acceso {np.Regla} se agregó correctamente." });
                }
                catch (Exception ex) {
                    return BadRequest(new { message = $"No se pudo agegar el permiso especificado. Error: {ex.Message}" });
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Agrega un listado de roles a un permiso especificado
        /// </summary>
        /// <param name="permisoRoles"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("permiso_roles")]
        public async Task<IActionResult> RolesInPermissionAsync([Bind("permiso, roles")] PermisoAddRoles permisoRoles)
        {
            if (ModelState.IsValid)
            {
                var conteo = 0;
                var permiso = await srvPermiso.EncontrarPorNombre(permisoRoles.permiso);
                if (permiso == null) { return NotFound(new { message = "Permiso no encontrado"}); }

                foreach (var rol in permisoRoles.roles)
                {
                    var r = await srvRol.EncontrarPorNombre(rol);
                    if (r != null)
                    {
                        conteo++;
                        await srvRol.AgregarPermiso(r, permiso);
                    }
                }
                return Ok(new { message = $"La regla de acceso {permisoRoles.permiso} se agregó correctamente a {conteo} roles de {permisoRoles.roles.Count()}." });
            }
            return BadRequest();
        }
    }
}
