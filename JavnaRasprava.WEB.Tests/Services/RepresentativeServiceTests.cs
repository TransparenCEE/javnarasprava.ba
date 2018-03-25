using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.Representative;
using JavnaRasprava.WEB.DomainModels;

namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class RepresentativeServiceTests
	{
		[TestMethod]
		public void GetRepresentative_First_Returned()
		{
			int firstId = 0;
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				firstId = context.Representatives.First().RepresentativeID;

				var response = new RepresentativeService().GetRepresentative( firstId );

				Assert.IsNotNull( response );

			}
		}

		[TestMethod]
		public void GetAllRepresentativesForParliament_First_Returned()
		{
			int stateId = 0;
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				stateId = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE ).First().ParliamentID;

				var response = new RepresentativeService().GetAllRepresentativesForParliament( stateId );

				Assert.IsNotNull( response );
				Assert.AreEqual( 2, response.ParliamentHouses.Count() );

			}
		}

		[TestMethod]
		public void GetRepresentative_OnSeedFirstSDA_QuestionDetailsReturned()
		{

			var seed = Helpers.GetTestLawExtended();
			var sdaRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDA" )
				.FirstOrDefault();

			var sdpRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDP" )
				.FirstOrDefault();


			var response = new RepresentativeService().GetRepresentative( sdaRep.RepresentativeID );

			Assert.AreEqual( 1, response.TotalQuestions, "Total Questions" );
			Assert.AreEqual( 1, response.TotalAnswers, "Total Answers" );
			Assert.AreEqual( 1, response.Laws.Count, "Total Laws" );
			Assert.AreEqual( 1, response.Laws[0].Questions.Count, "Total Question objects" );
			Assert.IsNotNull( response.Laws[0].Questions[0].Answer, "Answer exists" );



			
		}

		[TestMethod]
		public void GetRepresentative_OnSeedFirstSDP_QuestionDetailsReturned()
		{

			var seed = Helpers.GetTestLawExtended();
			var sdaRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDA" )
				.FirstOrDefault();

			var sdpRep = seed.LawRepresentativeAssociations
				.Where( x => x.Representative.Party.Name == "SDP" )
				.FirstOrDefault();


			var response = new RepresentativeService().GetRepresentative( sdpRep.RepresentativeID );

			Assert.AreEqual( 1, response.TotalQuestions, "Total Questions" );
			Assert.AreEqual( 0, response.TotalAnswers, "Total Answers" );
			Assert.AreEqual( 1, response.Laws.Count, "Total Laws" );
			Assert.AreEqual( 1, response.Laws[0].Questions.Count, "Total Question objects" );
			Assert.IsNull( response.Laws[0].Questions[0].Answer, "Answer exists" );




		}
	}
}
