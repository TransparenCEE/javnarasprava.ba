using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;

namespace JavnaRasprava.WEB.Controllers
{
	public class ParliamentController : BaseController
	{
		// GET: Parliament
		public ActionResult Index()
		{
			ParliamentService service = new ParliamentService();
			ParliamentListModel model = service.GetParliaments();
			return View( model );
		}

		public void SelectParliament( int parId, string home )
		{
			SessionManager.Current.CurrentParliamentId = parId;

			Response.Redirect( "~/" + home );
		}

	}
}