using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
	public class Helper
	{
		internal static void Seed( DomainModels.ApplicationDbContext context )
		{
			PopulateCategories( context );

			PopulateLocations( context );

			PopulateParliaments( context );

			PopulateParliamentHouse( context );

			PopulateParties( context );

			PopulateProcedures( context );

			PopulateStatuses( context );

			PopulateRepresentatives( context );

			PopulateExperts( context );

			PopulateAdmins( context );

			PopulateUsers( context );

			PopulateLaws( context );

			PopulateCustomVotes( context );

			PopulatePetitions( context );

			PopulatePetitionProgress( context );
		}

		private static void PopulateCategories( DomainModels.ApplicationDbContext context )
		{
			context.LawCategories.AddOrUpdate(x=>x.Title,
				new LawCategory{Title = "Ekonomija"},
				new LawCategory{Title ="Državno uređenje"},
				new LawCategory{Title ="Kultura i sport"},
				new LawCategory{Title ="Pravna država"},
				new LawCategory{Title ="Zdravstvo"},
				new LawCategory{Title ="Spoljna politika"},
				new LawCategory{Title ="Socijalna politka"},
				new LawCategory{Title ="Obrazovanje"}
				);
			context.SaveChanges();
		}

		private static void PopulatePetitionProgress( DomainModels.ApplicationDbContext context )
		{
			var darko = context.Representatives.Where( x => x.FirstName == "Darko" && x.LastName == "Babalj" ).First();
			var mirso = context.Representatives.Where( x => x.FirstName == "Mirsad" && x.LastName == "Đugum" ).First();
			var pero = context.Representatives.Where( x => x.FirstName == "Petar" && x.LastName == "Kunić" ).First();
			var lazo = context.Representatives.Where( x => x.FirstName == "Lazar" && x.LastName == "Prodanović" ).First();
			var stateParliament = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE ).FirstOrDefault();


			context.PetitionProgresses.AddOrUpdate( x => x.Title,
				new PetitionProgress { Title = "Poslanik Darko Babalj", Description = "Za peticije koje imaju ovoliko glasova dajemo kraću kafu" ,ImageDoneRelativePath = "checked.png", ImageToDoRelativePath = "question.png"  ,
					RepresentativeID = darko.RepresentativeID, 
					ParliamentID = stateParliament.ParliamentID },
				new PetitionProgress { Title = "Poslanik Mirsad Đugum", Description = "Za peticije koje imaju ovoliko glasova dajemo dužu kafu", ImageDoneRelativePath = "checked.png", ImageToDoRelativePath = "question.png",
									   RepresentativeID = mirso.RepresentativeID,
					ParliamentID = stateParliament.ParliamentID},
				new PetitionProgress { Title = "Poslanik Petar Kunić", Description = "Za peticije koje imaju ovoliko glasova dajemo malu kafu s mlijekom", ImageDoneRelativePath = "checked.png", ImageToDoRelativePath = "question.png" ,
									   RepresentativeID = pero.RepresentativeID, 
					ParliamentID = stateParliament.ParliamentID},
				new PetitionProgress { Title = "Poslanik Lazar Prodanović", Description = "Za peticije koje imaju ovoliko glasova dajemo veliku kafu s mlijekom", ImageDoneRelativePath = "checked.png", ImageToDoRelativePath = "question.png" ,
									   RepresentativeID = lazo.RepresentativeID, 
					ParliamentID = stateParliament.ParliamentID}
				);
		}

		private static void PopulateAdmins( DomainModels.ApplicationDbContext context )
		{
			if ( !context.Roles.Any( r => r.Name == "admin" ) )
			{
				var roleStore = new RoleStore<IdentityRole>( context );
				var roleManager = new RoleManager<IdentityRole>( roleStore );
				var role = new IdentityRole { Name = "admin" };
				roleManager.Create( role );
			}

			var store = new UserStore<ApplicationUser>( context );
			var manager = new UserManager<ApplicationUser>( store );
			if ( !context.Users.Any( u => u.UserName == "firstadmin@jr.ba" ) )
			{
				var user = new ApplicationUser { UserName = "firstadmin@jr.ba" };


				manager.Create( user, "Jes!'mAdmin" );
				manager.AddToRole( user.Id, "admin" );
			}

			if ( !context.Users.Any( u => u.UserName == "admintoplay@jr.ba" ) )
			{
				var user = new ApplicationUser { UserName = "admintoplay@jr.ba" };

				manager.Create( user, "jaSe1gr@m" );
				manager.AddToRole( user.Id, "admin" );
			}

			if ( !context.Users.Any( u => u.UserName == "bestadmin@jr.ba" ) )
			{
				var user = new ApplicationUser { UserName = "bestadmin@jr.ba" };

				manager.Create( user, "Yes1C@n" );
				manager.AddToRole( user.Id, "admin" );
			}
		}

		private static void PopulateStatuses( DomainModels.ApplicationDbContext context )
		{
			context.LawStatuses.AddOrUpdate( x => x.Name,
				new LawStatus { Name = "Prijedlog", StatusDescription= "Zakon je u formi prijedloga" },
				new LawStatus { Name = "Javna Rasprava", StatusDescription = "Takon je u javnoj raspravi" },
				new LawStatus { Name = "Glasanje", StatusDescription= "Zakon je u glasanju" } );
		}

		private static void PopulatePetitions( DomainModels.ApplicationDbContext context )
		{
			var user1 = context.Users.Where( x => x.UserName == "email1@email.com" ).FirstOrDefault();
			var user2 = context.Users.Where( x => x.UserName == "email2@email.com" ).FirstOrDefault();
			var stateParliament = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE ).FirstOrDefault();

			context.Petitions.AddOrUpdate( x => new { x.Title, x.TargetInstitutionId },
				new Petition
				{
					Title = "Trebalo bi više kofeina dodati u jaku kafu",
					Description ="Ja mislim da jača kafa nije dovoljno jaka i da treba donijeti zakon da se kafa pojača",
					Verified = true,
					SubmitterName = "Anonimac",
					SubmitterUserID = user1.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Zabrana pušenja u kafanama",
					Description ="Kafane su zaista zadimljene, skoro pa se ne može disati. Mislim da bi trebalo zabraniti pušenje u zatvorenim prostorima u potpunosti.",
					Verified = true,
					SubmitterName = "Anonimac",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Eutanazija pasa lutalica",
					Description ="Detailed description.",
					Verified = true,
					SubmitterName = "Anonimac",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Oštro kažnjavanje saobraćajnih prestupa",
					Description ="Detailed description.",
					Verified = true,
					SubmitterName = "Anonimac",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Društveno korisni rad umjesto ležanja u zatvoru",
					Description ="Detailed description.",
					Verified = true,
					SubmitterName = "Anonimac",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Ukidanje putarine za auto put",
					Description ="Detailed description.",
					Verified = true,
					SubmitterName = "NGO",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				},
				new Petition
				{
					Title = "Pro bono rad u parlamentima",
					Description ="Detailed description.",
					Verified = true,
					SubmitterName = "N. N.",
					SubmitterUserID = user2.Id,
					TargetInstitutionId = stateParliament.ParliamentID
				} );
		}

		private static void PopulateLocations( DomainModels.ApplicationDbContext context )
		{
			JavnaRasprava.WEB.Migrations.ApplicationDbContext.Seed.Locations.Seed( context );
			context.SaveChanges();
		}

		#region == Seed Methods ==

		private static void PopulateProcedures( DomainModels.ApplicationDbContext context )
		{
			context.Procedures.AddOrUpdate( x => x.Title,
				new Procedure { Title= "Redovna", Description = "Redovna procedure donosenja zakona." },
				new Procedure { Title ="Skracena", Description="Skracena procedura donosenja zakona preskace neke faze u proceduri kako bi se ubrzao proces" },
				new Procedure { Title="Hitna", Description="Hitna procedura donosenja zakona" }
				);
			context.SaveChanges();
		}

		private static void PopulateCustomVotes( DomainModels.ApplicationDbContext context )
		{
			var law = context.Laws
				.Include( x => x.LawSections )
				.FirstOrDefault();

			context.LawCustomVotes.AddOrUpdate( x => new { x.LawID, x.Text },
				new LawCustomVote { LawID = law.LawID, IsSuggested = true, Vote = true, Text= "Ovo je izuzetno vazan zakon" },
				new LawCustomVote { LawID = law.LawID, IsSuggested = true, Vote = false, Text = "Jos nije vrijeme da diskusiju o ovom pitanju" }
				);

			var firstSection = law.LawSections.First();

			context.LawSectionCustomVotes.AddOrUpdate( x => new { x.LawSectionID, x.Text },

				new LawSectionCustomVote { LawSectionID = firstSection.LawSectionID, IsSuggested = true, Vote = true, Text = "Ovo je najvazniji aspekt zakona" },
				new LawSectionCustomVote { LawSectionID = firstSection.LawSectionID, IsSuggested = true, Vote = false, Text = "Apsolutno nebitna tema" }

				);

			context.SaveChanges();
		}

		private static void PopulateUsers( DomainModels.ApplicationDbContext context )
		{
			var location = context.Locations.Where( x => x.Name == "Centar" ).First();
			var party = context.Parties.First();


			context.Users.AddOrUpdate( x => x.Email,
				new ApplicationUser
				{
					Id = "CD490FFE-CAF5-41E9-85C2-BA99A402C8D1",
					Email="anonymous@javnarasprava.ba",
					EmailConfirmed = false,
					PasswordHash ="ACFnbOlCkaNivPWjhMjhamNa6fk3a/ZoYTYXnxiUp4dFzJzt4n6uy8z6zB7HNaPNqg==",
					SecurityStamp ="313a3f35-a5ec-4548-9fca-7443db30ff5c",
					PhoneNumber = null,
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEndDateUtc = null,
					LockoutEnabled = false,
					AccessFailedCount = 0,
					UserName = "anonymous",
					PartyID = null,
					LocationID = null,
					Age = null,
					Education = null
				},
				new ApplicationUser
				{
					Id = "6a0d9d73-3bbd-4291-89c0-77f23489c5c6",
					Email="email2@email.com",
					EmailConfirmed = false,
					PasswordHash ="ACFnbOlCkaNivPWjhMjhamNa6fk3a/ZoYTYXnxiUp4dFzJzt4n6uy8z6zB7HNaPNqg==",
					SecurityStamp ="313a3f35-a5ec-4548-9fca-7443db30ff5c",
					PhoneNumber = null,
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEndDateUtc = null,
					LockoutEnabled = false,
					AccessFailedCount = 0,
					UserName = "email2@email.com",
					PartyID = party.PartyID,
					LocationID = location.LocationID,
					Age = Age.MidAge,
					Education = Education.Doktorat
				},
				new ApplicationUser
				{
					Id = "763e709f-fa20-45d4-9fcc-8068dcd69236",
					Email="email1@email.com",
					EmailConfirmed = false,
					PasswordHash ="AKbcv+GTA4vwmidC1ICzQVQNlkJnr6r2muV71BvThGVCGxUdWECW2sE6d4gTf8rDPQ==",
					SecurityStamp ="700c5918-4632-4667-8ddf-e7aa63581920",
					PhoneNumber = null,
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEndDateUtc = null,
					LockoutEnabled = false,
					AccessFailedCount = 0,
					UserName = "email1@email.com",
					PartyID = party.PartyID,
					LocationID = location.LocationID,
					Age = Age.Senior,
					Education = Education.Doktorat
				} );



			context.SaveChanges();


		}

		private static void PopulateExperts( DomainModels.ApplicationDbContext context )
		{
			context.Experts.AddOrUpdate( x => new { x.FirstName, x.LastName },
				new Expert { FirstName ="Enes", LastName="Pelko", About ="He is the smartest expert ever." } );

			context.SaveChanges();
		}

		private static void PopulateLaws( DomainModels.ApplicationDbContext context )
		{
			var stateParliament = context.Parliaments
			   .Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE )
			   .FirstOrDefault();

			var repSda = context.Parties.Where( x => x.Name == "SDA" ).First().Representatives.First();
			var repSdp = context.Parties.Where( x => x.Name == "SDP" ).First().Representatives.First();

			var expert = context.Experts.FirstOrDefault();
			var procedure = context.Procedures.First();
			var user1 = context.Users.Where( x => x.UserName == "email1@email.com" ).FirstOrDefault();
			var user2 = context.Users.Where( x => x.UserName == "email2@email.com" ).FirstOrDefault();

			var law = new Law
			{
				ParliamentID = stateParliament.ParliamentID,
				Title = "First test law Title",
				IsActive=true,
				//LawStatusID = 1,
				StatusText = "Ovo je opis statusa",
				StatusTitle = "Zakon je u formi prijedloga",
				Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
				Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
				CreateDateTimeUtc = DateTime.UtcNow,
				ExpetedVotingDay = DateTime.Today.AddMonths( 1 ),
				ProcedureID = procedure.ProcedureID,
				Submitter = "Klub poslanika SDA",
				ImageRelativePath = "default.png",
				LawSections = new List<LawSection>
				{
					 new LawSection 
					 { 
						 Title = "Test Section 1",
						 Text = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.",
						 Description = "This is section description"
					 },
					 new LawSection 
					 { 
						 Title = "Test Section 2",
						 Text = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.",
						 Description = "This is section description"
					 }                  
				}
				,
				ExpertComments = new List<ExpertComment>
				{
					 new ExpertComment
					 {
						  Expert = expert,
						   Text = "This is my good comment about this law."
					 }
				}
				,
				LawComments = new List<LawComment>
				{
					 new LawComment
					 {
						   DateTimeUtc = DateTime.UtcNow,
						   ApplicationUser = user1,
						   Text = "Test comment from user1",
						   LawCommentVotes = new List<LawCommentLike>
						   {
							   new LawCommentLike{ ApplicationUser = user2, Vote = true },
							   new LawCommentLike{ ApplicationUser = user1, Vote = true }
						   }
					 },
					 new LawComment
					 {
						   DateTimeUtc = DateTime.UtcNow,
						   ApplicationUser = user2,
						   Text = "Test comment from user2",
						   LawCommentVotes = new List<LawCommentLike>
						   {
							   new LawCommentLike{ ApplicationUser = user2, Vote = true },
							   new LawCommentLike{ ApplicationUser = user1, Vote = false }
						   }
					 },

				},
				Questions = new List<Question>
				{
					new Question
					{
						 Text = "Zasto mislite da je lorem bolji od ipsum?",
						 IsSuggested = true,
						 Verified = true,
						 CreateTimeUtc = DateTime.UtcNow,
						 UserRepresentativeQuestions = new List<UserRepresentativeQuestion>
						 {
							 new UserRepresentativeQuestion{ ApplicationUser = user1, Representative = repSda, CreateTimeUtc = DateTime.UtcNow },
							 new UserRepresentativeQuestion{ ApplicationUser = user1, Representative = repSdp, CreateTimeUtc = DateTime.UtcNow.AddDays( -3 ) }
						 },
						 Answers = new List<Answer>
						 {
							 new Answer{ Text ="Jer je ipsum najbolji!", Representative = repSda, AnsweredTimeUtc = DateTime.UtcNow.AddDays( -1 ) }
						  }
					},
					new Question
					{
						Text = "Kako vi vidite uticaj ovog zakona na zastitu zivotinja.",
						IsSuggested = true,
						 CreateTimeUtc = DateTime.UtcNow
					}
				},
				CategoryId = context.LawCategories.First().LawCategoryId

			};

			context.Laws.AddOrUpdate( x => new { x.Title, x.ParliamentID, x.ProcedureID, x.CategoryId }, law );
			context.SaveChanges();

			law = context.Laws.Where( x => x.Title == "First test law Title" ).FirstOrDefault();

			context.LawRepresentativeAssociations.AddOrUpdate( x => new { x.RepresentativeID, x.LawID },
				 new LawRepresentativeAssociation
				 {
					 LawID = law.LawID,
					 RepresentativeID = repSda.RepresentativeID,
					 Reason = "Predlagac"
				 },
				 new LawRepresentativeAssociation
				 {
					 LawID = law.LawID,
					 RepresentativeID = repSdp.RepresentativeID,
					 Reason = "Opozicija"
				 }
			 );
			context.SaveChanges();

		}

		private static void PopulateRepresentatives( DomainModels.ApplicationDbContext context )
		{
			// State parliament lower

			var stateParliamentID = context.Parliaments
				.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE )
				.FirstOrDefault()
				.ParliamentHouses
				.Where( ph => ph.Name == StringConstants.HOUSE_NAME_STATE_LOWER )
				.FirstOrDefault().ParliamentHouseID;


			var stateParliamentUpperID= context.Parliaments
				.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE )
				.FirstOrDefault()
				.ParliamentHouses
				.Where( ph => ph.Name == StringConstants.HOUSE_NAME_STATE_UPPER )
				.FirstOrDefault().ParliamentHouseID;

			var SDA = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SDA ).FirstOrDefault().PartyID;
			var SDP = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SDP ).FirstOrDefault().PartyID;
			var SDS = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SDS ).FirstOrDefault().PartyID;
			var SBB = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SBBBiH ).FirstOrDefault().PartyID;
			var DNS = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_DNS ).FirstOrDefault().PartyID;
			var SNSD = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SNSD ).FirstOrDefault().PartyID;
			var DNZ = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_DNZ ).FirstOrDefault().PartyID;
			var HDZ = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_HDZBiH ).FirstOrDefault().PartyID;
			var SBiH = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_SBiH ).FirstOrDefault().PartyID;
			var HDZ1990 = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_HDZ1990 ).FirstOrDefault().PartyID;
			var NSRzB = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_NSRzB ).FirstOrDefault().PartyID;
			var HSP = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_HSP ).FirstOrDefault().PartyID;
			var PDP = context.Parties.Where( x => x.Name == StringConstants.PARTY_NAME_PDP ).FirstOrDefault().PartyID;

			context.Representatives.AddOrUpdate( x => new { x.FirstName, x.LastName, x.ParliamentHouseID, x.PartyID },
				new Representative { FirstName = "Darko", LastName = "Babalj", PartyID = SDS, ParliamentHouseID = stateParliamentID, ImageRelativePath = "darko babalj 1.jpg" },
new Representative { FirstName = "Mirsad", LastName = "Đugum", PartyID = SBB, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mirsad dugum.jpg", NumberOfVotes = 1 },
new Representative { FirstName = "Petar", LastName = "Kunić", PartyID = DNS, ParliamentHouseID = stateParliamentID, ImageRelativePath = "petar kunic.jpg", NumberOfVotes = 2 },
new Representative { FirstName = "Lazar", LastName = "Prodanović", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "lazar prodanovic.jpg", NumberOfVotes = 3 },
new Representative { FirstName = "Adnan", LastName = "Bašić", PartyID = SBB, ParliamentHouseID = stateParliamentID, ImageRelativePath = "adnan basic.jpg", NumberOfVotes = 4 },
new Representative { FirstName = "Amir", LastName = "Fazlić", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "amir fazlic.jpg", NumberOfVotes = 5 },
new Representative { FirstName = "Mirza", LastName = "Kušljugić", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mirza kusljugic.jpg", NumberOfVotes = 6 },
new Representative { FirstName = "Nermin", LastName = "Purić", PartyID = DNZ, ParliamentHouseID = stateParliamentID, ImageRelativePath = "nermin puric.jpg", NumberOfVotes = 7 },
new Representative { FirstName = "Denis", LastName = "Bećirović", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "denic becirovic.jpg" , NumberOfVotes = 8},
new Representative { FirstName = "Mato", LastName = "Franjičević", PartyID = HDZ, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mato franjicevic.jpg", NumberOfVotes = 9 },
new Representative { FirstName = "Niko", LastName = "Lozančić", PartyID = HDZ, ParliamentHouseID = stateParliamentID, ImageRelativePath = "niko lozancic.jpg" , NumberOfVotes = 10},
new Representative { FirstName = "Asim", LastName = "Sarajlić", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "asim sarajlic.jpg", NumberOfVotes = 11},
new Representative { FirstName = "Beriz", LastName = "Belkić", PartyID = SBiH, ParliamentHouseID = stateParliamentID, ImageRelativePath = "beriz belkic.jpg" },
new Representative { FirstName = "Azra", LastName = "Hadžiahmetović", PartyID = SBiH, ParliamentHouseID = stateParliamentID, ImageRelativePath = "azra hadziahmetovic.jpg" },
new Representative { FirstName = "Božo", LastName = "Ljubić", PartyID = HDZ1990, ParliamentHouseID = stateParliamentID, ImageRelativePath = "bozo ljubic.jpg" },
new Representative { FirstName = "Salko", LastName = "Sokolović", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "salko sokolovic.jpg" },
new Representative { FirstName = "Borislav", LastName = "Bojić", PartyID = SDS, ParliamentHouseID = stateParliamentID, ImageRelativePath = "borislav bojic.jpg" },
new Representative { FirstName = "Mladen", LastName = "Ivanković Lijanović", PartyID = NSRzB, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mladen ivankovic lijanovic.jpg" },
new Representative { FirstName = "Saša", LastName = "Magazinović", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "sasa magazinovic.jpg" },
new Representative { FirstName = "Senad", LastName = "Šepić", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "senad sepic.jpg" },
new Representative { FirstName = "Mladen", LastName = "Bosić", PartyID = SDS, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mladen bosic.jpg" },
new Representative { FirstName = "Zijad", LastName = "Jagodić", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "zijad jagodic 2012.jpg" },
new Representative { FirstName = "Dušanka", LastName = "Majkić", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "dusanka majkic.jpg" },
new Representative { FirstName = "Boško", LastName = "Tomić", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "bosko tomic.jpg" },
new Representative { FirstName = "Saša", LastName = "Bursać", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "sasa bursac.jpg" },
new Representative { FirstName = "Slavko", LastName = "Jovičić", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "slavko jovicic.jpg" },
new Representative { FirstName = "Milica", LastName = "Marković", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "milica markovic.jpg" },
new Representative { FirstName = "Dragan", LastName = "Vrankić", PartyID = HDZ, ParliamentHouseID = stateParliamentID, ImageRelativePath = "dragan vrankic.jpg" },
new Representative { FirstName = "Nermina", LastName = "Ćemalović", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "nermina cemalovic.jpg" },
new Representative { FirstName = "Zvonko", LastName = "Jurišić", PartyID = HSP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "zvonko jurisic.jpg" },
new Representative { FirstName = "Danijela", LastName = "Martinović", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "danijela martinovic.jpg" },
new Representative { FirstName = "Nermina", LastName = "Zaimović-Uzunović", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "nermina zaimovic uzunovic.jpg" },
new Representative { FirstName = "Ismeta", LastName = "Dervoz", PartyID = SBB, ParliamentHouseID = stateParliamentID, ImageRelativePath = "ismeta dervoz .jpg" },
new Representative { FirstName = "Emir", LastName = "Kabil", PartyID = SBB, ParliamentHouseID = stateParliamentID, ImageRelativePath = "emir kabil.jpg" },
new Representative { FirstName = "Šemsudin", LastName = "Mehmedović", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "semsudin mehmedovic.jpg" },
new Representative { FirstName = "Milorad", LastName = "Živković", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "milorad zivkovic.jpg" },
new Representative { FirstName = "Anto", LastName = "Domazet", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "anto domazet.jpg" },
new Representative { FirstName = "Drago", LastName = "Kalabić", PartyID = SNSD, ParliamentHouseID = stateParliamentID, ImageRelativePath = "drago kalabic.jpg" },
new Representative { FirstName = "Mirsad", LastName = "Mešić", PartyID = SDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "mirsad mesic.jpg" },
new Representative { FirstName = "Šefik", LastName = "Džaferović", PartyID = SDA, ParliamentHouseID = stateParliamentID, ImageRelativePath = "sefik dzaferovic.jpg" },
new Representative { FirstName = "Vesna", LastName = "Krstović-Spremo", PartyID = PDP, ParliamentHouseID = stateParliamentID, ImageRelativePath = "vesna krstovic spremo.jpg" },
new Representative { FirstName = "Aleksandra", LastName = "Pandurević", PartyID = SDS, ParliamentHouseID = stateParliamentID, ImageRelativePath = "Aleksandra Pandurevic.jpg" },


// Dom naroda
new Representative { FirstName = "Mehmed", LastName = "Bradarić", PartyID = SDP, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "mehmed bradaric.JPG" },
new Representative { FirstName = "Mladen", LastName = "Ivanić", PartyID = PDP, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "mladen ivanic.JPG" },
new Representative { FirstName = "Borjana", LastName = "Krišto", PartyID = HDZ, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "borjana kristo.JPG" },
new Representative { FirstName = "Ognjen", LastName = "Tadić", PartyID = SDS, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "ognjen tadic okt 2011.jpg" },
new Representative { FirstName = "Dragan", LastName = "Čović", PartyID = HDZ, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "dragan covic za pd.jpg" },
new Representative { FirstName = "Nermina", LastName = "Kapetanović", PartyID = SDA, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "nermina kapetanovic.JPG" },
new Representative { FirstName = "Martin", LastName = "Raguž", PartyID = HDZ1990, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "martin raguz za web 2.jpg" },
new Representative { FirstName = "Sulejman", LastName = "Tihić", PartyID = SDA, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "sulejman tihic.JPG" },
new Representative { FirstName = "Halid", LastName = "Genjac", PartyID = SDA, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "halid genjac.JPG" },
new Representative { FirstName = "Staša", LastName = "Košarac", PartyID = SNSD, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "stasa kosarac.JPG" },
new Representative { FirstName = "Dragutin", LastName = "Rodić", PartyID = DNS, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "dragutin rodic.JPG" },
new Representative { FirstName = "Krunoslav", LastName = "Vrdoljak", PartyID = SDP, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "krunoslav vrdoljak.JPG" },
new Representative { FirstName = "Seudin", LastName = "Hodžić", PartyID = SDP, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "seudin hodzic za web 1.jpg" },
new Representative { FirstName = "Stjepan", LastName = "Krešić", PartyID = HSP, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "stjepan kresic.JPG" },
new Representative { FirstName = "Krstan", LastName = "Simić", PartyID = SNSD, ParliamentHouseID = stateParliamentUpperID, ImageRelativePath = "krstan simic.JPG" }



				);

			context.SaveChanges();
		}

		private static void PopulateParties( DomainModels.ApplicationDbContext context )
		{
			context.Parties.AddOrUpdate( x => x.Name,
				new Party { Name = StringConstants.PARTY_NAME_SDA },
				new Party { Name = StringConstants.PARTY_NAME_SBBBiH },
				new Party { Name = StringConstants.PARTY_NAME_SDP },
				new Party { Name = StringConstants.PARTY_NAME_HDZBiH },
				new Party { Name = StringConstants.PARTY_NAME_SBiH },
				new Party { Name = StringConstants.PARTY_NAME_HDZ1990 },
				new Party { Name = StringConstants.PARTY_NAME_SNSD },
				new Party { Name = StringConstants.PARTY_NAME_SDS },
				new Party { Name = StringConstants.PARTY_NAME_PDP },
				new Party { Name = StringConstants.PARTY_NAME_DNS },
				new Party { Name = StringConstants.PARTY_NAME_NSRzB },
				new Party { Name = StringConstants.PARTY_NAME_DNZ },
				new Party { Name = StringConstants.PARTY_NAME_HSP },
				new Party { Name = "Enesova"}
				);

			context.SaveChanges();
		}

		private static void PopulateParliamentHouse( JavnaRasprava.WEB.DomainModels.ApplicationDbContext context )
		{
			var stateParliament = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_STATE ).FirstOrDefault();
			var federalParliament = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_FEDERAL ).FirstOrDefault();
			var rsParliament = context.Parliaments.Where( x => x.Name == StringConstants.PARLIAMENT_NAME_RS ).FirstOrDefault();

			var stateDomNaroda = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_STATE_UPPER,
				ParliamentID = stateParliament.ParliamentID
			};

			var statePredstavnicki = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_STATE_LOWER,
				ParliamentID = stateParliament.ParliamentID
			};

			var fbihDomNaroda = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_FBIH_UPPER,
				ParliamentID = federalParliament.ParliamentID
			};

			var fbihPredstavnicki = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_FBIH_LOWER,
				ParliamentID = federalParliament.ParliamentID
			};

			var rsDomNaroda = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_RS_UPPER,
				ParliamentID = rsParliament.ParliamentID
			};

			var rsPredstavnicki = new ParliamentHouse
			{
				Name = StringConstants.HOUSE_NAME_RS_LOWER,
				ParliamentID = rsParliament.ParliamentID
			};
			context.ParliamentHouses.AddOrUpdate( ph => new { ph.ParliamentID, ph.Name },
				stateDomNaroda,
				statePredstavnicki,
				fbihDomNaroda,
				fbihPredstavnicki,
				rsDomNaroda,
				rsPredstavnicki );

			context.SaveChanges();
		}

		private static void PopulateParliaments( JavnaRasprava.WEB.DomainModels.ApplicationDbContext context )
		{
			var stateParliament = new Parliament
			{
				Name = StringConstants.PARLIAMENT_NAME_STATE
			};
			var federalParliament = new Parliament
			{
				Name = StringConstants.PARLIAMENT_NAME_FEDERAL
			};
			var rsParliament = new Parliament
			{
				Name = StringConstants.PARLIAMENT_NAME_RS
			};

			context.Parliaments.AddOrUpdate( p => p.Name, stateParliament, federalParliament, rsParliament );

			context.SaveChanges();
		}

		#endregion
	}
}