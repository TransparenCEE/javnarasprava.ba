using EEP.Utility;
using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JavnaRasprava.WEB
{
	public class RouteConfig
	{
		public static void RegisterRoutes( RouteCollection routes )
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );
			routes.MapMvcAttributeRoutes();
			var defaultpCode = FeatureToggle.GetDefaultPCode();
			routes.MapRoute(
				name: "Parliament",
				url: "{pCode}/{controller}/{action}/{id}",
				defaults: new { pCode = "bih", controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
