using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Controllers
{
    public class IntegrationsController : Controller
    {
		// GET: Integrations
		[Route( "Integrations/SendEmailsToRepresentatives", Name = "def.Integrations.SendEmailsToRepresentatives" )]
		public ActionResult SendEmailsToRepresentatives( string token )
        {
            if ( AppConfig.GetStringValue( "IntegrationToken" ) != token )
                return new HttpUnauthorizedResult();

            new NotificationService().StartWeeklyReportAsync();

            return Json( new { isSuccess = true }, JsonRequestBehavior.AllowGet );

        }

		[Route( "Integrations/Ping", Name = "def.Integrations.Ping" )]
		public ActionResult Ping( string token, string message )
        {
            if ( AppConfig.GetStringValue( "IntegrationToken" ) != token )
                return new HttpUnauthorizedResult();

            return Json( new { Message = message }, JsonRequestBehavior.AllowGet );
        }
    }
}