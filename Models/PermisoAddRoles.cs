namespace webapi.Models
{
    public class PermisoAddRoles
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Nombre de permiso")]
        public string permiso { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public IEnumerable<string> roles { get; set; }
    }
}
