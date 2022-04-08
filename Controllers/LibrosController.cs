
namespace webapi.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly libreriaContext db;

        public LibrosController(libreriaContext contexto)
        {
            db = contexto;
        }

        /// <summary>
        /// Obtiene todos los Libros en la biblioteca
        /// </summary>
        /// <returns>Listado de Libros</returns>
        [HttpGet]
        [Authorize(Policy = "RolPermiso.Libros.Get")]
       
        [ProducesResponseType(typeof(List<Libro>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await db.Libros.Include(a => a.Autor).ToListAsync());
        }

        /// <summary>
        /// Obtiene un Libro que corresponda con el Identificador.
        /// </summary>
        /// <param name="id">Identificador de Libro</param>
        /// <returns>Libro</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Libro), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsyncByID(int id)
        {
            return await db.Libros.FindAsync(id)
                 is Libro libro
                     ? Ok(libro)
                     : NotFound();

        }

        /// <summary>
        /// Obtiene listado de libros por página
        /// </summary>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">libros a mostrar por página</param>
        /// <returns>Listado de libros</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Libro>), StatusCodes.Status200OK)]
        [Route("libros_por_pagina")]
        public async Task<IEnumerable<Libro>> GetAsyncPorPagina(int pageNumber, int pageSize) {
            return await db.Libros
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Libro>), StatusCodes.Status200OK)]
        [Route("buscar/{query}")]
        public async Task<IActionResult> GetAsyncBuscar(string query)
        {
            var _encontrados = await db.Libros.Where(x => x.Titulo.ToLower().Contains(query.ToLower())).ToListAsync();

            return _encontrados.Count > 0
                ? Ok(_encontrados)
                : NotFound(Array.Empty<Libro>());
        }

        /// <summary>
        /// Agrega un nuevo Libro
        /// </summary>
        /// <param name="libro">Objecto de tipo Libro</param>
        /// <returns>Libro creado</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Libro), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync(Libro libro)
        {
            db.Libros.Add(libro);
            await db.SaveChangesAsync();
            return Created($"/libros/{libro.Id}", libro);
        }

        /// <summary>
        /// Actualiza un Libro en base a su Identificador
        /// </summary>
        /// <param name="id">Identificador de Libro</param>
        /// <param name="libro">Información del Libro</param>
        /// <returns>Autor modificado</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync([FromQuery] int id, [FromBody] Libro libro)
        {
            var encontrado = await db.Libros.FindAsync(id);

            if (encontrado is null) return NotFound();
            encontrado.Titulo = libro.Titulo;

            await db.SaveChangesAsync();
            return Ok(encontrado);
        }
    }    
}
