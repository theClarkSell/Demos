using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace PhotoGallery
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            
            //Add route for extension support
            //routes.MapRoute(
            //  "LegacyUrl",
            //  "{controller}/{action}.cshtml/{id}",
            //  new { controller = "Gallery", action = "Default", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Gallery", action = "Default", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
            WebSecurity.InitializeDatabaseConnection("PhotoGallery", "UserProfiles", "UserId", "Email", true);
        }
    }
}