using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.Resources;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models.Home;

namespace JavnaRasprava.WEB.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index( string pCode )
		{
			string requestedSubDomain = null;
			var fullAddress = HttpContext.Request.Headers["Host"].Split( '.' );

			if ( fullAddress.Length > 2 )
			{
				requestedSubDomain = fullAddress[0];
			}

			new ParliamentService().GetTenantData( requestedSubDomain, out string tenantSubDomain, out string targetpCode );

			if ( targetpCode != pCode )
				return RedirectToAction( "Index", new { pCode = targetpCode } );

			return View();
		}

		public ActionResult About()
		{
			if ( (string)this.HttpContext.Items["TenantCode"] == FeatureToggle.Tenant_TK )
				return View( "About.tk" );

			if ( FeatureToggle.IsAlbania() )
				return View( "About.sq" );

			return View();
		}

		public ActionResult Manual()
		{
			if ( (string)this.HttpContext.Items["TenantCode"] == FeatureToggle.Tenant_TK )
				return View( "Manual.tk" );

			if ( FeatureToggle.IsAlbania() )
				return View( "Manual.sq" );

			return View();
		}
		public ActionResult RepManual()
		{
			if ( FeatureToggle.IsAlbania() )
				return View( "RepManual.sq" );

			return View( "Manual" );
		}


		public ActionResult Error()
		{
			throw new ApplicationException( "There has been an error" );
		}
	}
}