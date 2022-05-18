namespace webapi.Models
{
    public class Ejercicio
    {
        public Ejercicio()
        {
            Respuestas = new HashSet<Respuesta>();
        }
        
      

        public int Id { get; set; }
        public string Enunciado { get; set; } = null!;
        public int IdLibro { get; set; }        
       
        

        public virtual Libro LibroAsociado { get; set; } = null!;

        public virtual ICollection<Respuesta> Respuestas { get; set; }
    }
}
