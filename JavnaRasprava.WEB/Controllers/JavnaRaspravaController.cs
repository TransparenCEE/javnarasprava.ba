using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.Resources;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.Law;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace JavnaRasprava.WEB.Controllers
{
	public class JavnaRaspravaController : BaseController
	{
		// GET: JavnaRasprava
		[Route( "{pcode}/JavnaRasprava", Name = "def.JavnaRasprava.Index" )]
		[Route( "{pcode}/KonsultimiPublik", Name = "sq.JavnaRasprava.Index" )]
		//[Route("JavnaRasprava", Name = "leg.JavnaRasprava.Index")]
		[OutputCache( CacheProfile = "DefaultProfile", VaryByParam = "pcode" )]
		public ActionResult Index( string pcode )
		{
			var parliamentService = new ParliamentService();
			var parliamentId = parliamentService.GetParliamentId( pcode );

			var service = new LawService();
			var repService = new RepresentativeService();
			var model = new LawHomeModel();

			model.LatestLaws = service.GetLatestLaws( AppConfig.GetIntValue( "JavnaRasprava.Index.LatestLaws", 5 ), parliamentId );
			model.LatestLaws.Title = GlobalLocalization.LatestLawsTitle;

			model.MostActive = service.GetMostActive( AppConfig.GetIntValue( "JavnaRasprava.Index.MostActive", 5 ), parliamentId );
			model.MostActive.Title = GlobalLocalization.MostActiveLawsTitle;

			model.NextLawsInVote = service.GetNextLawsInVote( AppConfig.GetIntValue( "JavnaRasprava.Index.NextLawsInVote", 5 ), parliamentId );
			model.NextLawsInVote.Title = GlobalLocalization.NextLawsInVoteTitle;

			model.TopRepresentatives = repService.GetTopRepresentatives( Infrastructure.AppConfig.GetIntValue( "JavnaRasprava.Index.TopRepresentatives", 5 ), parliamentId );

			model.Search = service.InitializeSearchModel();

			return View( model );
		}

		public ActionResult InfoBoxLaws()
		{
			var parliamentId = (int)this.HttpContext.Items["ParliamentId"];

			var service = new LawService();
			var model = service.GetInfoBoxLaws( parliamentId );
			model.Title = GlobalLocalization.PointedOutLawsTitle;

			return PartialView( "_InfoBoxLaws", model );
		}

		public ActionResult InfoBoxLawSections()
		{
			var parliamentId = (int)this.HttpContext.Items["ParliamentId"];

			var service = new LawService();
			var model = service.GetInfoBoxLawSections( parliamentId );
			model.Title = GlobalLocalization.PointedOutSectionsTitle;

			return PartialView( "_InfoBoxLawSections", model );
		}

		public ActionResult InfoBoxTop()
		{
			var parliamentId = (int)this.HttpContext.Items["ParliamentId"];

			var service = new LawService();
			var model = service.GetInfoBoxTop( parliamentId );
			model.Title = GlobalLocalization.PointedOutTop;

			return PartialView( "_InfoBoxTop", model );
		}

		[Route( "{pCode}/JavnaRasprava/LawDetails", Name = "def.JavnaRasprava.LawDetails" )]
		[Route( "JavnaRasprava/LawDetails", Name = "leg.JavnaRasprava.LawDetails" )]
		[Route( "{pCode}/Projektligji/{lawId}", Name = "sq.JavnaRasprava.LawDetails" )]
		[Route( "{pCode}/Zakon/{lawId}", Name = "bs.JavnaRasprava.LawDetails" )]
		public ActionResult LawDetails( int? lawId, string pCode, string scrollPosition, string sp )
		{
			if ( lawId == null )
				return HttpNotFound();

			LawService service = new LawService();
			LawModel model = service.GetLawModel( lawId.Value, User.Identity.GetUserId(), Infrastructure.CommentOrder.Chronological );

			if ( model == null )
				return HttpNotFound();

			new ParliamentService().GetTenantData( model.Law.ParliamentID, out string parliamentCode );
			if ( pCode == null || pCode != parliamentCode )
			{
				return RedirectToRoute( JavnaRasprava.Resources.Routes.JavnaRasprava_LawDetails, new { pCode = parliamentCode, lawId } );
			}

			QuestionsService questionsService = new QuestionsService();
			model.RepresentativeAnswers = questionsService.GetLatestAnswersForLaw( lawId.Value, User.Identity.GetUserId() );

			ViewBag.scrollPosition = sp ?? scrollPosition;

			model.FbCommentsPath = string.Format( ConfigurationManager.AppSettings["Facebook.LawUrlTemplate"], lawId );

			return View( "Law", model );
		}

		[Route( "JavnaRasprava/GetLawVoteOptions", Name = "def.JavnaRasprava.GetLawVoteOptionsVoteForLaw" )]
		public ActionResult GetLawVoteOptions( int lawId )
		{
			LawService service = new LawService();
			LawCustomVoteListModel model = service.GetLawCustomVotesList( lawId );
			return PartialView( "_LawVoteOptions", model );
		}

		[HttpPost]
		[Route( "JavnaRasprava/VoteForLaw", Name = "def.JavnaRasprava.VoteForLaw" )]
		public ActionResult VoteForLaw( int lawId, int answerId, string customAnswerText )
		{
			LawService service = new LawService();

			service.VoteLaw( lawId, User.Identity.GetUserId(), Request.UserHostAddress, answerId, customAnswerText );

			LawModel model = service.GetLawModel( lawId, User.Identity.GetUserId(), Infrastructure.CommentOrder.Chronological );

			return PartialView( "_LawVotingDetails", model );
			;
		}

		[Route( "JavnaRasprava/GetVotingDetails", Name = "def.JavnaRasprava.GetVotingDetails" )]
		public ActionResult GetVotingDetails( int id )
		{
			VotingService service = new VotingService();
			VotingResultsModel model = service.GetResultsForLaw( new VotingResultsModel { ID = id } );
			model.ActionName = "FilterVotingDetails";

			return PartialView( "_VotingDetails", model );
		}

		[HttpPost]
		public ActionResult FilterVotingDetails( VotingResultsModel model )
		{
			TryUpdateModel( model );

			VotingService service = new VotingService();
			model = service.GetResultsForLaw( model );
			model.ActionName = "FilterVotingDetails";

			return PartialView( "_VotingDetails", model );
		}

		[Route( "JavnaRasprava/GetSectionVotingDetails", Name = "def.JavnaRasprava.GetSectionVotingDetails" )]
		public ActionResult GetSectionVotingDetails( int id )
		{
			VotingService service = new VotingService();
			VotingResultsModel model = service.GetResultsForLawSection( new VotingResultsModel { ID = id } );
			model.ActionName = "FilterSectionVotingDetails";

			return PartialView( "_VotingDetails", model );
		}

		[HttpPost]
		public ActionResult FilterSectionVotingDetails( VotingResultsModel model )
		{
			TryUpdateModel( model );

			VotingService service = new VotingService();
			model = service.GetResultsForLawSection( model );
			model.ActionName = "FilterSectionVotingDetails";

			return PartialView( "_VotingDetails", model );
		}

		[Route( "JavnaRasprava/GetSectionVoteOptions", Name = "def.JavnaRasprava.GetSectionVoteOptions" )]
		public ActionResult GetSectionVoteOptions( int lawId, int sectionId )
		{
			LawService service = new LawService();
			LawSectionCustomVoteListModel model = service.getLawSectionCustomVotesList( lawId, sectionId );
			return PartialView( "_SectionVoteOptions", model );
		}

		[HttpPost]
		[Route( "JavnaRasprava/VoteForSection", Name = "def.JavnaRasprava.VoteForSection" )]
		public ActionResult VoteForSection( int lawId, int sectionId, int answerId, string customAnswerText )
		{
			LawService service = new LawService();
			service.VoteLawSection( sectionId, User.Identity.GetUserId(), Request.UserHostAddress, answerId, customAnswerText ); //TODO

			LawModel model = service.GetLawModel( lawId, User.Identity.GetUserId(), Infrastructure.CommentOrder.Chronological );

			return PartialView( "_SectionVotingDetails", model.Sections.Where( s => s.LawSection.LawSectionID == sectionId ).First() );
		}

		[Route( "JavnaRasprava/GetComments", Name = "def.JavnaRasprava.GetComments" )]
		public ActionResult GetComments( int lawId )
		{
			CommentsService service = new CommentsService();
			CommentsListModel model = service.GetCommentsForLaw( lawId, User.Identity.GetUserId(), Infrastructure.CommentOrder.Chronological );
			return PartialView( "_LawComments", model );

		}

		[Authorize]
		[HttpPost]
		public ActionResult AddComment( int lawId, CommentModel comment )
		{
			TryUpdateModel( comment );

			CommentsService service = new CommentsService();
			service.MakeComment( lawId, User.Identity.GetUserId(), comment.Comment.Text );
			CommentsListModel model = service.GetCommentsForLaw( lawId, User.Identity.GetUserId(), Infrastructure.CommentOrder.Chronological );
			return PartialView( "_LawComments", model );
		}

		[Authorize]
		[Route( "JavnaRasprava/GetLawQuestionModel", Name = "def.JavnaRasprava.GetLawQuestionModel" )]
		public ActionResult GetLawQuestionModel( int lawId )
		{
			QuestionsService service = new QuestionsService();
			AskLawQuestionModel model = service.GetQuestionsModel( lawId, User.Identity.GetUserId() );
			return PartialView( "Representatives/_AskRepresentatives", model );
		}

		[Authorize]
		[HttpPost]
		[Route( "JavnaRasprava/AskRepresentative", Name = "def.JavnaRasprava.AskRepresentative" )]
		public ActionResult AskRepresentative( int lawId, AskLawQuestionModel model )
		{
			TryUpdateModel( model );

			QuestionsService service = new QuestionsService();
			service.PostQuestion( model, User.Identity.GetUserId() );
			return PartialView( "Representatives/_AskRepresentativesSuccess" );
		}

		public ActionResult GetLawQuestionsDetails( int lawId )
		{
			QuestionsService service = new QuestionsService();
			LawQuestionsModel model = service.GetQuestionsForLaw( lawId, User.Identity.GetUserId() );
			return View( "LawQuestionsDetails", model );
		}

		[HttpPost]
		public ActionResult LikeAnswer( int answerId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeAnswer( answerId, User.Identity.GetUserId(), true );

			return PartialView( "_AnswerLikes", model );
		}

		[HttpPost]
		public ActionResult DislikeAnswer( int answerId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeAnswer( answerId, User.Identity.GetUserId(), false );

			return PartialView( "_AnswerLikes", model );
		}

		[HttpPost]
		public ActionResult LikeQuestion( int questionId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeQuestion( questionId, User.Identity.GetUserId(), true );

			return PartialView( "_QuestionLikes", model );
		}

		[HttpPost]
		public ActionResult DislikeQuestion( int questionId )
		{
			QuestionsService service = new QuestionsService();

			var model = service.LikeQuestion( questionId, User.Identity.GetUserId(), false );

			return PartialView( "_QuestionLikes", model );
		}

		[Authorize]
		public ActionResult GetAdminModal( int representativeId, int questionId, int lawId )
		{
			var model = new RepresentativeAnswerTestModel { QuestionId = questionId, RepresentativeId = representativeId, LawId = lawId };

			return PartialView( "_AdminModal", model );
		}

		[Authorize]
		[HttpPost]
		public ActionResult SubmitRepresentativeAnswer( RepresentativeAnswerTestModel model )
		{
			QuestionsService service = new QuestionsService();

			TryUpdateModel( model );

			service.PostAnswer( model.QuestionId, model.RepresentativeId, model.Answer );

			return RedirectToAction( "GetLawQuestionsDetails", "JavnaRasprava", new { lawId = model.LawId } );

		}

		[HttpGet]
		[Route( "JavnaRasprava/PostAnswer", Name = "def.JavnaRasprava.PostAnswerGet" )]
		[Route( "konsultimipublik/PostAnswer", Name = "sq.JavnaRasprava.PostAnswerGet" )]
		public ActionResult PostAnswer( string token )
		{
			AnsweringService service = new AnsweringService();
			var model = service.InitializePostAnswerModel( token );
			return View( model );
		}

		[HttpPost]
		[Route( "JavnaRasprava/PostAnswer", Name = "def.JavnaRasprava.PostAnswer" )]
		[Route( "konsultimipublik/PostAnswer", Name = "sq.JavnaRasprava.PostAnswer" )]
		public ActionResult PostAnswer( PostAnswerModel model, string test )
		{
			TryUpdateModel( model );

			AnsweringService service = new AnsweringService();
			var success = service.PostAnswerModel( model );

			var returnModel = new ThankYouModel
			{
				LawID = model.LawID,
				RepresentativeDisplayName = model.RepresentativeDisplayName,
				RepresentativeID = model.RepresentativeID,
			};

			// Redirect to action is a hack as html input is causing issues
			if ( success )
				return RedirectToAction( "ThankYou", returnModel );

			return RedirectToAction( "AnswerRejected", returnModel );
		}

		public ActionResult ThankYou( ThankYouModel model )
		{
			return View( model );
		}

		public ActionResult AnswerRejected( ThankYouModel model )
		{
			return View( model );
		}

		[HttpGet]
		public ActionResult Search()
		{
			LawService service = new LawService();
			LawSearchModel model = service.InitializeSearchModel();
			return View( model );
		}

		[HttpGet]
		[OutputCache( Duration = 0 )]
		public ActionResult FilterSearch( int page = 1, string sort = "AskedCount", string sortDir = "DESC", string title = "", int? parliamentId = null, int? categoryId = null, string queryString = "" )
		{
			LawService service = new LawService();
			LawSearchModel filter = new LawSearchModel
			{
				SortBy = sort,
				Order = sortDir,
				PageItemCount = 1000,
				page = null,
				ParliamentId = parliamentId.HasValue ? parliamentId.Value : 0,
				CategoryId = categoryId.HasValue ? categoryId.Value : 0,
				QueryString = queryString
			};
			var model = service.SearchLaw( filter );
			return PartialView( "_SearchResult", model );
		}
	}
}