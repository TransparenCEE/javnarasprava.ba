using JavnaRasprava.WEB.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class PetitionServiceTests
	{
		[ClassInitialize]
		public static void Initialize( TestContext context )
		{
			AutoMapper.Mapper.CreateMap<Models.PetitionModel, DomainModels.Petition>();
			AutoMapper.Mapper.CreateMap<DomainModels.Petition, Models.PetitionModel>();
		}

		[TestMethod]
		public void GetPetition_NewVerified_NonVerified()
		{
			var user = Helpers.CreateNewUser();
			Models.PetitionModel model = null;
			Models.PetitionModel savedModel = null;
			int petitionID = 0;

			try
			{
				var service = new PetitionService();
				model = new Models.PetitionModel
				{
					Title = "Some Title",
					Description = "Some Description",
					SubmitterName = "Some user",
					SubmitterUserID = user.Id,
					TargetInstitution = "Test institution",
					Verified = false

				};

				petitionID = service.CreateNewPetition( model );

				Assert.AreNotEqual( 0, petitionID, "ID not created" );

				savedModel = service.GetPetition( petitionID, user.Id );

				Assert.IsFalse( savedModel.Verified, "Petition should be non verified" );

			}
			finally
			{
				Helpers.DeleteUser( user );

				if ( model != null )
					new PetitionService().DeletePetition( petitionID );
			}
		}

		[TestMethod]
		public void DeletePetition_New_Deleted()
		{
			var user = Helpers.CreateNewUser();
			Models.PetitionModel model = null;
			Models.PetitionModel savedModel = null;

			try
			{
				var service = new PetitionService();
				model = new Models.PetitionModel
				{
					Title = "Some Title",
					Description = "Some Description",
					SubmitterName = "Some user",
					SubmitterUserID = user.Id,
					TargetInstitution = "Test institution",
					Verified = false

				};

				var id = service.CreateNewPetition( model );
				Assert.AreNotEqual( 0, id, "ID not created" );

				savedModel = service.GetPetition( id, user.Id );
				Assert.IsNotNull( savedModel );

				service.DeletePetition( id );
				savedModel = service.GetPetition( id, user.Id );
				Assert.IsNull( savedModel );

			}
			finally
			{
				Helpers.DeleteUser( user );

				if ( model != null )
					new PetitionService().DeletePetition( model.PetitionID );
			}
		}

		[TestMethod]
		public void UpdatePetition_New_Updated()
		{
			var user = Helpers.CreateNewUser();
			Models.PetitionModel model = null;
			Models.PetitionModel savedModel = null;
			int petitionID = 0;

			try
			{
				var service = new PetitionService();
				model = new Models.PetitionModel
				{
					Title = "Some Title",
					Description = "Some Description",
					SubmitterName = "Some user",
					SubmitterUserID = user.Id,
					TargetInstitution = "Test institution",
					Verified = false

				};

				petitionID = service.CreateNewPetition( model );
				Assert.AreNotEqual( 0, petitionID, "ID not created" );

				savedModel = service.GetPetition( petitionID, user.Id );
				Assert.IsNotNull( savedModel );

				string newTitle = "Some new title";
				savedModel.Title = newTitle;
				var changedModel = service.UpdatePetition( savedModel, user.Id );

				Assert.IsNotNull( savedModel );
				Assert.AreEqual( newTitle, changedModel.Title, "Title Changed" );
				Assert.AreEqual( changedModel.Description, model.Description, "Description did not change" );

			}
			finally
			{
				Helpers.DeleteUser( user );

				if ( model != null )
					new PetitionService().DeletePetition( petitionID );
			}
		}

		[TestMethod]
		public void SignPetition_OneUser_OneUSerSigned()
		{
			var creator = Helpers.CreateNewUser();
			var signer = Helpers.CreateNewUser();

			var pettition = Helpers.CreateNewPetition( creator );

			try
			{
				var service= new PetitionService();

				service.Sign( pettition, signer.Id );

				var actualPetition = service.GetPetition( pettition );

				Assert.AreEqual( 1, actualPetition.Signatures );
			}
			finally
			{
				Helpers.DeleteUser( signer );
				Helpers.DeleteUser( creator );
			}

		}

		[TestMethod]
		public void SearchPetition_Active_OnlyActive()
		{
			var user = Helpers.CreateNewUser();

			var activePetitionId = Helpers.CreateNewPetition( user, "Active Petition",
				startTime: DateTime.UtcNow.AddDays( -1 ),
				endTime: DateTime.UtcNow.AddDays( 1 ) );

			var inactivePetitionId = Helpers.CreateNewPetition( user, "Inactive Petition",
				startTime: DateTime.UtcNow.AddDays( 5 ),
				endTime: DateTime.UtcNow.AddDays( 10 ) );

			try
			{
				var service = new PetitionService();
				var actual = service.Search( null, null, null, true );

				Assert.AreEqual( 1, actual.Count );
				Assert.AreEqual( "Active Petition", actual[0].Title );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void SearchPetition_ByKeyword_FoundCorrect()
		{
			var submitter = Helpers.CreateNewUser();

			var petition1 = Helpers.CreateNewPetition( submitter,
				title: "First Found", startTime: DateTime.UtcNow.AddDays( -3 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			var petition2 =  Helpers.CreateNewPetition( submitter,
				title: "Second Found", startTime: DateTime.UtcNow.AddDays( -3 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			var petition3 =  Helpers.CreateNewPetition( submitter,
				title: "First hidden", startTime: DateTime.UtcNow.AddDays( -3 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			try
			{
				var results = new PetitionService().Search( null, "Found", null );

				Assert.AreEqual( 2, results.Count );
				Assert.AreEqual( "Second Found", results[1].Title );
			}
			finally
			{
				Helpers.DeleteUser( submitter );
			}

		}

		[TestMethod]
		public void SearchPetition_Successful_OnlySuccessfull()
		{
			var user = Helpers.CreateNewUser();
			var signingUsers = new List<string>();

			var successfulPetition = Helpers.CreateNewPetition( user, "Successful Petition",
				startTime: DateTime.UtcNow.AddDays( -1 ),
				endTime: DateTime.UtcNow.AddDays( 1 ),
				targetCount: 1 );

			signingUsers.AddRange( Helpers.SignPetition( successfulPetition, 1 ) );

			var unSuccesfulPetition = Helpers.CreateNewPetition( user, "Unsuccessful Petition",
				startTime: DateTime.UtcNow.AddDays( -2 ),
				endTime: DateTime.UtcNow.AddDays( -1 ),
				targetCount: 5 );

			signingUsers.AddRange( Helpers.SignPetition( unSuccesfulPetition, 2 ) );

			try
			{
				var service = new PetitionService();
				var actual = service.Search( null, null, null );

				Assert.AreEqual( 1, actual.Count );
				Assert.AreEqual( "Successful Petition", actual[0].Title );
			}
			finally
			{
				Helpers.DeleteUsers( signingUsers );
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void SearchPetition_Paging()
		{
			var submitter = Helpers.CreateNewUser();

			var petition1 = Helpers.CreateNewPetition( submitter,
				title: "First Found", startTime: DateTime.UtcNow.AddDays( -3 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			var petition2 =  Helpers.CreateNewPetition( submitter,
				title: "Second Found", startTime: DateTime.UtcNow.AddDays( -2 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			var petition3 =  Helpers.CreateNewPetition( submitter,
				title: "Third Found", startTime: DateTime.UtcNow.AddDays( -1 ), endTime: DateTime.UtcNow.AddDays( 1 ) );

			try
			{
				var service = new PetitionService();
				var results = service.Search( null, "Found", pageNumber: 2, pageItemCount: 1 );


				Assert.AreEqual( 1, results.Count );
				Assert.AreEqual( "Second Found", results[0].Title );
			}
			finally
			{
				Helpers.DeleteUser( submitter );
			}
		}

		[TestMethod]
		public void VerifyPetition_NewPetition_Verified()
		{
			var user = Helpers.CreateNewUser();
			var service = new PetitionService();
			var petitionId = service.CreateNewPetition( new Models.PetitionModel
			{
				Title = "Some Title",
				Description = "Some description",
				SubmitterName = "I am Submitter",
				TargetInstitution = "Some institution",
			} );

			try
			{
				service.VerifyPetition( petitionId, user.Id );
			}
			finally
			{
				service.DeletePetition( petitionId );
				Helpers.DeleteUser( user );
			}


		}
	}
}
