namespace webapi.Models
{
    public class PermisoBase
    {
        [Required]
        public string Accion { get; set; }

        [Required]
        public string Controlador { get; set; }
    }
}
