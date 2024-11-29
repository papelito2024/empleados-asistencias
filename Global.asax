using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        // Configurar CORS en Global.asax para toda la aplicación
        EnableCorsForAllOrigins();

        // Configuración de rutas MVC
        AreaRegistration.RegisterAllAreas();
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

    // Método para habilitar CORS para todos los orígenes
    private void EnableCorsForAllOrigins()
    {
        // Aquí estamos permitiendo todos los orígenes, puedes restringirlo si lo necesitas
        var cors = new EnableCorsAttribute("*", "*", "*");  // Permitir todos los orígenes, encabezados y métodos
        GlobalConfiguration.Configuration.EnableCors(cors);
    }
}
