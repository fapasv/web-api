namespace webapi.Services.Autorizacion
{
    internal class RequerimientoPermiso : IAuthorizationRequirement
    {
        public string Permiso { get; private set; }
        public RequerimientoPermiso(string permiso)
        {
            Permiso = permiso;
        }
    }

}