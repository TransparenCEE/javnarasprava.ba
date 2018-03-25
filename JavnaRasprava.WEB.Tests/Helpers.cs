using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace JavnaRasprava.WEB.Tests
{
	class Helpers
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#region == Law Methods ==

		public static Law GetTestLaw()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Laws
					.FirstOrDefault();
			}
		}

		public static Law GetTestLawExtended()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Laws
					.Include( "LawRepresentativeAssociations.Representative.Party" )
					.Include( x => x.Questions )
					.FirstOrDefault();
			}
		}

		internal static Law CreateLaw( int ParliamentID, List<string> relatedRepresentatives,
			List<string> questions, string userToAskQuestions = null )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var representatives = context.Representatives
					.Where( x => relatedRepresentatives.Contains( x.FirstName+x.LastName ) )
					.ToList();

				Law law = new Law
				{
					Title = "Test title",
					Description = "Test Description",
					Text = "Test text",
					CreateDateTimeUtc = DateTime.UtcNow,
					ExpetedVotingDay = DateTime.UtcNow.AddDays( 30 ),
					ParliamentID = ParliamentID,
					ProcedureID = 1,
					Submitter = "test submitter",
					LawRepresentativeAssociations = representatives.Select( x => new LawRepresentativeAssociation
					{
						Reason = "Test Reason",
						RepresentativeID = x.RepresentativeID
					} ).ToList(),
					Questions = questions.Select( x => new Question
					{
						CreateTimeUtc = DateTime.UtcNow,
						IsSuggested = true,
						Text = x
					} ).ToList()
				};

				context.Laws.Add( law );

				if ( userToAskQuestions != null )
				{
					var relations = new List<UserRepresentativeQuestion>();

					representatives.ForEach( r => relations.AddRange(
						law.Questions.Select( x => new UserRepresentativeQuestion
						{
							ApplicationUserID = userToAskQuestions,
							Question = x,
							Representative = r,
							CreateTimeUtc = DateTime.UtcNow
						} ) ) );
					context.UserRepresentativeQuestions.AddRange( relations );
				}
				context.SaveChanges();
				return law;
			}
		}

		internal static void DeleteLaw( Law law )
		{
			new LawService().DeleteLaw( law.LawID );
		}


		#endregion

		#region == User Methods ==

		public static ApplicationUser CreateNewUser()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var id = Guid.NewGuid().ToString();
				var email = string.Format( "{0}@email.com", Guid.NewGuid() );
				var location = context.Locations.First();
				var party = context.Parties.First();
				ApplicationUser user= new ApplicationUser
				{
					Id = id,
					Email =email,
					UserName = email,
					Location = location,
					Party = party
				};
				context.Users.Add( user );
				context.SaveChanges();
				return user;
			}
		}

		public static void DeleteUser( ApplicationUser user )
		{
			new UserService().DeleteUser( user );
		}

		public static void DeleteUsers( ICollection<string> userIds )
		{
			var userService = new UserService();

			foreach ( var id in userIds )
				userService.DeleteUser( id );
		}

		internal static ApplicationUser CreateNewUser( Infrastructure.Age age, Infrastructure.Education education,
			string locationName, string partyName )
		{
			var initalUser = CreateNewUser();

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var location = context.Locations.Where( x => x.Name == locationName ).First();
				var party = context.Parties.Where( x => x.Name == partyName ).First();

				initalUser.Party = party;
				initalUser.Location = location;
				initalUser.Age = age;
				initalUser.Education = education;

				context.SaveChanges();
				return initalUser;
			}
		}

		#endregion

		#region == Petition Methods ==

		public static int CreateNewPetition( ApplicationUser user,
			string title = "Some title",
			string description = "Some Description",
			int targetCount = 5000,
			string submitterName = "Some submitter",
			string targetInstitution = "Some institution",
			DateTime? startTime = null,
			DateTime? endTime = null )
		{
			var service = new PetitionService();
			var model = new Models.PetitionModel
			{
				Title = title,
				Description = description,
				SubmitterName = submitterName,
				SubmitterUserID = user.Id,
				TargetInstitution = targetInstitution,
				Verified = false

			};

			return service.CreateNewPetition( model );
		}

		public static void DeletePetition( int id )
		{
			new PetitionService().DeletePetition( id );
		}

		#endregion


		internal static int GetTotalNumberOfRepresentativesForparliament( int parliamentID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Parliaments
					.Where( x => x.ParliamentID == parliamentID )
					.SelectMany( x => x.ParliamentHouses )
					.SelectMany( x => x.Representatives )
					.Count();
			}
		}


		internal static List<string> SignPetition( int petitionID, int count )
		{
			List<string> results = new List<string>();
			for ( int i = 0; i< count; i++ )
			{
				var user = CreateNewUser();
				results.Add( user.Id );

				try
				{
					new PetitionService().Sign( petitionID, user.Id );
				}
				catch
				{

				}
			}
			return results;
		}

		#endregion




	}
}
