
namespace webapi.Services.Autorizacion
{
      

    internal class ProveedorPoliticasPermiso : IAuthorizationPolicyProvider
    {

        public DefaultAuthorizationPolicyProvider ProveedorAlternativo { get; }

        public ProveedorPoliticasPermiso(IOptions<AuthorizationOptions> options)
        {
            ProveedorAlternativo = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => ProveedorAlternativo.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => ProveedorAlternativo.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.Contains("RolPermiso", StringComparison.OrdinalIgnoreCase) || 
                policyName.Contains("UsuarioPermiso", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new RequerimientoPermiso(policyName));
                return Task.FromResult(policy.Build());
            }
            //return Task.FromResult<AuthorizationPolicy>(null);
            return ProveedorAlternativo.GetPolicyAsync(policyName);
        }
        
    }
}
