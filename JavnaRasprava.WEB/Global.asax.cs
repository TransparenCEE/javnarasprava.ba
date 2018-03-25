using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JavnaRasprava.WEB
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
			RouteConfig.RegisterRoutes( RouteTable.Routes );
			BundleConfig.RegisterBundles( BundleTable.Bundles );
			DomainModelStartup.Initialize();

			Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
				Infrastructure.AppConfig.GetStringValue( "AI.Key" );

			Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.TelemetryInitializers.Add( new Infrastructure.StatusCodeOverrideTelemetryInitializer() );

		}
	}
}
