using RockPaperScissors.Web.Infrastrucure;
using RockPaperScissors.Web.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RockPaperScissors.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(Game), new GameModelBinder());
        }
    }
}
