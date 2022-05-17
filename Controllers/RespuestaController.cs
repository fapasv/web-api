
namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly libreriaContext db;

        public RespuestaController(libreriaContext contexto)
        {
            db = contexto;
        }

        /// <summary>
        /// Obtiene todos las respuestas de un ejercicio
        /// </summary>
        /// <param name="idEjercicio">Identificador del ejercicio</param>
        /// <returns>Listado de Respuestas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Respuesta>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int idEjercicio)
        {
            return Ok(await db.Respuestas.Where(r => r.IdEjercicio == idEjercicio).ToListAsync());
        }

        /// <summary>
        /// Obtiene listado de Respuestas por página
        /// </summary>
        /// <param name="idEjercicio">Identificador del ejercicio</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Items a mostrar por página</param>
        /// <returns>Listado de Ejercicios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Respuesta>), StatusCodes.Status200OK)]
        [Route("respuesta_por_pagina")]
        public async Task<IEnumerable<Respuesta>> GetAsyncPorPagina(int idEjercicio, int pageNumber, int pageSize)
        {
            return await db.Respuestas.Where(r => r.IdEjercicio == idEjercicio)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }

        /// <summary>
        /// Obtiene todos las respuestas de un ejercicio
        /// </summary>
        /// <param name="idEjercicio">Identificador del ejercicio</param>
        /// <param name="idUsuario">Identificador del usuario</param>
        /// <returns>Listado de Respuestas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Respuesta>), StatusCodes.Status200OK)]
        [Route("ejercicio_usuario")]
        public async Task<IActionResult> GetByUserAsync(int idEjercicio, int idUsuario)
        {
            return Ok(await db.Respuestas.Where(r => r.IdEjercicio == idEjercicio && r.IdUsuario == idUsuario).ToListAsync());
        }


        /// <summary>
        /// Obtiene una Respuesta que corresponda con el Identificador.
        /// </summary>
        /// <param name="id">Identificador de Respuesta</param>
        /// <returns>Respuesta</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByID(int id)
        {
            return await db.Respuestas.FindAsync(id)
                 is Respuesta respuestas
                     ? Ok(respuestas)
                     : NotFound();

        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Respuesta>), StatusCodes.Status200OK)]
        [Route("buscar/{query}")]
        public async Task<IActionResult> GetAsyncBuscar(string query)
        {
            var _encontrados = await db.Respuestas.Where(ej => ej.Valor.ToLower().Contains(query.ToLower())).ToListAsync();

            return _encontrados.Count > 0
                ? Ok(_encontrados)
                : NotFound(Array.Empty<Respuesta>());
        }

        /// <summary>
        /// Agrega un nuevo Ejercicio
        /// </summary>
        /// <param name="ejercicio">Objecto de tipo Ejercicio</param>
        /// <returns>ejercicio creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(Ejercicio ejercicio)
        {
            db.Ejercicios.Add(ejercicio);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);
            return Created($"/libros/{ejercicio.Id}", ejercicio);
        }

        /// <summary>
        /// Actualiza una Respuesta en base a su Identificador
        /// </summary>
        /// <param name="id">Identificador de Respuesta</param>
        /// <param name="respuesta">Información de Respuesta</param>
        /// <returns>Respuesta modificada</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync([FromQuery] int id, [FromBody] Respuesta respuesta)
        {
            var encontrado = await db.Respuestas.FindAsync(id);

            if (encontrado is null) return NotFound();

            db.Entry(encontrado).CurrentValues.SetValues(respuesta);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);

            return Ok(encontrado);
        }

        /// <summary>
        /// Borra una Respuesta dependiendo de su Identificador
        /// </summary>
        /// <param name="id">Identificador de Respuesta</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var r = await db.Respuestas.FindAsync(id);

            if (r is null) return NotFound();

            db.Respuestas.Remove(r);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);
            return Ok(r);
        }
    }
}
