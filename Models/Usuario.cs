
using System.ComponentModel;

namespace webapi.Models
{
    public partial class Usuario
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]        
        public string Password { get; set; }


        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiracion { get; set; }

        [JsonIgnore]
        public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; }

        public Usuario()
        {
            UsuarioRoles = new HashSet<UsuarioRol>();
        }
    }
}
