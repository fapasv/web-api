
namespace webapi.Models
{
    public partial class Libro
    {
        public Libro()
        {
          
            Ejercicios = new HashSet<Ejercicio>();
        }
        public Libro(string titulo, string usuarioMod) { 
            Titulo = titulo;
            
            Ejercicios = new HashSet<Ejercicio>();
        }
        

        public int Id { get; set; }
        public string Titulo { get; set; }   

        [JsonIgnore]
        public virtual ICollection<Ejercicio> Ejercicios { get; set; }

        


    }

}