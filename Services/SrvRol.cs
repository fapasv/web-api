namespace webapi.Services
{
    public interface ISrvRol {
        public Task<bool> ExisteRol(string nombreRol);

        public Task<IEnumerable<Rol>> Roles();

        public Task CrearRol(string nombreRol);

        public Task<Rol?> EncontrarPorNombre(string nombreRol);

        public Task EliminarDeRol(Usuario usuario, string nombrerol);

        public Task Eliminar(Rol rol);

        public Task AgregarPermiso(Rol rol, Permiso permiso);

        public Task<IEnumerable<Permiso>> ObtenerPermisos(Rol rol);

        public Task<IEnumerable<Usuario>> UsuariosEnRol(string nombreRol);
    }
    public class SrvRol : ISrvRol
    {
        private readonly membresiaContext db;

        public SrvRol(membresiaContext contexto)
        {
            db = contexto;
        }

        public async Task AgregarPermiso(Rol rol, Permiso permiso)
        {
            db.RolPermisos.Add(new RolPermiso { IdPermiso = permiso.Id, IdRol = rol.Id });
            await db.SaveChangesAsync();
        }

        public async Task CrearRol(string nombreRol)
        {
            db.Roles.Add(new Rol() { Nombre = nombreRol });
            await db.SaveChangesAsync();            
        }

        public async Task Eliminar(Rol rol)
        {
            db.Roles.Remove(rol);
            await db.SaveChangesAsync();
        }

        public async Task EliminarDeRol(Usuario usuario, string nombreRol)
        {
            var usrs = await db.UsuarioRoles.Where(ur => 
                                                    ur.RolAsociado.Nombre == nombreRol
                                                    && ur.IdUsuario == usuario.Id).ToListAsync();
            if (usrs.Any())
            {                
                db.UsuarioRoles.RemoveRange(usrs);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Rol?> EncontrarPorNombre(string nombreRol) => await db.Roles.FirstOrDefaultAsync(r => r.Nombre == nombreRol);

        public async Task<bool> ExisteRol(string nombreRol) => await db.Roles.AnyAsync(r => r.Nombre == nombreRol);

        public async Task<IEnumerable<Permiso>> ObtenerPermisos(Rol rol) => 
            await db.RolPermisos.Where(rp => rp.IdRol == rol.Id).Select(rp => rp.PermisoAsociado).ToListAsync();

        public async Task<IEnumerable<Rol>> Roles() => await db.Roles.ToListAsync();

        public async Task<IEnumerable<Usuario>> UsuariosEnRol(string nombreRol) => 
            await db.UsuarioRoles.Where(ur => ur.RolAsociado.Nombre == nombreRol).Select(ur => ur.UsuarioAsociado).ToListAsync();
    }
}
