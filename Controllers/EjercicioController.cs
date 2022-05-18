
namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EjercicioController : ControllerBase
    {
        private readonly libreriaContext db;
        private readonly ISrvUsuario srvUsuario;

        public EjercicioController(libreriaContext contexto)
        {
            db = contexto;
        }

        /// <summary>
        /// Obtiene todos los ejercicios de un Libro
        /// </summary>
        /// <param name="idLibro">Identificador del libro</param>
        /// <returns>Listado de Ejercicios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Ejercicio>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int idLibro)
        {
            return Ok(await db.Ejercicios.Where(ej => ej.IdLibro == idLibro).ToListAsync());
        }




        /// <summary>
        /// Obtiene un Ejercicio que corresponda con el Identificador.
        /// </summary>
        /// <param name="id">Identificador de Ejercicio</param>
        /// <returns>Ejercicio</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByID(int id)
        {
            return await db.Ejercicios.Include(ej => ej.Respuestas).FirstOrDefaultAsync(ej => ej.Id == id)
                 is Ejercicio ejercicio
                     ? Ok(ejercicio)
                     : NotFound();

        }

        [HttpGet]
        [Route("ejercicios_usuario")]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByUserID(int id, int userId)
        {
            var ej = await db.Ejercicios.FindAsync(id);
            if (ej is null) { 
                return NotFound();
            }
            ej.Respuestas = await db.Respuestas.Where(r => r.IdUsuario == userId && r.IdEjercicio == id).ToListAsync();
            return Ok(ej);

        }



        /// <summary>
        /// Obtiene listado de Ejercicios por página
        /// </summary>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Items a mostrar por página</param>
        /// <returns>Listado de Ejercicios</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Ejercicio>), StatusCodes.Status200OK)]
        [Route("ejercicios_por_pagina")]
        public async Task<IEnumerable<Ejercicio>> GetAsyncPorPagina( int pageNumber, int pageSize)
        {
            return await db.Ejercicios
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<Ejercicio>), StatusCodes.Status200OK)]
        [Route("buscar/{query}")]
        public async Task<IActionResult> GetAsyncBuscar(string query)
        {
            var _encontrados = await db.Ejercicios.Where(ej => ej.Enunciado.ToLower().Contains(query.ToLower())).ToListAsync();

            return _encontrados.Count > 0
                ? Ok(_encontrados)
                : NotFound(Array.Empty<Ejercicio>());
        }

        /// <summary>
        /// Agrega un nuevo Ejercicio
        /// </summary>
        /// <param name="ejercicio">Objecto de tipo Ejercicio</param>
        /// <returns>ejercicio creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(EjercicioVm ejercicio)
        {
            var ej = new Ejercicio
            {
                IdLibro = ejercicio.IdLibro,
                Enunciado = ejercicio.Enunciado
            };
            db.Add(ej);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);
            return Created($"/ejercicios/{ej.Id}", ej);
        }

        /// <summary>
        /// Actualiza un Ejercicio en base a su Identificador
        /// </summary>
        /// <param name="id">Identificador de Ejercicio</param>
        /// <param name="ejercicio">Información del Ejercicio</param>
        /// <returns>Ejercicio modificado</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync([FromQuery] int id, [FromBody] Ejercicio ejercicio)
        {
            var encontrado = await db.Libros.FindAsync(id);

            if (encontrado is null) return NotFound();

            db.Entry(encontrado).CurrentValues.SetValues(ejercicio);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);

            return Ok(encontrado);
        }

        /// <summary>
        /// Borra un Ejercicio dependiendo de su Identificador
        /// </summary>
        /// <param name="id">Identificador de ejercicio</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Ejercicio), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id) {
            var ejercicio = await db.Ejercicios.FindAsync(id);

            if (ejercicio is null) return NotFound();

            db.Ejercicios.Remove(ejercicio);
            await db.SaveChangesAsync(User?.FindFirst(ClaimTypes.Name)?.Value);
            return Ok(ejercicio);
        }
    }
}
