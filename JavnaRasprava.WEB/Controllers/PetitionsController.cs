using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EEP.Utility;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Models;
using Microsoft.AspNet.Identity;

namespace JavnaRasprava.WEB.Controllers
{
    [OutputCache( Duration = 0 )]
    public class PetitionsController : BaseController
	{
        // GET: Petitions
        public ActionResult Index()
        {
            var service = new PetitionService();
			var parliamentService = new ParliamentService();
            PetitionHomeModel model = new PetitionHomeModel
            {
                LastSuccessfulPetitions = service.GatLatestPetitions( AppConfig.GetInt( "Petitions.Index.LastSuccessfulPetitions", 5 ) ),
                TopActivePetitions = service.GetTopActivePetitions( AppConfig.GetInt( "Petitions.Index.TopActivePetitions", 5 ) ),
				Parliaments = parliamentService.GetParliaments()
            };
            return View( model );
        }

        [Authorize]
        public ActionResult Create()
        {
            var service = new PetitionService();
            var model = service.InitializePetitionModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create( PetitionModel petition )
        {
            var service = new PetitionService();
            petition.SubmitterUserID = User.Identity.GetUserId();
            petition.SubmitterName = User.Identity.Name;

            var petitionId = service.CreateNewPetition( petition );

            return RedirectToAction( "PetitionCreated", "Petitions");

        }

        public ActionResult Details( int? petitionId = null )
        {
			if ( petitionId == null )
				return HttpNotFound();

            var service = new PetitionService();
            var model = service.GetPetition( petitionId.Value, User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null );

            if (model == null)
                return HttpNotFound();

            return View( model );
        }

        [HttpGet]
        [OutputCache( Duration = 0 )]
		public ActionResult FilterPetitions( int? ParliamentID = null, int page = 1, string sort = "Title", string sortDir = "Ascending", string title = "" )
        {
            var service = new PetitionService();
            var model = service.Search( null, title, null, pageItemCount: 1000, parliamentId: ParliamentID );

            return PartialView( "_SearchResult", model );
        }

        public ActionResult PetitionCreated()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [OutputCache( Duration = 0 )]
        public ActionResult SignPetition( int petitionId )
        {
            var service = new PetitionService();
            service.Sign( petitionId, User.Identity.GetUserId() );

            var model = service.GetPetition( petitionId, User.Identity.GetUserId() );

            return PartialView( "_Sign", model );
        }


        [Authorize]
        [HttpPost]
        [OutputCache( Duration = 0 )]
        public ActionResult VerifyPetition( int petitionId )
        {
            var service = new PetitionService();

            service.VerifyPetition( petitionId, User.Identity.GetUserId() );
            var model = service.GetPetition( petitionId, User.Identity.GetUserId() );

            return PartialView( "_Sign", model );
        }

		[HttpGet]
		[OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 60)]
		public ActionResult PetitionManual()
		{
			return View();
		}

	}
}