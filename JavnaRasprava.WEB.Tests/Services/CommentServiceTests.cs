using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;


namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class CommentServiceTests
	{
		[TestMethod]
		public void GetCommentsForLaw_Seed_VariousAsserts()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users.FirstOrDefault();
				var law = context.Laws.FirstOrDefault();

				var response = new CommentsService().GetCommentsForLaw( law.LawID, user.Id, CommentOrder.Chronological );

				Assert.AreEqual( 2, response.Comments.Count, "Insufficient number of comments" );
				Assert.IsNotNull( response.Comments.First().Comment.ApplicationUser, "User not present" );
				Assert.AreEqual( 2, response.Comments.Where( x => x.Comment.Text.Contains( "user1" ) ).First().VotesUp, "Wrong number of votes" );
				Assert.AreEqual( 50, response.Comments.Where( x => x.Comment.Text.Contains( "user2" ) ).First().VotesUpPercentage, 0.1, "Percentages are wrong" );
			}
		}

		[TestMethod]
		public void MakeComment_OnSeedLaw_CommentMade()
		{
			var commmentText = "Test comment";
			var law = Helpers.GetTestLaw();
			var user = Helpers.CreateNewUser();

			JavnaRasprava.WEB.DomainModels.ApplicationDbContext context = null;
			try
			{
				context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create();
				
				new CommentsService().MakeComment( law.LawID, user.Id, commmentText );

				var response = new CommentsService().GetCommentsForLaw( law.LawID, user.Id, CommentOrder.Chronological );
				var comment = response.Comments.Where( x => x.Comment.ApplicationUserID == user.Id && x.Comment.Text == commmentText ).FirstOrDefault();

				Assert.IsNotNull( comment, "Comment not made" );
				Assert.AreEqual( 0, comment.VotesDown, "New Comment, no votes down yet" );
				Assert.AreEqual( 0, comment.VotesUp, "New Comment, no votes up yet" );
				Assert.AreEqual( false, comment.UserVoted, "New Comment, no votes yet" );
			}
			finally
			{
				if (context != null)
					context.Dispose();

				Helpers.DeleteUser( user );
			}
		}
		[TestMethod]
		public void Votecomment_OnSeedLaw_Voted()
		{
			var commmentText = "Test comment";
			var law = Helpers.GetTestLaw();
			var user = Helpers.CreateNewUser();

			JavnaRasprava.WEB.DomainModels.ApplicationDbContext context = null;
			try
			{
				context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create();

				new CommentsService().MakeComment( law.LawID, user.Id, commmentText );

				var responseOld = new CommentsService().GetCommentsForLaw( law.LawID, user.Id, CommentOrder.Chronological );
				var commentOld = responseOld.Comments.Where( x => x.Comment.ApplicationUserID == user.Id && x.Comment.Text == commmentText ).FirstOrDefault();

				new CommentsService().VoteComment( commentOld.Comment.LawCommentID, user.Id, true );

				var responseNew = new CommentsService().GetCommentsForLaw( law.LawID, user.Id, CommentOrder.Chronological );
				var commentNew = responseNew.Comments.Where( x => x.Comment.LawCommentID == commentOld.Comment.LawCommentID ).FirstOrDefault();

				Assert.IsNotNull( commentNew, "Comment not made" );
				Assert.AreEqual( 0, commentNew.VotesDown, "New Comment, no votes down yet" );
				Assert.AreEqual( 1, commentNew.VotesUp, "user voted up" );
				Assert.AreEqual( true, commentNew.UserVoted, "User voted" );
			}
			finally
			{
				if ( context != null )
					context.Dispose();

				Helpers.DeleteUser( user );
			}
		}
	}
}
