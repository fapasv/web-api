
namespace webapi.Models
{
    public partial class Rol
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioRol> RolUsuarios { get; set; }

        [JsonIgnore]
        public virtual ICollection<RolPermiso> RolPermisos { get; set; }

        public Rol()
        {
            RolUsuarios = new HashSet<UsuarioRol>();
            RolPermisos = new HashSet<RolPermiso>();
        }
    }
}