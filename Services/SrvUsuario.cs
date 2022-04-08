namespace webapi.Services
{
    public interface ISrvUsuario
    {
        public Task<IEnumerable<Usuario>> Usuarios();

        public Task RemoverDeRoles(Usuario usuario, IEnumerable<string> roles);
        public Task AgregarARoles(Usuario usuario, IEnumerable<string> roles);
        public Task<IEnumerable<string>> ObtenerRoles(Usuario usuario);
        public Task<Usuario?> EncontrarPorNombre(string usuarioNombre);
        public Task CrearUsuario(Usuario usuario);

    }
    public class SrvUsuario : ISrvUsuario {
        private readonly membresiaContext db;

        public SrvUsuario(membresiaContext contexto)
        {
            db = contexto;
        }

        public async Task RemoverDeRoles(Usuario usr, IEnumerable<string> roles)
        {
            try
            {
                var aborrar = await db.UsuarioRoles.Where(ur => ur.IdUsuario == usr.Id && roles.Any(dr => dr == ur.RolAsociado.Nombre)).ToListAsync();
                db.UsuarioRoles.RemoveRange(aborrar);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AgregarARoles(Usuario usr, IEnumerable<string> roles)
        {
            try
            {
                var enrolar = await db.Roles.Where(r => roles.Any(nr => nr == r.Nombre)).Select(r => new UsuarioRol { IdRol = r.Id, IdUsuario = usr.Id }).ToListAsync();
                db.UsuarioRoles.AddRange(enrolar);
                await db.SaveChangesAsync();

            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<string>> ObtenerRoles(Usuario usr) 
            => await db.UsuarioRoles.Where(ur => ur.IdUsuario == usr.Id)
                                    .Select(ur => ur.RolAsociado.Nombre).ToListAsync();

        public async Task<Usuario?> EncontrarPorNombre(string usuarioNombre) => 
            await db.Usuarios.FirstOrDefaultAsync(u => u.Nombre == usuarioNombre);

        public async Task<IEnumerable<Usuario>> Usuarios() => await db.Usuarios.ToListAsync();

        public async Task CrearUsuario(Usuario usuario)
        {
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
        }

        
    }
}
