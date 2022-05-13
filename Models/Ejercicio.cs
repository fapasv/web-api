namespace webapi.Models
{
    public class Ejercicio
    {
        public Ejercicio()
        {
            Respuestas = new HashSet<Respuesta>();
        }
        public Ejercicio(int idLibro, string enunciado)
        {
            LibroId = idLibro;
            Enunciado = enunciado;           
            Respuestas = new HashSet<Respuesta>();
        }
      

        public int Id { get; set; }
        public string Enunciado { get; set; }

        public int IdLibro { get; set; }        
       
        
        [JsonIgnore]
        public virtual Libro Libro { get; set; }

        [JsonIgnore]
        public virtual ICollection<Respuesta> Respuestas { get; set; }
    }
}
