namespace webapi.Models
{
    public class UsuarioRoles
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Nombre de Usuario")]
        public string Usuario { get; set; } = "";

        [Required(ErrorMessage = "{0} es requerido")]
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        
    }
    }
