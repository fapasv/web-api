
namespace webapi.Models
{
    public partial class Autor
    {
        public Autor()
        {
            Libros = new HashSet<Libro>();
        }
        
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Alias { get; set; }

        public virtual ICollection<Libro> Libros { get; set; }
    }
}
