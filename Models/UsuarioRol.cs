
namespace webapi.Models
{
    public class UsuarioRol
    {
        public int IdUsuario{get;set;}
        public int IdRol{get;set;}

        [JsonIgnore]
        public virtual Usuario UsuarioAsociado { get; set; }
        
        [JsonIgnore]
        public virtual Rol RolAsociado { get; set; }
    }
}