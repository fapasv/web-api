
namespace webapi.Models
{
    public partial class Libro
    {        
        public int Id { get; set; }
        public string Titulo { get; set; } 
        public int AutorId { get; set; }
        public decimal Precio{get;set;}
        public DateTime? FechaPublicacion{get;set;}

        [JsonIgnore]
        public virtual Autor Autor { get; set; }
    }

}