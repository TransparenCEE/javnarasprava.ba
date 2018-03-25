using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class LawServiceTests
	{
		[TestMethod]
		public void GetLawModel_SeedLaw_VariousAsserts()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users.FirstOrDefault();
				var law = context.Laws.FirstOrDefault();

				var response = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );

				Assert.IsNotNull( response, "Response is null" );
				Assert.AreEqual( 2, response.Sections.Count, "Number of sections is invalid" );
				Assert.AreEqual( 1, response.Law.ExpertComments.Count, "Number of expert comments is wrong" );
				Assert.AreEqual( "Enes", response.Law.ExpertComments.FirstOrDefault().Expert.FirstName, "Wrong expert Name" );
				Assert.AreEqual( 2, response.Law.LawRepresentativeAssociations.Count, "Wrong number of representatives" );
				Assert.IsNotNull( response.Law.LawRepresentativeAssociations.First().Representative, "No representative object" );
				Assert.AreEqual( 2, response.CommentsCount, "Wrong comment Count" );
				Assert.AreEqual( 1, response.ExpertCommentsCount, "Wrong number of expert comments" );
				Assert.IsNotNull( response.Law.Procedure );

			}
		}

		[TestMethod]
		public void GetLawCustomVotesList_Seed_VariousAsserts()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var law = context.Laws.FirstOrDefault();
				var response = new LawService().GetLawCustomVotesList( law.LawID );

				Assert.AreEqual( 4, response.LawCustomVotes.Count, "Insufficient responses" );
				Assert.AreEqual( 2, response.LawCustomVotes.Count( x => x.LawCustomVoteID > 0 ), "Insufficient custom responses" );
			}
		}

		[TestMethod]
		public void GetLawSectionCustomVotesList_Seed_VariousAsserts()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Include( x => x.LawSections )
					.FirstOrDefault();
				var response = new LawService().getLawSectionCustomVotesList( law.LawID, law.LawSections.First().LawSectionID );

				Assert.AreEqual( 4, response.LawSectionCustomVotes.Count, "Insufficient responses" );
				Assert.AreEqual( 2, response.LawSectionCustomVotes.Count( x => x.LawSectionCustomVoteID > 0 ), "Insufficient custom responses" );
			}
		}


		[TestMethod]
		public void GetLatestLaws_3Asked_3Returned()
		{
			int parliamentID = 1;
			var law1 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law2 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law3 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law4 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law5 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );

			try
			{
				var response = new LawService().GetInfoBoxLaws(  parliamentID );

				Assert.AreEqual( 4, response.Count );
			}
			finally
			{
				Helpers.DeleteLaw( law1 );
				Helpers.DeleteLaw( law2 );
				Helpers.DeleteLaw( law3 );
				Helpers.DeleteLaw( law4 );
				Helpers.DeleteLaw( law5 );
			}
		}


		[TestMethod]
		public void GetNextinVote_3Asked_3Returned()
		{
			int parliamentID = 1;

			var law1 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law2 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law3 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law4 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law5 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );

			try
			{
				var response = new LawService().GetNextLawsInVote( 3, parliamentID );

				Assert.AreEqual( 3, response.Count );
			}
			finally
			{
				Helpers.DeleteLaw( law1 );
				Helpers.DeleteLaw( law2 );
				Helpers.DeleteLaw( law3 );
				Helpers.DeleteLaw( law4 );
				Helpers.DeleteLaw( law5 );
			}
		}

		[TestMethod]
		public void GetMostActive_3Asked_1Returned()
		{
			int parliamentID = 1;

			var commentUser = Helpers.CreateNewUser();

			var law1 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law2 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law3 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law4 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var law5 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );

			try
			{
				var commentService = new CommentsService();
				commentService.MakeComment( law1.LawID, commentUser.Id, "Some Comment" );
				commentService.MakeComment( law1.LawID, commentUser.Id, "Yet another comment" );

				var response = new LawService().GetMostActive( 3, parliamentID );

				Assert.AreEqual( 1, response.Count );
			}
			finally
			{
				Helpers.DeleteLaw( law1 );
				Helpers.DeleteLaw( law2 );
				Helpers.DeleteLaw( law3 );
				Helpers.DeleteLaw( law4 );
				Helpers.DeleteLaw( law5 );

				Helpers.DeleteUser( commentUser );
			}
		}

		

		


		


		[TestMethod]
		public void GetLawCommunicaitonSummary_Seed_Various()
		{
			var seed = Helpers.GetTestLawExtended();
			var sdaRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDA" )
				.FirstOrDefault();

			var sdpRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDP" )
				.FirstOrDefault();

			var actual = new LawService().GetLawCommunicaitonSummary( seed.LawID );


			Assert.AreEqual( seed.Questions.Where( x => x.Text.Contains( "lorem" ) ).FirstOrDefault().QuestionID, actual.MostFrequentQuestionID, "Wrong Question" );

			Assert.AreEqual( sdaRep.RepresentativeID, actual.BestRepresentativeID, "Wrong Best" );

			Assert.AreEqual( sdpRep.RepresentativeID, actual.WorstRepresentativeID, "Wrong Worst" );
		}

		#region == Voting tests  ==

		#region == Law ==

		[TestMethod]
		public void VoteLaw_StandardVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				//new LawService().VoteLaw( law.LawID, user.Id, -2, null );

				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );


				Assert.AreEqual( lawModelOld.VotesDown + 1, lawModelNew.VotesDown, "Vote not counted" );
				Assert.AreEqual( true, lawModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void VoteLaw_ExistingCustomVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var customVotes = new LawService().GetLawCustomVotesList( law.LawID );
				var customVote = customVotes.LawCustomVotes.Where( x => x.LawCustomVoteID > 0 ).First();
				//new LawService().VoteLaw( law.LawID, user.Id, customVote.LawCustomVoteID, null );

				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );

				int votesUpModifier = customVote.Vote.Value ? 1 : 0;
				int votesDownModifier = customVote.Vote.Value ? 0 : 1;

				Assert.AreEqual( lawModelOld.VotesDown + votesDownModifier, lawModelNew.VotesDown, "Vote Down not counted" );
				Assert.AreEqual( lawModelOld.VotesUp + votesUpModifier, lawModelNew.VotesUp, "Vote Down not counted" );
				Assert.AreEqual( true, lawModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void VoteLaw_NewCustomVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var customVotes = new LawService().GetLawCustomVotesList( law.LawID );
				//new LawService().VoteLaw( law.LawID, user.Id, -1, null );

				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );

				Assert.AreEqual( lawModelOld.VotesDown, lawModelNew.VotesDown, "There Should be no changes in the vote" );
				Assert.AreEqual( lawModelOld.VotesUp, lawModelNew.VotesDown, "There Should be no changes in the vote" );
				Assert.AreEqual( true, lawModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		#endregion

		#region == Section ==

		[TestMethod]
		public void VoteLawSection_StandardVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelOld = lawModelOld.Sections.First();
				//new LawService().VoteLawSection( lawSectionModelOld.LawSection.LawSectionID, user.Id, -2, null );

				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelNew = lawModelNew.Sections.Where( x => x.LawSection.LawSectionID == lawSectionModelOld.LawSection.LawSectionID ).First();

				Assert.AreEqual( lawSectionModelOld.VotesDown + 1, lawSectionModelNew.VotesDown, "Vote not counted" );
				Assert.AreEqual( true, lawSectionModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawSectionModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void VoteLawSection_ExistingCustomVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelOld = lawModelOld.Sections.First();

				var customVotes = new LawService().getLawSectionCustomVotesList( lawSectionModelOld.LawSection.LawID, lawSectionModelOld.LawSection.LawSectionID );
				var customVote = customVotes.LawSectionCustomVotes.Where( x => x.LawSectionCustomVoteID > 0 ).First();
				//new LawService().VoteLawSection( law.LawID, user.Id, customVote.LawSectionCustomVoteID, null );

				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelNew = lawModelNew.Sections.Where( x => x.LawSection.LawSectionID == lawSectionModelOld.LawSection.LawSectionID ).First();

				int votesUpModifier = customVote.Vote.Value ? 1 : 0;
				int votesDownModifier = customVote.Vote.Value ? 0 : 1;

				Assert.AreEqual( lawSectionModelOld.VotesDown + votesDownModifier, lawSectionModelNew.VotesDown, "Vote Down not counted" );
				Assert.AreEqual( lawSectionModelOld.VotesUp + votesUpModifier, lawSectionModelNew.VotesUp, "Vote Down not counted" );
				Assert.AreEqual( true, lawSectionModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawSectionModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void VoteLawsection_NewCustomVote_Voted()
		{
			var user = Helpers.CreateNewUser();
			var law = Helpers.GetTestLaw();

			try
			{
				var lawModelOld = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelOld = lawModelOld.Sections.First();

				//new LawService().VoteLawSection( lawSectionModelOld.LawSection.LawSectionID, user.Id, -1, null );


				var lawModelNew = new LawService().GetLawModel( law.LawID, user.Id, CommentOrder.Chronological );
				var lawSectionModelNew = lawModelNew.Sections.Where( x => x.LawSection.LawSectionID == lawSectionModelOld.LawSection.LawSectionID ).First();


				Assert.AreEqual( lawSectionModelOld.VotesDown, lawSectionModelNew.VotesDown, "There Should be no changes in the vote" );
				Assert.AreEqual( lawSectionModelOld.VotesUp, lawSectionModelNew.VotesDown, "There Should be no changes in the vote" );
				Assert.AreEqual( true, lawSectionModelNew.UserVoted, "User did vote" );
				Assert.AreEqual( false, lawSectionModelOld.UserVoted, "User did not have previous vote" );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		#endregion



		#endregion
	}
}
