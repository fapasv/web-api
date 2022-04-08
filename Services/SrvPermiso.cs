namespace webapi.Services
{
    public interface ISrvPermiso
    {
        public Task<IEnumerable<Permiso>> Permisos();
        public Task CrearPermiso(Permiso permiso);

        public Task<Permiso?> EncontrarPorNombre(string nombrePermiso);
    }

    public class SrvPermiso : ISrvPermiso
    {
        private readonly membresiaContext db;

        public SrvPermiso(membresiaContext contexto)
        {
            db = contexto;
        }

        public async Task CrearPermiso(Permiso permiso)
        {
            db.Permisos.Add(permiso);
            await db.SaveChangesAsync();
        }

        public async Task<Permiso?> EncontrarPorNombre(string nombrePermiso)
        {
            String[] starr = nombrePermiso.Split('.');

            return await db.Permisos.FirstOrDefaultAsync(p => p.Controlador==starr[1] && p.Accion == starr[2]);
        }

        public async Task<IEnumerable<Permiso>> Permisos()
        {
            return await db.Permisos.OrderBy(p => p.Controlador).ThenBy(p => p.Accion).ToListAsync();
        }
    }}
