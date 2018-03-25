using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Models;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Controllers
{
	public class AllLawsController : BaseController
	{
		[Route( "{pCode}/AllLaws", Name = "def.AllLaws.Index" )]
		//[Route("AllLaws", Name = "leg.AllLaws.Index")]
		[Route( "{pCode}/Projektligjet", Name = "sq.AllLaws.Index" )]
		[Route( "{pCode}/Zakoni", Name = "bs.AllLaws.Index" )]
		public ActionResult Index( string pCode )
		{
			var parliamentId = new ParliamentService().GetParliamentId( pCode );

			var service = new LawService();
			var searchModel = service.InitializeSearchModel();
			searchModel.Order = "desc";
			searchModel.SortBy = "createTime";
			searchModel.ParliamentId = parliamentId;
			searchModel.PageItemCount = 16;
			var results = service.SearchLaw( searchModel );

			var model = new AllLawsModel
			{
				SearchModel = searchModel,
				Results = results
			};

			return View( model );
		}

		public ActionResult Search( Models.LawSearchModel model )
		{
			var service = new LawService();
			var searchModel = service.InitializeSearchModel();

			searchModel.QueryString = model.QueryString;
			searchModel.page = model.page;
			searchModel.CategoryId = model.CategoryId;
			searchModel.ParliamentId = model.ParliamentId;
			searchModel.PageItemCount = 16;
			searchModel.LawSort = model.LawSort;


			switch ( model.LawSort )
			{

				case Infrastructure.LawSort.QuestionCount:
					searchModel.Order = "desc";
					searchModel.SortBy = "askedcount";
					break;
				case Infrastructure.LawSort.Title:
					searchModel.Order = "asc";
					searchModel.SortBy = "title";
					break;
				case Infrastructure.LawSort.VoteTime:
					searchModel.Order = "desc";
					searchModel.SortBy = "voteTime";
					break;
				case Infrastructure.LawSort.CreateTime:
				default:
					searchModel.Order = "desc";
					searchModel.SortBy = "createTime";
					break;
			}

			var results = service.SearchLaw( searchModel );

			var resultModel = new AllLawsModel
			{
				SearchModel = searchModel,
				Results = results
			};

			return View( "Index", resultModel );
		}
	}
}