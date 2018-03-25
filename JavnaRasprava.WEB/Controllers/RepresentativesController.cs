using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.Representative;
using Microsoft.AspNet.Identity;

namespace JavnaRasprava.WEB.Controllers
{
	[OutputCache( Duration = 0 )]
	public class RepresentativesController : BaseController
	{
		//[Route("Representatives", Name = "leg.Representatives.Index")]
		[Route( "{pCode}/Representatives", Name = "def.Representatives.Index" )]
		[Route( "{pCode}/Parlamentarci", Name = "bs.Representatives.Index" )]
		public ActionResult Index( string pCode )
		{
			var parliamentId = new ParliamentService().GetParliamentId( pCode );

			RepresentativeService service = new RepresentativeService();
			RepresentativeListModel model = service.GetAllRepresentativesForParliament( parliamentId );
			return View( model );
		}

		[HttpPost]
		[OutputCache( Duration = 0 )]
		public ActionResult FilterSearch( RepresentativeSearchModel searchModel )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeListModel resultModel = service.SearchForRepresentativesForParliament( searchModel );

			return PartialView( "_RepresentativesSearchResult", resultModel );
		}

		[Route( "{pcode}/Representatives/Representative", Name = "def.Representatives.Representative" )]
		[Route( "Representatives/Representative", Name = "leg.Representatives.Representative" )]
		[Route( "{pcode}/Representatives/Representative/{repID}", Name = "sq.Representatives.Representative" )]
		[Route( "{pcode}/Parlamentarac/{repID}", Name = "bs.Representatives.Representative" )]
		public ActionResult Representative( string pCode, int? repID )
		{
			if ( repID == null )
				return HttpNotFound( "No such Representative Found" );

			RepresentativeService service = new RepresentativeService();
			RepresentativeModel model = service.GetRepresentative( repID.Value, User.Identity.GetUserId() );

			if ( model == null )
				return HttpNotFound( "No such Representative Found" );

			if ( pCode == null || pCode != model.Representative.ParliamentHouse.Parliament.Code )
				return RedirectToRoute( Resources.Routes.Representatives_Representative,
					new { pCode = model.Representative.ParliamentHouse.Parliament.Code, repID } );

			return View( "Representative", model );
		}

		[Authorize]
		[Route( "Representatives/GetQuestionsModelForRepresentative", Name = "def.Representatives.GetQuestionsModelForRepresentative" )]
		public ActionResult GetQuestionsModelForRepresentative( int repId )
		{
			QuestionsService service = new QuestionsService();
			AskRepresentativeQuestionModel model = service.GetQuestionsmodelForRepresentative( repId, User.Identity.GetUserId() );
			return PartialView( "_AskRepresentativeQuestion", model );
		}

		[HttpPost]
		public ActionResult AskRepresentative( AskRepresentativeQuestionModel model )
		{
			QuestionsService service = new QuestionsService();
			if ( !ModelState.IsValid )
			{
				return PartialView( "_AskRepresentativeQuestion", model );
			}
			service.PostRepresentativeQuestion( model, User.Identity.GetUserId() );
			return PartialView( "_AskRepresentativeSuccess", model );
		}

		[HttpPost]
		public ActionResult LikeAnswer( int answerId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeAnswer( answerId, User.Identity.GetUserId(), true );
			return PartialView( "_AnswerLikes", CopyAnswerModelDataToRepresentativeAnswerModel( model ) );
		}

		//[HttpGet]
		//public ActionResult LikeAnswer( int answerId )
		//{
		//	return RedirectToAction( "Index", "JavnaRasprava" );
		//}

		[HttpPost]
		public ActionResult DislikeAnswer( int answerId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeAnswer( answerId, User.Identity.GetUserId(), false );

			return PartialView( "_AnswerLikes", CopyAnswerModelDataToRepresentativeAnswerModel( model ) );
		}

		[HttpPost]
		public ActionResult LikeQuestion( int questionId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeQuestion( questionId, User.Identity.GetUserId(), true );

			return PartialView( "_QuestionLikes", CopyQuestionModelDataToRepresentativeQuestionModel( model ) );
		}

		[HttpPost]
		public ActionResult DislikeQuestion( int questionId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeQuestion( questionId, User.Identity.GetUserId(), false );

			return PartialView( "_QuestionLikes", CopyQuestionModelDataToRepresentativeQuestionModel( model ) );
		}


		private static RepresentativeAnswerModel CopyAnswerModelDataToRepresentativeAnswerModel( AnswerModel model )
		{
			return new RepresentativeAnswerModel
			{
				DislikesCount = model.DislikesCount,
				ID = model.ID,
				LikesCount = model.LikesCount,
				UserLiked = model.UserLiked
			};
		}


		private static RepresentativeQuestionModel CopyQuestionModelDataToRepresentativeQuestionModel( QuestionModel model )
		{
			return new RepresentativeQuestionModel
			{
				DislikesCount = model.DislikesCount,
				ID = model.Id,
				LikesCount = model.LikesCount,
				UserLiked = model.UserLiked
			};
		}
	}
}