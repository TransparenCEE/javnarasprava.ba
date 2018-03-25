using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Controllers
{
	public class NewsController : BaseController
	{
		[Route( "{pCode}/News/Details", Name = "def.News.Details" )]
		//[Route("News/Details", Name = "leg.News.Details")]
		[Route( "{pCode}/Lajmet/{id}", Name = "sq.News.Details" )]
		[Route( "{pCode}/Vijest/{id}", Name = "bs.News.Details" )]
		public ActionResult Details( int id )
		{
			var service = new NewsService();
			var model = service.Get( id );
			return View( model );
		}

		[Route( "{pCode}/News", Name = "def.News.Index" )]
		//[Route("News", Name = "leg.News.Index")]
		[Route( "{pCode}/Lajmet", Name = "sq.News.Index" )]
		[Route( "{pCode}/Vijesti", Name = "bs.News.Index" )]
		[HttpGet]
		public ActionResult Index( string pCode )
		{
			var parliamentId = new ParliamentService().GetParliamentId( pCode );

			var service = new NewsService();
			var model = service.InitializeSearchModel( parliamentId );
			return View( model );
		}

		public ActionResult Search( NewsSearchModel searchModel, int page = 1 )
		{
			var service = new NewsService();

			var model = service.SearchNews( searchModel );

			var resultModel = new NewsListModel
			{
				SearchModel = searchModel,
				Results = model
			};

			return View( "Index", resultModel );
		}
	}
}
