using System.Web.Http;
using System.Web.Http.Cors;

public class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Habilitar CORS globalmente para todas las rutas
        var cors = new EnableCorsAttribute("*", "*", "*");  // Permitir todos los orígenes, métodos y cabeceras
        config.EnableCors(cors);

        // Otras configuraciones de la API
        config.MapHttpAttributeRoutes();
    }
}
