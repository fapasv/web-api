namespace webapi.Services.Autorizacion
{
    
        internal class PermisoAutorizacionHandler : AuthorizationHandler<RequerimientoPermiso>
        {
            public PermisoAutorizacionHandler() { }
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext contexto, RequerimientoPermiso requerimiento)
            {
                if (contexto.User == null) { return Task.CompletedTask; }

                var permisos = contexto.User.Claims.Where(x => (x.Type == "UsuarioPermiso" || x.Type == "RolPermiso") &&
                                                                    x.Value == requerimiento.Permiso &&
                                                                    x.Issuer == "capacitacion.fapa.net");

                if (permisos.Any()) { contexto.Succeed(requerimiento); }

                return Task.CompletedTask;
            }
        }
    }
