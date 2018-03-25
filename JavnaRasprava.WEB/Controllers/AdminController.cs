using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.Admin;
using JavnaRasprava.WEB.Models.Law;
using JavnaRasprava.WEB.Models.Petition;
using JavnaRasprava.WEB.Models.Representative;
using Microsoft.AspNet.Identity;
using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models.User;
using Microsoft.AspNet.Identity.Owin;

namespace JavnaRasprava.WEB.Controllers
{
	[Authorize( Roles = "admin" )]
	public class AdminController : BaseController
	{
		#region Index

		[Route( "admin", Name = "def.Admin.Index" )]
		public ActionResult Index()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			LawService lawService = new LawService();
			PetitionService petitionService = new PetitionService();

			var model = new AdminIndexModel
			{
				UnverifiedCustomVotesCount = lawService.GetUnverifiedCustomVotesCount(),
				UnverifiedLawSectionCustomVoteCount = lawService.GetUnverifiedLawSectionCustomVotesCount(),
				UnverifiedQuestionsCount = lawService.GetUnverifiedQuestionsCount(),
				UnverifiedRepresentativeQuestionsCount = lawService.GetUnverifiedRepresentativeQuestionsCount(),
				UnverifiedPetitionsCount = petitionService.GetUnverifiedPetitionsCount()
			};

			return View( model );
		}

		public ActionResult IndexUnverifiedPetitions()
		{
			PetitionService petitionService = new PetitionService();
			var model = petitionService.GetUnverifiedPetitions();

			return PartialView( "_IndexUnverifiedPetitions", model );

		}

		public ActionResult IndexUnverifiedAnswers()
		{
			LawService lawService = new LawService();
			var model = lawService.GetUnverifiedCustomVotes();

			return PartialView( "_IndexUnverifiedAnswers", model );

		}

		public ActionResult IndexUnverifiedSectionAnswers()
		{
			LawService lawService = new LawService();
			var model = lawService.GetUnverifiedLawSectionCustomVotes();

			return PartialView( "_IndexUnverifiedSectionAnswers", model );

		}

		public ActionResult IndexUnverifiedQuestions()
		{
			LawService lawService = new LawService();
			var model = lawService.GetUnverifiedQuestions();

			return PartialView( "_IndexUnverifiedQuestions", model );

		}

		public ActionResult IndexUnverifiedDirectQuestions()
		{
			LawService lawService = new LawService();
			var model = lawService.GetUnverifiedRepresentativeQuestions();

			return PartialView( "_IndexUnverifiedDirectQuestions", model );

		}
		#endregion

		#region Representatives

		[HttpPost]
		public ActionResult CreateRepresentativeAssignment( int representativeId, RepresentativeAssignmentModel model )
		{
			TryUpdateModel( model );
			RepresentativeService service = new RepresentativeService();
			model = service.CreateAssignment( representativeId, model.Title );
			return PartialView( "_RepresentativeAssignmentItem", model );
		}

		[HttpPost]
		public ActionResult CreateRepresentativeExternalLink( int representativeId, RepresentativeExternalLinkModel model )
		{
			TryUpdateModel( model );
			RepresentativeService service = new RepresentativeService();
			model = service.CreateLink( representativeId, model.Description, model.Url );
			return PartialView( "_RepresentativeExternalLinkItem", model );
		}

		[HttpGet]
		public ActionResult CreateRepresentative( int parliamentHouseId )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeEditModel model = service.InitializeRepresentativeModelForCreate( parliamentHouseId );
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateRepresentative( RepresentativeEditModel model )
		{
			TryUpdateModel( model );


			RepresentativeService service = new RepresentativeService();
			var representativeId = service.CreateRepresentative( model );
			return RedirectToAction( "RepresentativeDetails", new { repId = representativeId } );
		}

