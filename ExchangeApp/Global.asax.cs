using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ExchangeApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private ExchangeRateDeamon deamon;
        public WebApiApplication()
        {
            deamon = new ExchangeRateDeamon();
        }        
        protected void Application_Start()
        {
            deamon.Start();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void Dispose()
        {
            deamon.Dispose();
            base.Dispose();
        }
    }
}
