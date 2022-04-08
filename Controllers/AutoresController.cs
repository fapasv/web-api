
namespace webapi.Controllers
{
   // [ApiExplorerSettings(GroupName = "Recursos")]
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly libreriaContext db;

        public AutoresController(libreriaContext context)
        {
            db = context;
        }


        /// <summary>
        /// Obtiene todos los autores en la biblioteca
        /// </summary>
        /// <returns>Listado de Autores</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Autor>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await db.Autors.Include(a => a.Libros).ToListAsync());
        }

        /// <summary>
        /// Obtiene un autor que corresponda con el Identificador.
        /// </summary>
        /// <param name="id">Identificador de Autor</param>
        /// <returns>Autor</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByID(int id)
        {
            return await db.Autors.Include(a => a.Libros).FirstOrDefaultAsync(a => a.Id == id)
                 is Autor autor
                     ? Ok(autor)
                     : NotFound();

        }

        /// <summary>
        /// Agrega un nuevo Autor
        /// </summary>
        /// <param name="autor">Objecto de tipo Autor</param>
        /// <returns>Autor creado</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Autor), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(Autor autor)
        {
            db.Autors.Add(autor);
            await db.SaveChangesAsync();
            return Created($"/autores/{autor.Id}", autor);
        }

        /// <summary>
        /// Actualiza un Autor en base a su Identificador
        /// </summary>
        /// <param name="id">Identificador de Autor</param>
        /// <param name="autor">Información del Autor</param>
        /// <returns>Autor modificado</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync([FromQuery] int id, [FromBody] Autor autor)
        {
            if (autor.Id != id)
            {
                return BadRequest();
            }

            var encontrado = await db.Autors.FindAsync(id);
            if (encontrado is null) return NotFound();

            encontrado.Nombre = autor.Nombre;
            encontrado.Alias = autor.Alias;

            await db.SaveChangesAsync();
            return Ok(encontrado);
        }

        /// <summary>
        /// Borra un Autor por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Autor borrado</returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id) {
            if (await db.Autors.FindAsync(id) is Autor borrar)
            {
                db.Autors.Remove(borrar);
                await db.SaveChangesAsync();
                return Ok(borrar);
            }
            return NotFound();
        }
    }
}
