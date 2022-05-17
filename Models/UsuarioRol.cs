
namespace webapi.Models
{
    public class UsuarioRol
    {
        [ForeignKey(nameof(Usuario))]
        public int IdUsuario{get;set;}

        [ForeignKey(nameof(Rol))]
        public int IdRol{get;set;}

        [JsonIgnore]
        public virtual Usuario UsuarioAsociado { get; set; }
        
        [JsonIgnore]
        public virtual Rol RolAsociado { get; set; }
    }
}