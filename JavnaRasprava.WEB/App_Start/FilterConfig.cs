using JavnaRasprava.WEB.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB
{
    public class FilterConfig
    {
		public static void RegisterGlobalFilters( GlobalFilterCollection filters )
		{
			filters.Add( new CustomHandleErrorAttribute() );
		}
	}
}
