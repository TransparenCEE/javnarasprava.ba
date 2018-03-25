using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class TenantRouteConstraint : IRouteConstraint
	{
		public bool Match( HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection )
		{
			Tenant tenant = Tenant.www;
			var fullAddress = httpContext.Request.Headers[ "Host" ].Split( '.' );
			if ( fullAddress.Length > 2 )
			{
				var tenantSubdomain = fullAddress[ 0 ];

				if ( !Enum.TryParse( tenantSubdomain, out tenant ) )
					tenant = Tenant.www;
			}

			if ( !values.ContainsKey( "tenant" ) )
			{
				values.Add( "tenant", tenant );
			}

			return true;
		}
	}
}