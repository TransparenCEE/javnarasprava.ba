using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JavnaRasprava.WEB.BLL
{
	public class CommentsService
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public Models.CommentsListModel GetCommentsForLaw( int lawID, string userID, Infrastructure.CommentOrder order )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var results = new Models.CommentsListModel()
				{
					Comments = new List<Models.CommentModel>(),
					LawID =lawID
				};

				var comments = context.LawComments
										.Where( x => x.LawID == lawID )
										.Include( x => x.ApplicationUser )
										.ToList();

				var commentVotingResults = context.LawCommentLikes
												.Where( x => x.LawID == lawID )
												.GroupBy( x => new { x.LawCommentID, x.Vote } )
												.Select( g => new { CommentID = g.Key.LawCommentID, Vote = g.Key.Vote, Count = g.Count() } )
												.ToList();

				var userVotes = context.LawCommentLikes
										.Where( x => x.LawID == lawID && x.ApplicationUserID == userID )
										.Select( x => x.LawCommentID )
										.ToList();

				foreach ( var comment in comments )
				{
					var result = new Models.CommentModel()
					{
						Comment = comment,
						UserVoted = userVotes.Any( x => x == comment.LawCommentID )
					};

					var nullableResult = commentVotingResults.Where( x => x.CommentID == comment.LawCommentID && x.Vote == true ).FirstOrDefault();
					result.VotesUp = nullableResult == null ? 0 : nullableResult.Count;

					nullableResult = commentVotingResults.Where( x => x.CommentID == comment.LawCommentID && x.Vote == false ).FirstOrDefault();
					result.VotesDown = nullableResult == null ? 0 : nullableResult.Count;

					result.VotesDownPercentage = Infrastructure.Math.Percentage( result.VotesDown, result.VotesDown + result.VotesUp );
					result.VotesUpPercentage = Infrastructure.Math.Percentage( result.VotesUp, result.VotesDown + result.VotesUp );

					results.Comments.Add( result );
				}

				return results;
			}
		}

		public void MakeComment( int lawID, string userID, string text )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				if ( string.IsNullOrWhiteSpace( text ) )
					return;

				var user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
				if ( user == null )
					return;

				var law = context.Laws.Where( x => x.LawID == lawID ).FirstOrDefault();
				if ( law == null )
					return;

				context.LawComments
					.Add( new DomainModels.LawComment
					{
						ApplicationUser = user,
						Law = law,
						DateTimeUtc = DateTime.UtcNow,
						Text = text
					} );

				context.SaveChanges();
			}
		}

		public void VoteComment( int commentID, string userID, bool vote )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
				if ( user == null )
					return;

				var comment = context.LawComments
					.Where( x => x.LawCommentID == commentID )
					.Include( x => x.Law )
					.FirstOrDefault();
				if ( comment == null )
					return;

				if ( context.LawCommentLikes.Any( x => x.ApplicationUserID == userID ) )
					return;

				context.LawCommentLikes.Add(
					new DomainModels.LawCommentLike
					{
						ApplicationUser = user,
						LawComment = comment,
						Vote = vote,
						Law = comment.Law
					} );

				context.SaveChanges();

			}
		}
		#endregion
	}
}