using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class FeatureToggle
	{
		public const string Tenant_TK = "TK";
		public const string Tenant_BA = "BA";
		public const string Tenant_ALB = "ALB";

		public static bool IsAlbania()
		{
			return ConfigurationManager.AppSettings["FeatureToggle.Instance"] == "ALB";
		}

		public static bool IsBosnia()
		{
			return ConfigurationManager.AppSettings["FeatureToggle.Instance"] == "BA";
		}

		internal static object GetDefaultPCode()
		{
			return AppConfig.GetStringValue( "FeatureToggle.DefaultPCode" );
		}

		public static string GetDefaultCulture()
		{
			if ( IsBosnia() ) return "bs";
			if ( IsAlbania() ) return "sq";
			return "bs";
		}
	}
}