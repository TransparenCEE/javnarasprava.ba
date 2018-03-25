using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models;
using System.Linq;

namespace JavnaRasprava.WEB.BLL
{
	public class VotingService
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public Models.VotingResultsModel PrepareFilters()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new VotingResultsModel
				{
					Parties = UserService.GetAllPartiesSelectListInternal( context ),
					Locations = UserService.GetAllLocationsSelectListInternal( context )
				};

				return result;
			}
		}

		public Models.VotingResultsModel GetResultsForLaw( Models.VotingResultsModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = InitResult( model, context );

				// Getting basic vote counts
				var lawVotes = context.LawVotes
								.Where( x => x.LawID == model.ID );

				if ( model.LocationID != 0 )
					lawVotes = lawVotes.Where( x => x.ApplicationUser.LocationID == model.LocationID );

				if ( model.PartyID != 0 )
					lawVotes = lawVotes.Where( x => x.ApplicationUser.PartyID == model.PartyID );

				if ( model.Age != 0 )
					lawVotes = lawVotes.Where( x => x.ApplicationUser.Age == model.Age );

				if ( model.Education != 0 )
					lawVotes = lawVotes.Where( x => x.ApplicationUser.Education == (JavnaRasprava.WEB.Infrastructure.Education)model.Age );

				var lawVotesList = lawVotes.GroupBy( x => x.Vote )
											.Select( g => new { Key = g.Key, Count = g.Count() } )
											.ToList();

				var unsafeResult = lawVotesList.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
				result.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

				unsafeResult = lawVotesList.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
				result.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

				result.VotesDownPercentage = Infrastructure.Math.Percentage( result.VotesDown, result.VotesDown + result.VotesUp );
				result.VotesUpPercentage = Infrastructure.Math.Percentage( result.VotesUp, result.VotesDown + result.VotesUp );

				// populate custom votes distribution
				lawVotes.Where( x => x.LawCustomVote != null )
					.GroupBy( x => new { x.LawCustomVote.LawCustomVoteID, x.LawCustomVote.Text, x.LawCustomVote.Vote } )
					.Select( g => new { ID = g.Key.LawCustomVoteID, Text = g.Key.Text, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList()
					.ForEach( x =>
						{
							result.CustomResultsDistribution.Add(
								new CustomVoteResultModel
								{
									ID = x.ID,
									Text = x.Text,
									Vote = x.Vote,
									Count = x.Count
								} );
						} );

				// populate default votes distribution
				lawVotes.Where( x => x.LawCustomVote == null )
					.GroupBy( x => x.Vote )
					.Select( g => new { Key = g.Key, Count = g.Count() } )
					.ToList()
					.ForEach( x =>
					{
						result.CustomResultsDistribution.Add(
							new CustomVoteResultModel
							{
								ID = 0,
								Text = x.Key.Value ? Resources.GlobalLocalization.Global_VoteUp : Resources.GlobalLocalization.Global_VoteDown,
								Vote = x.Key.Value,
								Count = x.Count
							} );
					} );

				var totalCustomVotesCount = result.CustomResultsDistribution.Sum( x => x.Count );

				foreach ( var customVote in result.CustomResultsDistribution )
				{
					customVote.Percent = Math.Percentage( customVote.Count, totalCustomVotesCount );
				}

				return result;
			}
		}

		public Models.VotingResultsModel GetResultsForLawSection( Models.VotingResultsModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = InitResult( model, context );

				// Getting basic vote counts
				var votesQuery = context.LawSectionVotes
								.Where( x => x.LawSectionID == model.ID );

				if ( model.LocationID != 0 )
					votesQuery = votesQuery.Where( x => x.ApplicationUser.LocationID == model.LocationID );

				if ( model.PartyID != 0 )
					votesQuery = votesQuery.Where( x => x.ApplicationUser.PartyID == model.PartyID );

				if ( model.Age != 0 )
					votesQuery = votesQuery.Where( x => x.ApplicationUser.Age == model.Age );

				if ( model.Education != 0 )
					votesQuery = votesQuery.Where( x => x.ApplicationUser.Education == (JavnaRasprava.WEB.Infrastructure.Education)model.Age );

				var votesList = votesQuery.GroupBy( x => x.Vote )
											.Select( g => new { Key = g.Key, Count = g.Count() } )
											.ToList();

				var unsafeResult = votesList.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
				result.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

				unsafeResult = votesList.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
				result.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

				result.VotesDownPercentage = Infrastructure.Math.Percentage( result.VotesDown, result.VotesDown + result.VotesUp );
				result.VotesUpPercentage = Infrastructure.Math.Percentage( result.VotesUp, result.VotesDown + result.VotesUp );

				// Populate custom votes distribution
				votesQuery.Where( x => x.LawSectionCustomVote != null )
					.GroupBy( x => new { x.LawSectionCustomVote.LawSectionCustomVoteID, x.LawSectionCustomVote.Text, x.LawSectionCustomVote.Vote } )
					.Select( g => new { ID = g.Key.LawSectionCustomVoteID, Text = g.Key.Text, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList()
					.ForEach( x =>
					{
						result.CustomResultsDistribution.Add(
							new CustomVoteResultModel
							{
								ID = x.ID,
								Text = x.Text,
								Vote = x.Vote,
								Count = x.Count
							} );
					} );

				// populate default votes distribution
				votesQuery.Where( x => x.LawSectionCustomVote == null )
					.GroupBy( x => x.Vote )
					.Select( g => new { Key = g.Key, Count = g.Count() } )
					.ToList()
					.ForEach( x =>
					{
						result.CustomResultsDistribution.Add(
							new CustomVoteResultModel
							{
								ID = 0,
								Text = x.Key.Value ? Resources.GlobalLocalization.Global_VoteUp : Resources.GlobalLocalization.Global_VoteDown,
								Vote = x.Key.Value,
								Count = x.Count
							} );
					} );

				var totalCustomVotesCount = result.CustomResultsDistribution.Sum( x => x.Count );

				foreach ( var customVote in result.CustomResultsDistribution )
				{
					customVote.Percent = Math.Percentage( customVote.Count, totalCustomVotesCount );
				}

				return result;

			}
		}

		private static VotingResultsModel InitResult( Models.VotingResultsModel model, DomainModels.ApplicationDbContext context )
		{
			var result = new Models.VotingResultsModel
			{
				ID = model.ID,
				Age = model.Age,
				Education = model.Education,
				LocationID = model.LocationID,
				Locations = UserService.GetAllLocationsSelectListInternal( context ),
				PartyID = model.PartyID,
				Parties = UserService.GetAllPartiesSelectListInternal( context ),
				CustomResultsDistribution = new System.Collections.Generic.List<CustomVoteResultModel>()
			};
			return result;
		}

		#endregion
	}
}