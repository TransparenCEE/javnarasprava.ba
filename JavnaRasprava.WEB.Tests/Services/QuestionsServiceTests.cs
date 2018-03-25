using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.DomainModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class QuestionsServiceTests
	{
		[TestMethod]
		public void GetQuestionsModel_Seed()
		{
			var law = Helpers.GetTestLaw();
			var user = Helpers.CreateNewUser();
			var numberOfRepresentatives = Helpers.GetTotalNumberOfRepresentativesForparliament( law.ParliamentID );


			try
			{
				var response = new QuestionsService().GetQuestionsModel( law.LawID, user.Id );

				Assert.AreEqual( 2, response.SuggestedRepresentatives.Count );
				Assert.AreEqual( 2, response.Questions.Count );
				Assert.AreEqual( numberOfRepresentatives, response.SuggestedRepresentatives.Count + response.OtherRepresentatives.Count );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void GetQuestionsForLaw_Seed()
		{
			var law = Helpers.GetTestLaw();
			var response = new QuestionsService().GetQuestionsForLaw( law.LawID, "" );

			Assert.AreEqual( 2, response.Questions.Count );
			Assert.IsTrue( response.Questions[0].IsPredefined );
			Assert.AreEqual( 2, response.Questions[0].AskedCount );
			Assert.AreEqual( 2, response.Questions[0].Representatives.Count );

			Assert.AreEqual( 0, response.Questions[1].AskedCount );
			Assert.AreEqual( 0, response.Questions[1].Representatives.Count );
		}

		[TestMethod]
		public void GetQuestionsForLaw_ChainedMethods()
		{
			ApplicationDbContext context = null;

			var law = Helpers.CreateLaw( 1, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var user1 = Helpers.CreateNewUser();
			var user2 = Helpers.CreateNewUser();

			try
			{
				var service = new QuestionsService();

				var askmodel1 = service.GetQuestionsModel( law.LawID, user1.Id );
				askmodel1.SuggestedRepresentatives[0].IsSelected=true;
				askmodel1.SuggestedRepresentatives[1].IsSelected=true;
				askmodel1.Questions[0].IsSelected = true;

				service.PostQuestion( askmodel1, user1.Id );


				var askmodel2 = service.GetQuestionsModel( law.LawID, user2.Id );
				askmodel2.SuggestedRepresentatives[0].IsSelected=true;
				askmodel2.SuggestedRepresentatives[2].IsSelected=true;
				askmodel2.Questions[0].IsSelected = true;
				askmodel2.Text="Test Custom Question";

				service.PostQuestion( askmodel2, user2.Id );

				context = ApplicationDbContext.Create();

				context.Answers.AddRange( new List<Answer>
				{
					new Answer {
						QuestionID = askmodel1.Questions[0].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[0].ID, Text = "Test answer", AnsweredTimeUtc = DateTime.UtcNow 
					}
				} );
				context.SaveChanges();

				var response = service.GetQuestionsForLaw( law.LawID, user1.Id );

				Assert.IsNotNull( response.Law, "Law does not exist" );
				Assert.AreEqual( 3, response.Questions.Count, "Questions.Count" );
				Assert.IsTrue( response.Questions[0].IsPredefined, "Predefined question not marked" );
				Assert.IsFalse( response.Questions[2].IsPredefined, "Custom question marked as predefined" );

				Assert.AreEqual( 4, response.Questions[0].AskedCount, "First Question Asked times" );
				Assert.AreEqual( 1, response.Questions[0].AnswersCount, "First Question answered times" );

				Assert.AreEqual( 2, response.Questions[2].AskedCount, "Third question Asked times" );
				Assert.AreEqual( 0, response.Questions[2].AnswersCount, "Third Question answered times" );




				Assert.AreEqual( 3, response.Questions[0].Representatives.Count, "Reps on first question" );
				Assert.AreEqual( 2, response.Questions[2].Representatives.Count, "Reps on custom question" );

				Assert.AreEqual( 2, response.Questions[0].Representatives[0].AskedCount );
				Assert.AreEqual( 1, response.Questions[0].Representatives[1].AskedCount );

				Assert.IsTrue( response.Questions[0].Representatives[0].Answered );
				Assert.AreEqual( "Test answer", response.Questions[0].Representatives[0].Answer.Text );




			}
			finally
			{
				if ( context !=null )
					context.Dispose();

				Helpers.DeleteLaw( law );
				Helpers.DeleteUser( user1 );
				Helpers.DeleteUser( user2 );
			}


		}

		[TestMethod]
		public void GetLatestAnswers_ChainMethods()
		{
			ApplicationDbContext context = null;
			var law = Helpers.CreateLaw( 1, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var user1 = Helpers.CreateNewUser();

			try
			{
				var service = new QuestionsService();

				var askmodel1 = service.GetQuestionsModel( law.LawID, user1.Id );
				askmodel1.SuggestedRepresentatives[0].IsSelected=true;
				askmodel1.SuggestedRepresentatives[1].IsSelected=true;
				askmodel1.SuggestedRepresentatives[2].IsSelected=true;
				askmodel1.Questions[0].IsSelected = true;
				askmodel1.Questions[1].IsSelected = true;

				service.PostQuestion( askmodel1, user1.Id );




				context = ApplicationDbContext.Create();

				context.Answers.AddRange( new List<Answer>
				{
					new Answer {
						QuestionID = askmodel1.Questions[0].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[0].ID, Text = "Test answer 00", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(1)
					},

					new Answer {
						QuestionID = askmodel1.Questions[0].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[1].ID, Text = "Test answer 01", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(2)
					},
					new Answer {
						QuestionID = askmodel1.Questions[0].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[2].ID, Text = "Test answer 02", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(3)
					},
					new Answer {
						QuestionID = askmodel1.Questions[1].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[0].ID, Text = "Test answer 10", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(4)
					},
					new Answer {
						QuestionID = askmodel1.Questions[1].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[1].ID, Text = "Test answer 11", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(5)
					},
					new Answer {
						QuestionID = askmodel1.Questions[1].ID, RepresentativeID = askmodel1.SuggestedRepresentatives[2].ID, Text = "Test answer 12", AnsweredTimeUtc = DateTime.UtcNow.AddMinutes(6)
					},

				} );
				context.SaveChanges();

				var response = service.GetLatestAnswersForLaw( law.LawID, user1.Id, 5 );

				Assert.AreEqual( law.LawID, response.LawID, "Law ID" );
				Assert.AreEqual( 5, response.Count, "Total question count" );
				Assert.IsNotNull( response.Answers, "Answers returned" );
				Assert.IsNotNull( response.Answers[0].Question, "Question populated" );
				Assert.IsNotNull( response.Answers[0].Representative, "Representative populated" );

				var selectedAnswer = response.Answers.Where( x => x.Question.Id ==askmodel1.Questions[0].ID && x.Representative.ID == askmodel1.SuggestedRepresentatives[2].ID ).First();

				Assert.AreEqual( "Test answer 02", selectedAnswer.Text );






			}
			finally
			{
				if ( context !=null )
					context.Dispose();

				Helpers.DeleteLaw( law );
				Helpers.DeleteUser( user1 );
			}
		}

		[TestMethod]
		public void PostAnswer_RepresentativeAnswer_Count1()
		{
			QuestionsService service = new QuestionsService();

			Law law = null;
			ApplicationUser user = null;

			try
			{
				user = Helpers.CreateNewUser();

				law = Helpers.CreateLaw( 1, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" }, user.Id );

				var questionID = law.Questions.First().QuestionID;
				var repID = law.LawRepresentativeAssociations.First().RepresentativeID;

				service.PostAnswer( questionID, repID, "Test Answer" );

				var response = service.GetQuestionsForLaw( law.LawID, user.Id );

				var actual = response.Questions
					.Where( x => x.Id == questionID )
					.First()
					.Representatives
					.Where( x => x.ID == repID )
					.First()
					.Answer.Text;

				Assert.AreEqual( "Test Answer", actual );
			}
			finally
			{
				Helpers.DeleteLaw( law );
				Helpers.DeleteUser( user );
			}
		}

		[TestMethod]
		public void QuestionAndAnswerLikes_ChainMethods()
		{
			QuestionsService service = new QuestionsService();

			Law law = null;
			ApplicationUser user1 = null;
			ApplicationUser user2 = null;
			ApplicationUser user3 = null;

			try
			{
				user1 = Helpers.CreateNewUser();
				user2 = Helpers.CreateNewUser();
				user3 = Helpers.CreateNewUser();

				law = Helpers.CreateLaw( 1, new List<string> { "AmirFazlic", "AsimSarajlic" }, new List<string> { "First Question", "Second Question" }, user1.Id );

				var questionID = law.Questions.First().QuestionID;
				var repID = law.LawRepresentativeAssociations.First().RepresentativeID;
				var nextRepID = law.LawRepresentativeAssociations.Where( x => x.RepresentativeID != repID ).First().RepresentativeID;
				service.PostAnswer( questionID, repID, "Test Answer" );
				service.PostAnswer( questionID, nextRepID, "Test Answer" );

				var modelPriorToLikes = service.GetQuestionsForLaw( law.LawID, user2.Id );
				var question = modelPriorToLikes.Questions.Where( x => x.Id == questionID ).First();
				var answer = question.Representatives.Where( x => x.ID == repID ).First().Answer;
				Assert.AreEqual( 0, question.LikesCount, "New Question no one liked yet" );
				Assert.IsFalse( question.UserLiked.HasValue, "New question user did not like yet" );
				Assert.AreEqual( 0, answer.LikesCount, "New answer no one liked yet" );
				Assert.IsFalse( answer.UserLiked.HasValue, "New answer user did not like yet" );


				service.LikeQuestion( question.Id, user1.Id, true );
				service.LikeQuestion( question.Id, user2.Id, true );

				service.LikeAnswer( answer.ID, user1.Id, false );
				service.LikeAnswer( answer.ID, user2.Id, true );
				service.LikeAnswer( answer.ID, user3.Id, false );

				var modelForUser2 = service.GetQuestionsForLaw( law.LawID, user2.Id );
				question = modelForUser2.Questions.Where( x => x.Id == questionID ).First();
				var nextQuestion = modelForUser2.Questions.Where( x => x.Id != questionID ).First();
				answer = question.Representatives.Where( x => x.ID == repID ).First().Answer;
				var nextAnswer = question.Representatives.Where( x => x.ID == nextRepID ).First().Answer;
				Assert.AreEqual( 2, question.LikesCount, "2 users liked" );
				Assert.AreEqual( 0, question.DislikesCount, "0 users disliked" );
				Assert.IsTrue( question.UserLiked.HasValue, "User2 liked" );

				Assert.AreEqual( 0, nextQuestion.LikesCount, "0 users liked next question" );
				Assert.AreEqual( 0, nextQuestion.DislikesCount, "0 users disliked next question" );
				Assert.IsFalse( nextQuestion.UserLiked.HasValue, "User2 did not like next question" );
				
				Assert.AreEqual( 1, answer.LikesCount, "1 user liked answer" );
				Assert.AreEqual( 2, answer.DislikesCount, "2 users disliked answer" );
				Assert.IsTrue( answer.UserLiked.HasValue, "user2 liked answer" );
				Assert.IsTrue( answer.UserLiked.Value, "user2 liked answer" );

				Assert.AreEqual( 0, nextAnswer.LikesCount, "0 user liked next answer" );
				Assert.AreEqual( 0, nextAnswer.DislikesCount, "0 users disliked next answer" );
				Assert.IsFalse( nextAnswer.UserLiked.HasValue, "user2 did not like next answer" );

				var modelForUser3 = service.GetQuestionsForLaw( law.LawID, user3.Id );
				question = modelForUser3.Questions.Where( x => x.Id == questionID ).First();
				answer = question.Representatives.Where( x => x.ID == repID ).First().Answer;

				Assert.IsFalse( question.UserLiked.HasValue, "user3 did not like the question" );
				Assert.IsFalse( answer.UserLiked.Value, "user3 disliked answer" );
			}
			finally
			{
				Helpers.DeleteLaw( law );
				Helpers.DeleteUser( user1 );
				Helpers.DeleteUser( user2 );
				Helpers.DeleteUser( user3 );
			}
		}

		[TestMethod]
		public void PostQuestion_NoRepresentative_NoQuestions()
		{
			int parliamentID = 1;

			var law1 = Helpers.CreateLaw( parliamentID, new List<string> { "AmirFazlic", "AsimSarajlic", "SalkoSokolovic" }, new List<string> { "First Question", "Second Question" } );
			var user = Helpers.CreateNewUser();

			try
			{
				var service = new QuestionsService();

				var model = service.GetQuestionsModel(law1.LawID, user.Id);
				model.Text = "Some Text";
				service.PostQuestion( model, user.Id );

				var response = service.GetQuestionsForLaw( law1.LawID, user.Id );

				Assert.AreEqual( 2, response.Questions.Count );
			}
			finally
			{
				Helpers.DeleteUser( user );
			}
		}
	}
}
