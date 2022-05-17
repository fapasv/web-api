
namespace webapi.Models
{
    public partial class RolPermiso
    {
        [ForeignKey(nameof(Rol))]
        public int IdRol{get;set;}

        [ForeignKey(nameof(Permiso))]
        public int IdPermiso{get;set;}

        [JsonIgnore]
        public virtual Permiso PermisoAsociado { get; set; }
        
        [JsonIgnore]
        public virtual Rol RolAsociado { get; set; }
    }
}