
namespace webapi.Models
{
    public partial class Libro
    {
        public Libro()
        {
          
            Ejercicios = new HashSet<Ejercicio>();
        }
        
        

        public int Id { get; set; }
        public string Titulo { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Ejercicio> Ejercicios { get; set; }

        


    }

}