using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JavnaRasprava.WEB.Infrastructure.Helpers;
using JavnaRasprava.WEB.Infrastructure;
using System.Threading;
using JavnaRasprava.WEB.Helpers;
using JavnaRasprava.WEB.BLL;

namespace JavnaRasprava.WEB.Controllers
{
	public abstract class BaseController : Controller
	{
		protected override void OnActionExecuting( ActionExecutingContext filterContext )
		{
			// Just to add something to session.
			SessionManager.Current.ToString();

			var pCode = (string)this.RouteData.Values["pcode"];
			new ParliamentService().GetTenantData( ref pCode, out string tenantCode, out int parliamentId, out string tenantSubdomain );

			this.HttpContext.Items["ParliamentId"] = parliamentId;
			this.HttpContext.Items["ParliamentCode"] = pCode;
			this.HttpContext.Items["TenantCode"] = tenantCode;
			this.HttpContext.Items["TenantSubdomain"] = tenantSubdomain;

			ViewBag.ParliamentId = parliamentId;
			ViewBag.ParliamentCode = pCode;
			ViewBag.TenantCode = tenantCode;
			ViewBag.TenantSubdomain = tenantSubdomain;

			// For views
			ViewBag.CssName = $"/Content/Site.{tenantCode}.css";

			base.OnActionExecuting( filterContext );
		}

		protected override IAsyncResult BeginExecuteCore( AsyncCallback callback, object state )
		{
			string cultureName = null;

			// Attempt to read the culture cookie from Request
			HttpCookie cultureCookie = Request.Cookies["_culture"];
			if ( cultureCookie != null )
				cultureName = cultureCookie.Value;
			else
				cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
						Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
						null;
			// Validate culture name
			cultureName = CultureHelper.GetImplementedCulture( cultureName ); // This is safe

			// Modify current thread's cultures            
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo( cultureName );
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			return base.BeginExecuteCore( callback, state );
		}

	}
}