		[HttpGet]
		public ActionResult EditRepresentative( int repId )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeEditModel model = service.GetRepresentativeEditModel( repId );
			return View( model );
		}

		[HttpPost, ValidateInput( false )]
		public ActionResult EditRepresentative( RepresentativeEditModel model )
		{
			TryUpdateModel( model );
			RepresentativeService service = new RepresentativeService();
			model = service.UpdateRepresentative( model );
			return RedirectToAction( "RepresentativeDetails", new { repId = model.RepresentativeID } );
		}

		[HttpPost]
		public ActionResult DeleteRepresentativeExternalLinkItem( int externalLinkID )
		{
			RepresentativeService service = new RepresentativeService();
			var isDeleted = service.DeleteExternalLink( externalLinkID );
			return Json( new { isDeleted = isDeleted, externalLinkID = externalLinkID }, JsonRequestBehavior.AllowGet ); ;
		}

		[HttpPost]
		public ActionResult DeleteRepresentativeAssignment( int assignmentId )
		{
			RepresentativeService service = new RepresentativeService();
			var isDeleted = service.DeleteAssignment( assignmentId );
			return Json( new { isDeleted = isDeleted, assignmentId = assignmentId }, JsonRequestBehavior.AllowGet ); ;
		}

		[HttpGet]
		public ActionResult DeleteRepresentative( int repId )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeEditModel model = service.GetRepresentativeEditModel( repId );
			return View( model );
		}

		[HttpPost]
		public ActionResult DeleteRepresentative( int repId, string test )
		{
			RepresentativeService service = new RepresentativeService();
			service.DeleteRepresentative( repId );
			return RedirectToAction( "ManageRepresentatives" );
		}


		[HttpGet]
		public ActionResult ManageRepresentativeAssigments( int repId )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeEditModel model = service.GetRepresentativeEditModel( repId );
			return View( model );
		}

		[HttpGet]
		public ActionResult ManageRepresentativeExternalLinks( int repId )
		{
			RepresentativeService service = new RepresentativeService();
			RepresentativeEditModel model = service.GetRepresentativeEditModel( repId );
			return View( model );
		}

		[HttpGet]
		public ActionResult RepresentativeDetails( int repId )
		{
			var service = new RepresentativeService();
			var model = service.GetRepresentativeEditModel( repId );
			return View( model );
		}

		[HttpGet]
		public ActionResult RepresentativeQuestions( int repId )
		{
			var service = new QuestionsService();
			var model = service.GetQuestionMessagesForRepresentative( repId );
			return PartialView( "QuestionMessage/_representativeQuestions", model );
		}

		[HttpGet]
		public ActionResult ManageRepresentatives()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			RepresentativeService service = new RepresentativeService();
			RepresentativeListModel model = service.GetAllRepresentativesForParliament( SessionManager.Current.CurrentParliamentId );
			return View( model );
		}

		[HttpGet]
		public ActionResult EditRepresentativeQuestion( int questionId )
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			var service = new LawService();
			var model = service.GetRepresentativeQuestionEditModel( questionId );

			if ( model == null )
				return HttpNotFound();

			return View( model );
		}

		[HttpPost, ValidateInput( false )]
		public ActionResult EditRepresentativeQuestion( RepresentativeQuestionEditModel model )
		{
			TryValidateModel( model );
			var service = new LawService();
			service.UpdateRepresentativeQuestionEditModel( model );
			return RedirectToAction( "RepresentativeDetails", new { repId = model.RepresentativeId } );
		}

		[HttpPost, ValidateInput( false )]
		public ActionResult ProcessRepresentativeQuestions( List<int> questionId, int representativeId )
		{

			var service = new NotificationService();
			service.ProcessNewQuestions( questionId );
			return new EmptyResult();
		}
		#endregion

		#region Law Details

		[HttpGet]
		public ActionResult CreateLaw()
		{
			LawService service = new LawService();
			var model = service.InitializeLawEditModel( SessionManager.Current.CurrentParliamentId );
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateLaw( LawEditModel model )
		{
			LawService service = new LawService();

			if ( !TryUpdateModel( model ) )
			{
				service.InitializeExistingLawEditModel( model, SessionManager.Current.CurrentParliamentId );
				return View( model );
			}

			model = service.CreateLaw( model );
			return RedirectToAction( "LawDetails", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult DeleteLaw( int lawId )
		{
			LawService service = new LawService();
			LawEditModel model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult DeleteLaw( int lawId, string test )
		{
			LawService service = new LawService();
			service.DeleteLaw( lawId );
			return RedirectToAction( "ManageLaws" );
		}

		[HttpGet]
		public ActionResult EditLaw( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult EditLaw( LawEditModel model )
		{
			LawService service = new LawService();

			if ( !TryUpdateModel( model ) )
			{
				service.InitializeExistingLawEditModel( model, SessionManager.Current.CurrentParliamentId );
				return View( model );
			}

			model = service.UpdateLaw( model );
			return RedirectToAction( "LawDetails", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult LawDetails( int lawId )
		{
			LawService service = new LawService();
			LawEditModel model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpGet]
		public ActionResult ManageLaws()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;
			LawService service = new LawService();

			var model = service.GetLawTitles( SessionManager.Current.CurrentParliamentId );
			return View( model );
		}

		[HttpPost]
		public ActionResult PointOutLaw( int lawId )
		{
			LawService service = new LawService();
			service.PointOut( lawId );

			return Json( new { isSuccess = true }, JsonRequestBehavior.AllowGet );
		}


		[HttpPost]
		public ActionResult PointOutLawRemove( int lawId )
		{
			LawService service = new LawService();
			service.PointOut( lawId, null );

			return Json( new { isSuccess = true }, JsonRequestBehavior.AllowGet );
		}

		#endregion

		#region Experts

		[HttpGet]
		public ActionResult GetAllExperts()
		{
			var service = new ExpertService();
			var model = service.GetAll();
			return View( "Experts/Index", model );
		}
		[HttpGet]
		public ActionResult ExpertDetails( int id )
		{
			var service = new ExpertService();
			var model = service.Get( id );
			return View( "Experts/Details", model );
		}

		[HttpGet]
		public ActionResult ExpertCreate()
		{
			return View( "Experts/Create" );
		}

		// POST: Experts/Create
		[HttpPost]
		public ActionResult ExpertCreate( DomainModels.Expert model )
		{
			var service = new ExpertService();
			var result = service.CreateExpert( model );
			return RedirectToAction( "GetAllExperts" );
		}

		// GET: Experts/Edit/5
		public ActionResult ExpertEdit( int id )
		{
			var service = new ExpertService();
			var model = service.Get( id );
			return View( "Experts/Edit", model );
		}

		// POST: Experts/Edit/5
		[HttpPost]
		public ActionResult ExpertEdit( int id, DomainModels.Expert model )
		{
			var service = new ExpertService();
			service.UpdateExpert( model );
			return RedirectToAction( "GetAllExperts" );
		}

		// GET: Experts/Delete/5
		public ActionResult ExpertDelete( int id )
		{
			var service = new ExpertService();
			var model = service.Get( id );
			return View( "Experts/Delete", model );
		}


		// POST: Experts/Delete/5
		[HttpPost]
		public ActionResult ExpertDelete( int id, Expert collection )
		{
			var service = new ExpertService();
			service.Delete( id );
			return RedirectToAction( "GetAllExperts" );

		}

		#endregion

		#region Law expert comments

		[HttpGet]
		public ActionResult ManageLawExpertComments( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpGet]
		public ActionResult CreateLawExpertComment( int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializeCreateExpertCommentModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateLawExpertComment( CreateExpertcommentModel model )
		{
			TryUpdateModel( model );
			LawService service = new LawService();
			service.CreateExpertComment( model );
			return RedirectToAction( "ManageLawExpertComments", new { lawId = model.LawID } );
		}

		[HttpPost]
		public ActionResult DeleteLawExpertComment( int lawExpertCommentId )
		{
			LawService service = new LawService();
			var isDeleted = service.DeleteExpertComment( lawExpertCommentId );
			return Json( new { isDeleted = isDeleted, lawExpertCommentId = lawExpertCommentId }, JsonRequestBehavior.AllowGet ); ;
		}
		#endregion

		#region Law sections

		[HttpGet]
		public ActionResult CreateLawSection( int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializeLawSectionEditModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateLawSection( LawSectionEditModel model )
		{
			TryUpdateModel( model );
			LawService service = new LawService();
			service.CreateLawSectionEditModel( model );
			return RedirectToAction( "ManageLawSections", new { lawId = model.LawID } );
		}


		[HttpGet]
		public ActionResult CreateSectionCustomVote( int lawSectionId, int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializeSectionCustomVoteEditModel( lawSectionId, lawId );
			model.IsSuggested = true;
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateSectionCustomVote( LawSectionCustomVoteModel model )
		{
			TryUpdateModel( model );
			LawService service = new LawService();
			service.CreateLawSectionCustomVote( model );
			return RedirectToAction( "ManageLawSections", new { lawId = model.LawID } );
		}

		[HttpPost]
		public ActionResult DeleteLawSection( int lawSectionId )
		{
			LawService service = new LawService();
			var isDeleted = service.DeleteLawSection( lawSectionId );
			return Json( new { isDeleted = isDeleted, lawSectionId = lawSectionId }, JsonRequestBehavior.AllowGet );
		}

		[HttpPost]
		public ActionResult DeleteLawSectionCustomVote( int lawSectionCustomVoteID )
		{
			LawService service = new LawService();
			var isDeleted = service.DeleteLawSectionCustomVote( lawSectionCustomVoteID );
			return Json( new { isDeleted = isDeleted, lawSectionCustomVoteID = lawSectionCustomVoteID }, JsonRequestBehavior.AllowGet );
		}

		[HttpGet]
		public ActionResult EditLawSection( int lawSectionId )
		{
			LawService service = new LawService();
			var model = service.GetLawSectionEditModel( lawSectionId );
			return View( model );
		}

		[HttpPost]
		public ActionResult EditLawSection( LawSectionEditModel model )
		{
			TryUpdateModel( model );

			LawService service = new LawService();
			model = service.UpdateLawSectionEditModel( model );
			return RedirectToAction( "ManageLawSections", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult EditLawSectionCustomVote( int lawSectionCustomVoteID )
		{
			LawService service = new LawService();
			var model = service.GetLawSectionCustomVoteEditModel( lawSectionCustomVoteID );
			return View( model );
		}

		[HttpPost]
		public ActionResult EditLawSectionCustomVote( LawSectionCustomVoteModel model )
		{
			TryUpdateModel( model );

			LawService service = new LawService();
			model = service.UpdateLawSectionCustomVote( model );
			return RedirectToAction( "ManageLawSections", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult ManageLawSections( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawSectionEditModelList( lawId );
			return View( model );
		}


		[HttpPost]
		public ActionResult PointOutLawSection( int lawSectionId )
		{
			LawService service = new LawService();
			service.PointOutSection( lawSectionId );

			return Json( new { isSuccess = true }, JsonRequestBehavior.AllowGet );
		}

		[HttpPost]
		public ActionResult PointOutLawSectionRemove( int lawSectionId )
		{
			LawService service = new LawService();
			service.PointOutSection( lawSectionId, null );

			return Json( new { isSuccess = true }, JsonRequestBehavior.AllowGet );
		}

		#endregion

		#region Law custom votes

		[HttpGet]
		public ActionResult CreateLawCustomVote( int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializeCustomVoteEditModel( lawId );
			model.IsSuggested = true;
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateLawCustomVote( CustomVoteEditModel model )
		{
			TryUpdateModel( model );
			LawService service = new LawService();
			service.CreateCustomVote( model );
			return RedirectToAction( "ManageLawCustomVotes", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult EditLawCustomVote( int customVoteId )
		{
			LawService service = new LawService();
			var model = service.GetCustomVoteEditModel( customVoteId );
			return View( model );
		}

		[HttpPost]
		public ActionResult EditLawCustomVote( CustomVoteEditModel model )
		{
			TryUpdateModel( model );

			LawService service = new LawService();
			model = service.UpdateCustomVote( model );
			return RedirectToAction( "ManageLawCustomVotes", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult ManageLawCustomVotes( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult DeleteLawCustomVote( int customVoteId )
		{
			LawService service = new LawService();
			var isDeleted = service.DeleteCustomVote( customVoteId );
			return Json( new { isDeleted = isDeleted, customVoteId = customVoteId }, JsonRequestBehavior.AllowGet ); ;
		}

		#endregion

		#region Law questions

		[HttpGet]
		public ActionResult CreateLawQuestion( int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializePredefinedQuestionEditModel( lawId );
			model.IsSuggested = true;
			model.Verified = true;
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateLawQuestion( PredefinedQuestionEditModel model )
		{
			TryUpdateModel( model );
			model.IsSuggested = true;
			LawService service = new LawService();
			service.CreatePredefinedQuestionEditModel( model );
			return RedirectToAction( "ManageLawQuestions", new { lawId = model.LawID } );
		}


		[HttpGet]
		public ActionResult EditLawQuestion( int questionId )
		{
			LawService service = new LawService();
			var model = service.GetPredefinedQuestionEditModel( questionId );
			return View( model );
		}

		[HttpPost]
		public ActionResult EditLawQuestion( PredefinedQuestionEditModel model )
		{
			TryUpdateModel( model );

			LawService service = new LawService();
			model = service.UpdatePredefinedQuestionEditModel( model );
			return RedirectToAction( "ManageLawQuestions", new { lawId = model.LawID } );
		}

		[HttpGet]
		public ActionResult ManageLawQuestions( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawEditModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult DeleteLawQuestion( int questionId )
		{
			LawService service = new LawService();
			var isDeleted = service.DeletePredefinedQuestionEditModel( questionId );
			return Json( new { isDeleted = isDeleted, questionId = questionId }, JsonRequestBehavior.AllowGet ); ;
		}

		#endregion

		#region Law suggested representatives

		[HttpGet]
		public ActionResult CreateSuggestedRepresentative( int lawId )
		{
			LawService service = new LawService();
			var model = service.InitializeCreateLawRepresentativeModel( lawId );
			return View( model );
		}

		[HttpPost]
		public ActionResult CreateSuggestedRepresentative( CreateLawRepresentativeModel model )
		{
			TryUpdateModel( model );
			LawService service = new LawService();
			service.AddLawRepresentative( model );
			return RedirectToAction( "ManageLawSuggestedRepresentatives", new { lawId = model.LawID } );
		}

		[HttpPost]
		public ActionResult DeleteLawSuggestedRepresentative( int lawRepresentativeAssociationID )
		{
			LawService service = new LawService();
			var isDeleted = service.DeleteLawRepresentative( lawRepresentativeAssociationID );
			return Json( new { isDeleted = isDeleted, lawRepresentativeAssociationID = lawRepresentativeAssociationID }, JsonRequestBehavior.AllowGet ); ;
		}

		[HttpGet]
		public ActionResult ManageLawSuggestedRepresentatives( int lawId )
		{
			LawService service = new LawService();
			var model = service.GetLawEditModel( lawId );
			return View( model );
		}
		#endregion

		#region Petitions

		[HttpGet]
		public ActionResult DeletePetition( int petitionId )
		{
			PetitionService service = new PetitionService();
			PetitionModel model = service.GetPetition( petitionId );
			return View( "Petition/DeletePetition", model );
		}

		[HttpPost]
		public ActionResult DeletePetition( int petitionId, string test )
		{
			PetitionService service = new PetitionService();
			service.DeletePetition( petitionId );
			return RedirectToAction( "ManagePetitions" );
		}

		[HttpGet]
		public ActionResult EditPetition( int petitionId )
		{
			PetitionService service = new PetitionService();
			PetitionModel model = service.GetPetition( petitionId );
			return View( "Petition/EditPetition", model );
		}

		[HttpPost, ValidateInput( false )]
		public ActionResult EditPetition( PetitionModel model )
		{
			TryUpdateModel( model );
			PetitionService service = new PetitionService();
			service.UpdatePetition( model, User.Identity.GetUserId() );
			return RedirectToAction( "PetitionDetails", new { petitionId = model.PetitionID } );
		}

		[HttpGet]
		public ActionResult PetitionDetails( int petitionId )
		{
			PetitionService service = new PetitionService();
			PetitionModel model = service.GetPetition( petitionId );
			return View( "Petition/PetitionDetails", model );
		}

		[HttpGet]
		public ActionResult ManagePetitions()
		{
			PetitionService service = new PetitionService();
			var model = service.GetAllPetitions();
			return View( "Petition/ManagePetitions", model );
		}
		#endregion

		#region Petition Progresses

		public ActionResult ManagePetitionProgresses()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			PetitionService service = new PetitionService();
			var model = service.GetAllProgresses( SessionManager.Current.CurrentParliamentId );
			return View( "PetitionProgress/ManagePetitionProgresses", model );
		}

		public ActionResult CreatePetitionProgresses()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			PetitionService service = new PetitionService();
			var model = service.InitializePetitionProgressEditModel( SessionManager.Current.CurrentParliamentId );
			return View( "PetitionProgress/CreatePetitionProgresses", model );
		}

		[HttpPost]
		public ActionResult CreatePetitionProgresses( PetitionProgressEditModel model )
		{
			TryUpdateModel( model );
			PetitionService service = new PetitionService();
			int progressId = service.CreatePetitionProgressEditModel( model );
			return RedirectToAction( "PetitionProgressesDetails", new { petitionProgressId = progressId } );
		}

		public ActionResult EditPetitionProgresses( int petitionProgressId )
		{
			PetitionService service = new PetitionService();
			var model = service.GetPetitionProgressEditModel( petitionProgressId );
			return View( "PetitionProgress/EditPetitionProgresses", model );
		}

		[HttpPost]
		public ActionResult EditPetitionProgresses( PetitionProgressEditModel model )
		{
			TryUpdateModel( model );
			PetitionService service = new PetitionService();
			service.UpdatePetitionProgressEditModel( model );
			return RedirectToAction( "PetitionProgressesDetails", new { petitionProgressId = model.PetitionProgresID } );
		}

		public ActionResult PetitionProgressesDetails( int petitionProgressId )
		{
			PetitionService service = new PetitionService();
			var model = service.GetPetitionProgressEditModel( petitionProgressId );
			return View( "PetitionProgress/PetitionProgressesDetails", model );
		}

		public ActionResult DeletePetitionProgresses( int petitionProgressId )
		{
			PetitionService service = new PetitionService();
			var model = service.GetPetitionProgressEditModel( petitionProgressId );
			return View( "PetitionProgress/DeletePetitionProgresses", model );
		}

		[HttpPost]
		public ActionResult DeletePetitionProgresses( int petitionProgressId, string test )
		{
			PetitionService service = new PetitionService();
			var model = service.DeletePetitionProgressEditModel( petitionProgressId );
			return RedirectToAction( "ManagePetitionProgresses" );
		}

		#endregion

		#region Quiz

		[HttpGet]
		public ActionResult ManageQuiz()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			var service = new QuizService();
			var model = service.GetAllQuiz( SessionManager.Current.CurrentParliamentId );
			return View( "Quiz/Index", model );
		}

		[HttpGet]
		public ActionResult QuizDetails( int id )
		{
			var service = new QuizService();
			var model = service.GetQuizEditModel( id );
			return View( "Quiz/Details", model );
		}

		[HttpGet]
		public ActionResult EditQuiz( int id )
		{
			var service = new QuizService();
			var model = service.GetQuizEditModel( id );
			return View( "Quiz/Edit", model );
		}

		[HttpPost]
		public ActionResult EditQuiz( Models.Quiz.QuizEditModel model )
		{
			var service = new QuizService();
			service.UpdateQuiz( model );
			return RedirectToAction( "QuizDetails", new { id = model.QuizId } );
		}

		[HttpGet]
		public ActionResult DeleteQuiz( int id )
		{
			var service = new QuizService();
			var model = service.GetQuizEditModel( id );
			return View( "Quiz/Delete", model );
		}

		[HttpPost]
		public ActionResult DeleteQuiz( int id, string test )
		{
			var service = new QuizService();
			service.DeleteQuiz( id );
			return RedirectToAction( "ManageQuiz" );
		}

		[HttpGet]
		public ActionResult CreateQuiz()
		{
			var service = new QuizService();
			var model = service.InitializeQuizEditModel( SessionManager.Current.CurrentParliamentId );
			return View( "Quiz/Create", model );
		}

		// POST: Experts/Create
		[HttpPost]
		public ActionResult CreateQuiz( Models.Quiz.QuizEditModel model )
		{
			var service = new QuizService();
			service.CreateQuiz( model );
			return RedirectToAction( "ManageQuiz" );
		}

		[HttpGet]
		public ActionResult EditQuizItems( int id )
		{
			var service = new QuizService();
			var model = service.GetQuizEditModel( id );
			return View( "Quiz/EditQuizItems", model );
		}

		[HttpGet]
		public ActionResult CreateQuizItem( int id )
		{
			var service = new QuizService();
			var model = service.InitializeEditQuizItemModel( id, null );
			return View( "Quiz/CreateQuizItem", model );
		}

		[HttpPost]
		public ActionResult CreateQuizItem( Models.Quiz.QuizItemEditModel model )
		{
			var service = new QuizService();
			service.CreateNewQuizItemModel( model );
			return RedirectToAction( "EditQuizItems", new { id = model.QuizId } );
		}


		[HttpGet]
		public ActionResult EditQuizItem( int quizId, int itemId )
		{
			var service = new QuizService();
			var model = service.InitializeEditQuizItemModel( quizId, itemId );
			return View( "Quiz/EditQuizItem", model );
		}

		[HttpPost]
		public ActionResult EditQuizItem( Models.Quiz.QuizItemEditModel model )
		{
			var service = new QuizService();
			service.UpdateQuizItem( model );
			return RedirectToAction( "QuizDetails", new { id = model.QuizId } );
		}

		[HttpGet]
		public ActionResult DeleteQuizItem( int id )
		{
			var service = new QuizService();
			var quizId = service.DeleteQuizItem( id );
			return RedirectToAction( "EditQuizItems", new { id = quizId } );
		}
		[HttpPost]
		public ActionResult GetLawSections( int id )
		{
			var service = new LawService();
			var data = service.GetLawSectionTitles( id );
			var transformedData = data.Select( x => new { LawSectionId = x.LawSectionID, Title = x.Title } ).ToList();
			transformedData.Insert( 0, new { LawSectionId = 0, Title = "" } );
			return Json( transformedData, JsonRequestBehavior.AllowGet );
		}

		#endregion

		#region Parliament

		[HttpGet]
		public ActionResult ManageParliament()
		{
			var service = new ParliamentService();
			var model = service.GetAllParliamentEditModels();
			return View( "Parliament/Index", model );
		}

		[HttpGet]
		public ActionResult ParliamentDetails( int id )
		{
			var service = new ParliamentService();
			var model = service.GetParliamentEditModel( id );
			return View( "Parliament/Details", model );
		}

		[HttpGet]
		public ActionResult EditParliament( int id )
		{
			var service = new ParliamentService();
			var model = service.GetParliamentEditModel( id );
			return View( "Parliament/Edit", model );
		}

		[HttpPost]
		public ActionResult EditParliament( Models.Parliament.ParliamentEditModel model )
		{
			var service = new ParliamentService();
			service.UpdateParliament( model.ParliamentID.Value, model );
			return RedirectToAction( "ParliamentDetails", new { id = model.ParliamentID } );
		}

		[HttpGet]
		public ActionResult DeleteParliament( int id )
		{
			var service = new ParliamentService();
			var model = service.GetParliamentEditModel( id );
			return View( "Parliament/Delete", model );
		}

		[HttpPost]
		public ActionResult DeleteParliament( int id, string test )
		{
			var service = new ParliamentService();
			service.DeleteParliament( id );
			return RedirectToAction( "ManageParliament" );
		}

		[HttpGet]
		public ActionResult CreateParliament()
		{
			var service = new ParliamentService();
			var model = service.InitializeParliamentEditModel( SessionManager.Current.CurrentParliamentId );
			return View( "Parliament/Create", model );
		}

		// POST: Experts/Create
		[HttpPost]
		public ActionResult CreateParliament( Models.Parliament.ParliamentEditModel model )
		{
			var service = new ParliamentService();
			service.CreateParliament( model );
			return RedirectToAction( "ManageParliament" );
		}

		[HttpGet]
		public ActionResult EditParliamentHouses( int id )
		{
			var service = new ParliamentService();
			var model = service.GetParliamentEditModel( id );
			return View( "Parliament/EditParliamentHouses", model );
		}

		[HttpGet]
		public ActionResult CreateParliamentHouse( int id )
		{
			var service = new ParliamentService();
			var model = service.InitializeEditParliamentHouseModel( id );
			return View( "Parliament/CreateParliamentHouse", model );
		}

		[HttpPost]
		public ActionResult CreateParliamentHouse( Models.Parliament.ParliamentHouseEditModel model )
		{
			var service = new ParliamentService();
			service.CreateNewParliamentHouseModel( model );
			return RedirectToAction( "EditParliamentHouses", new { id = model.ParliamentID } );
		}


		[HttpGet]
		public ActionResult EditParliamentHouse( int ParliamentId, int HouseId )
		{
			var service = new ParliamentService();
			var model = service.InitializeEditParliamentHouseModel( ParliamentId, HouseId );
			return View( "Parliament/EditParliamentHouse", model );
		}

		[HttpPost]
		public ActionResult EditParliamentHouse( Models.Parliament.ParliamentHouseEditModel model )
		{
			var service = new ParliamentService();
			service.UpdateParliamentHouse( model.ParliamentHouseID, model );
			return RedirectToAction( "EditParliamentHouses", new { id = model.ParliamentID } );
		}

		[HttpGet]
		public ActionResult DeleteParliamentHouse( int id )
		{
			var service = new ParliamentService();
			var ParliamentId = service.DeleteParliamentHouse( id );
			return RedirectToAction( "EditParliamentHouses", new { id = ParliamentId } );
		}

		#endregion

		#region InfoBox

		public ActionResult InfoBoxStatus( Models.InfoBox.InfoBoxItemModel model )
		{
			var ibService = new InfoBoxService();

			var returnModel = ibService.GetInfoBox( model.BoxName, model.Reference, model.Partition, model.Type );

			return PartialView( "InfoBoxStatus/_InfoBoxStatus", returnModel );
		}

		public ActionResult InfoBoxStatusUpdate( Models.InfoBox.InfoBoxItemModel model )
		{
			var ibService = new InfoBoxService();

			var returnModel = ibService.UpdateInfoBox( model.BoxName, model.Reference, model.Partition, model.Type, model.Position );

			return PartialView( "InfoBoxStatus/_InfoBoxStatus", returnModel );
		}

		#endregion

		#region News

		[HttpGet]
		public ActionResult GetAllNews()
		{
			if ( SessionManager.Current.CurrentParliamentId == 0 )
				SessionManager.Current.CurrentParliamentId = 1;

			var service = new NewsService();
			var model = service.GetAll( SessionManager.Current.CurrentParliamentId );
			return View( "News/Index", model );
		}
		[HttpGet]
		public ActionResult NewsDetails( int id )
		{
			var service = new NewsService();
			var model = service.Get( id );
			return View( "News/Details", model );
		}

		[HttpGet]
		public ActionResult NewsCreate()
		{
			var service = new NewsService();
			var model = service.InitCreate( SessionManager.Current.CurrentParliamentId );
			return View( "News/Create", model );
		}

		// POST: News/Create
		[HttpPost]
		public ActionResult NewsCreate( Models.News.NewsModel model )
		{
			var service = new NewsService();
			var result = service.Create( model );
			return RedirectToAction( "GetAllNews" );
		}

		// GET: News/Edit/5
		public ActionResult NewsEdit( int id )
		{
			var service = new NewsService();
			var model = service.Get( id );
			return View( "News/Edit", model );
		}

		// POST: News/Edit/5
		[HttpPost]
		public ActionResult NewsEdit( int id, Models.News.NewsModel model )
		{
			var service = new NewsService();
			service.Update( model );
			return RedirectToAction( "GetAllNews" );
		}

		// GET: News/Delete/5
		public ActionResult NewsDelete( int id )
		{
			var service = new NewsService();
			var model = service.Get( id );
			return View( "News/Delete", model );
		}


		// POST: Newss/Delete/5
		[HttpPost]
		public ActionResult NewsDelete( int id, string fake )
		{
			var service = new NewsService();
			service.Delete( id );
			return RedirectToAction( "GetAllNews" );

		}

		#endregion

		#region Parties

		[HttpGet]
		public ActionResult GetAllParties()
		{
			var service = new PartyService();
			var model = service.GetAllParties();
			return View( "Parties/Index", model );
		}
		[HttpGet]
		public ActionResult PartyDetails( int id )
		{
			var service = new PartyService();
			var model = service.GetParty( id );
			return View( "Parties/Details", model );
		}

		[HttpGet]
		public ActionResult PartyCreate()
		{
			return View( "Parties/Create" );
		}

		// POST: Experts/Create
		[HttpPost]
		public ActionResult PartyCreate( PartyModel model )
		{
			var service = new PartyService();
			var result = service.CreateParty( model );
			return RedirectToAction( "GetAllParties", "Admin", null );
		}

		// GET: Experts/Edit/5
		public ActionResult PartyEdit( int id )
		{
			var service = new PartyService();
			var model = service.GetParty( id );
			return View( "Parties/Edit", model );
			//return RedirectToAction( "GetAllParties", "Admin", null );
		}

		// POST: Experts/Edit/5
		[HttpPost]
		public ActionResult PartyEdit( int id, PartyModel model )
		{
			var service = new PartyService();
			service.UpdateParty( model );
			return RedirectToAction( "GetAllParties", "Admin", null );
		}

		// GET: Experts/Delete/5
		public ActionResult PartyDelete( int id )
		{
			var service = new PartyService();
			var model = service.GetParty( id );
			return View( "Parties/Delete", model );
		}


		// POST: Experts/Delete/5
		[HttpPost]
		public ActionResult PartyDelete( int id, PartyModel collection )
		{
			var service = new PartyService();
			service.DeleteParty( id );
			return RedirectToAction( "GetAllParties", "Admin", null );

		}

		#endregion

		#region Users

		public ActionResult GetAllUsers()
		{
			var service = new UserService();
			var searchModel = service.InitializeSearchModel();
			searchModel.PageItemCount = 50;
			var results = service.GetSearchUsers( searchModel );

			var model = new UserSearchPageModel
			{
				SearchModel = searchModel,
				Users = results
			};

			return View( "Users/Index", model );
		}

		public ActionResult SearchUsers( UserSearchModel model )
		{
			var service = new UserService();
			var searchModel = service.InitializeSearchModel( model );
			searchModel.PageItemCount = 50;

			var results = service.GetSearchUsers( searchModel );

			var resultsModel = new UserSearchPageModel
			{
				SearchModel = searchModel,
				Users = results
			};

			return View( "Users/Index", resultsModel );
		}

		public ActionResult UserDetails( string id )
		{
			var service = new UserService();
			var model = service.Get( id );
			return View( "Users/Details", model );
		}

		public ActionResult SetAdmin( string id, bool adminState )
		{
			var service = new UserService();
			service.SetAdmin( id, adminState, HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>() );

			return RedirectToAction( "UserDetails", new { id = id } );
		}

		public ActionResult SetDisabled( string id, bool disabledState )
		{
			var service = new UserService();
			service.SetDisabled( id, disabledState );

			return RedirectToAction( "UserDetails", new { id = id } );
		}

		#endregion
	}
}