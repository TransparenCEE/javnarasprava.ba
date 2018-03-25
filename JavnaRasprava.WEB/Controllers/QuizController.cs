using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Models.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace JavnaRasprava.WEB.Controllers
{
	public class QuizController : BaseController
	{
		// GET: Quiz
		[HttpGet]
		[Route( "Quiz", Name = "leg.Quiz.Index" )]
		[Route( "{pCode}/Quiz", Name = "def.Quiz.Index" )]
		[Route( "{pCode}/Quiz", Name = "sq.Quiz.Index" )]
		[Route( "{pCode}/Kvizovi", Name = "bs.Quiz.Index" )]
		public ActionResult Index( string pCode )
		{
			var parliamentId = new ParliamentService().GetParliamentId( pCode );

			var service = new QuizService();
			var model = service.InitializeSearchModel( parliamentId );
			return View( model );
		}

		public ActionResult Search( QuizSearchModel searchModel, int page = 1 )
		{
			var service = new QuizService();
			var model = service.SearchQuiz( searchModel );

			var resultModel = new QuizListModel
			{
				SearchModel = searchModel,
				Results = model
			};

			return View( "Index", resultModel );
		}



		// GET: Quiz/Details/5
		[Route( "Quiz/Take/{id}", Name = "leg.Quiz.Take" )]
		[Route( "{pCode}/Quiz/Take/{id}", Name = "def.Quiz.Take" )]
		public ActionResult Take( int id, int? questionId = null )
		{
			QuizService service = new QuizService();

			var model = service.GetQuizAnsweringModel( id, questionId, User.Identity.GetUserId() );

			if (model == null)
				return HttpNotFound( "Quiz Not Found" );

			return View( model );
		}

		public ActionResult VoteLaw( LawVoteModel model )
		{
			LawService service = new LawService();

			service.VoteLaw( model.LawId, User.Identity.GetUserId(), Request.UserHostAddress, model.lawVote, model.customVoteAnswer );

			if ( model.NextQuestionId == -1 )
				return RedirectToAction( "Results", new { id = model.QuizId } );

			return RedirectToAction( "Take", new { id = model.QuizId, questionId = model.NextQuestionId } );
		}

		public ActionResult VoteSection( LawSectionVoteModel model )
		{
			LawService service = new LawService();

			service.VoteLawSection( model.SectionId, User.Identity.GetUserId(), Request.UserHostAddress, model.sectionVote, model.customSectionVoteAnswer );

			if ( model.NextQuestionId == -1 )
				return RedirectToAction( "Results", new { id = model.QuizId } );

			return RedirectToAction( "Take", new { id = model.QuizId, questionId = model.NextQuestionId } );
		}

		public ActionResult Results( int id )
		{
			var service = new QuizService();

			var model = service.GetQuizResults( id, User.Identity.GetUserId() );

			return View( model );
		}
	}
}
