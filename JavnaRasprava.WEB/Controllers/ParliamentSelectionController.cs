using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Controllers
{
	public class ParliamentSelectionController : BaseController
	{
		public ActionResult JavnaRasprava( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.JavnaRasprava_Index, "../Shared/_skupstine" );
		}

		public ActionResult Representatives( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.Representatives_Index, "../Shared/_skupstineRepresentatives" );
		}

		public ActionResult AllLaws( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.AllLaws_Index, "../Shared/_skupstine" );
		}

		public ActionResult NewsIndex( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.News_Index, "../Shared/_skupstine" );
		}

		public ActionResult QuizIndex( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.Quiz_Index, "../Shared/_skupstine" );
		}

		public ActionResult AllSections( string pCode )
		{
			return GetPartialView( pCode, Resources.Routes.AllSections_Index, "../Shared/_skupstine" );
		}

		public ActionResult Admin( string pCode, string returnUrl )
		{
			return GetPartialView( pCode, returnUrl, "../Shared/_skupstineAdmin", true );
		}

		private ActionResult GetPartialView( string pCode, string returnUrl, string partialViewName, bool returnAll = false )
		{
			var parliament = GetCurrentParliament( pCode );

			var parliaments = new ParliamentService().GetAllParliamentsCached().Where( x => x.Tenant == parliament.Tenant ).ToList();


			if ( !returnAll && parliaments.Count() == 1 )
				return new EmptyResult();

			string baseTenantAddress = AppConfig.GetStringValue( "BaseTenantAddress" );
			ViewBag.BaseTenantAddress = baseTenantAddress;
			ViewBag.ReturnUrl = returnUrl;

			var parliamentsToPass = returnAll ? new ParliamentService().GetAllParliamentsCached() : parliaments;

			if ( parliamentsToPass.Count() == 1 )
				return new EmptyResult();

			int colNumber = 1;
			switch ( parliamentsToPass.Count )
			{
				case 2:
				case 3:
				case 4:
				case 6:
					colNumber = ( 12 / parliamentsToPass.Count );
					break;
				case 5:
					colNumber = 2;
					break;
			}
			ViewBag.DivClass = $"col-sm-{colNumber}";

			return PartialView( partialViewName, parliamentsToPass );
		}

		private Parliament GetCurrentParliament( string code )
		{
			//return new ParliamentService().GetAllParliamentsCached().Single(x => x.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));


			var parlieamentId = (int)this.HttpContext.Items["ParliamentId"];
			return new ParliamentService().GetAllParliamentsCached().Single( x => x.ParliamentID == parlieamentId );
			//return parlieamentId;
		}
	}
}