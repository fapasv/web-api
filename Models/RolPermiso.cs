
namespace webapi.Models
{
    public partial class RolPermiso
    {
        public int IdRol{get;set;}
        public int IdPermiso{get;set;}

        [JsonIgnore]
        public virtual Permiso PermisoAsociado { get; set; }
        
        [JsonIgnore]
        public virtual Rol RolAsociado { get; set; }
    }
}