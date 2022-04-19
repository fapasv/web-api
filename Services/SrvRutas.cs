
namespace webapi.Services
{
    public interface ISrvRutas
    {
        IEnumerable<string?> Controladores();
        IEnumerable<string?> Acciones(string nombreControlador);

        bool ExisteRuta(string controlador, string accion);
    }

    public class SrvRutas : ISrvRutas
    {
        private readonly IActionDescriptorCollectionProvider _proveedor;

        public SrvRutas(IActionDescriptorCollectionProvider proveedor)
        {
            _proveedor = proveedor;
        }

        public IEnumerable<string?> Acciones(string nombreControlador)
        {
            return _proveedor.ActionDescriptors.Items
                   .Where(a => a.RouteValues["controller"] == nombreControlador && !string.IsNullOrEmpty(a.RouteValues["action"]))
                   .Select(a => a.RouteValues["action"]).Distinct().ToList();
        }

        public IEnumerable<string?> Controladores()
        {
            return _proveedor.ActionDescriptors.Items
           .Where(a => a.RouteValues["controller"] != null)
           .Select(a => a.RouteValues["controller"]).Distinct().ToList();
        }

        public bool ExisteRuta(string controlador, string accion)
        {
            return _proveedor.ActionDescriptors.Items.Any(a => a.RouteValues["controller"] == controlador && a.RouteValues["action"] == accion);
        }
    }
}
