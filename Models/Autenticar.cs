namespace webapi.Models
{
    public class Autenticar
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
