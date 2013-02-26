using ES.Lab.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ES.Lab.Api.Infrastructure.Security;

namespace ES.Lab.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //Web Api
            Start(GlobalConfiguration.Configuration);
            //MVC
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        public static void Start(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = Bootstrapper.Start();
            configuration.MessageHandlers
                .Add(
                    configuration.DependencyResolver.GetService(typeof (BasicAuthenticationMessageHandler))
                    as BasicAuthenticationMessageHandler);
            WebApiConfig.Register(configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}