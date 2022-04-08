

namespace webapi.Models
{
    public partial class Permiso
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Controlador { get; set; }

        [Required]
        public string Accion { get; set; }

        [Required]
        public string Tipo { get; set; }

        [NotMapped]
        public string Regla => $"{Tipo}.{Controlador}.{Accion}";

        [JsonIgnore]
        public virtual ICollection<RolPermiso> PermisoRoles { get; set; }

        public Permiso()
        {
            PermisoRoles = new HashSet<RolPermiso>();
        }
    }
}