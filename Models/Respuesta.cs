namespace webapi.Models
{
    public class Respuesta
    {
        public Respuesta()
        {

        }

        public Respuesta(int idEjercicio, int idUsuario, string valor)
        {
            IdEjercicio = idEjercicio;
            IdUsuario = idUsuario;
            Valor = valor;
        }

        public int IdEjercicio { get; set; }
        public int IdUsuario { get; set; }
        public string Valor { get; set; }

       

        [JsonIgnore]
        public virtual Ejercicio Ejercicio { get; set; }        

       

        
    }
}
