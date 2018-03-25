using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace JavnaRasprava.WEB.DomainModels
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region == Properties ==

		public DbSet<Answer> Answers { get; set; }

		public DbSet<AnswerLike> AnswerLikes { get; set; }

		public DbSet<AnswerToken> AnswerTokens { get; set; }

		public DbSet<Expert> Experts { get; set; }

		public DbSet<ExpertComment> ExpertComments { get; set; }

		public DbSet<ExternalLink> ExternalLinks { get; set; }

		public DbSet<InfoBoxItem> InfoBoxItems { get; set; }

		public DbSet<Law> Laws { get; set; }

		public DbSet<LawCategory> LawCategories { get; set; }

		public DbSet<LawComment> LawComments { get; set; }

		public DbSet<LawCommentLike> LawCommentLikes { get; set; }

		public DbSet<LawCustomVote> LawCustomVotes { get; set; }

		public DbSet<LawRepresentativeAssociation> LawRepresentativeAssociations { get; set; }

		public DbSet<LawSection> LawSections { get; set; }

		public DbSet<LawSectionCustomVote> LawSectionCustomVotes { get; set; }

		public DbSet<LawSectionVote> LawSectionVotes { get; set; }

		public DbSet<LawStatus> LawStatuses { get; set; }

		public DbSet<LawVote> LawVotes { get; set; }

		public DbSet<Location> Locations { get; set; }

		public DbSet<News> News { get; set; }

		public DbSet<Notification> Notifications { get; set; }

		public DbSet<NotificationData> NotificationData { get; set; }

		public DbSet<Parliament> Parliaments { get; set; }

		public DbSet<ParliamentHouse> ParliamentHouses { get; set; }

		public DbSet<Party> Parties { get; set; }

		public DbSet<Petition> Petitions { get; set; }

		public DbSet<PetitionSignature> PetitionSignatures { get; set; }

		public DbSet<Procedure> Procedures { get; set; }

		public DbSet<Question> Questions { get; set; }

		public DbSet<QuestionLike> QuestionLikes { get; set; }

		public DbSet<QuestionMessage> QuestionMessages { get; set; }

		public DbSet<Quiz> Quizes { get; set; }

		public DbSet<QuizItem> QuizItems { get; set; }

		public DbSet<Region> Regions { get; set; }

		public DbSet<Representative> Representatives { get; set; }

		public DbSet<RepresentativeAssignment> RepresentativeAssignments { get; set; }

		public DbSet<UserRepresentativeQuestion> UserRepresentativeQuestions { get; set; }

		public DbSet<PetitionProgress> PetitionProgresses { get; set; }

		#endregion

		#region == Constructors ==

		public ApplicationDbContext()
			: base( "DefaultConnection", throwIfV1Schema: false )
		{
			this.Configuration.LazyLoadingEnabled = false;

			Database.SetInitializer<ApplicationDbContext>( new MigrateDatabaseToLatestVersion<ApplicationDbContext, JavnaRasprava.WEB.Migrations.ApplicationDbContext.Configuration>() );
		}

		#endregion


		#region == Methods ==

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );

			modelBuilder.Entity<Location>()
				.HasRequired( l => l.Region )
				.WithMany( r => r.Locations )
				.HasForeignKey( l => l.RegionID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<ApplicationUser>()
				.HasOptional( au => au.Location )
				.WithMany()
				.HasForeignKey( au => au.LocationID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<ApplicationUser>()
				.HasOptional( au => au.Party )
				.WithMany()
				.HasForeignKey( au => au.PartyID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<Law>()
				.HasRequired( l => l.Procedure )
				.WithMany()
				.HasForeignKey( l => l.ProcedureID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawRepresentativeAssociation>()
				.HasRequired( a => a.Law )
				.WithMany( l => l.LawRepresentativeAssociations )
				.HasForeignKey( a => a.LawID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<LawRepresentativeAssociation>()
				.HasRequired( f => f.Representative )
				.WithMany()
				.HasForeignKey( x => x.RepresentativeID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawVote>()
				.HasRequired( lv => lv.ApplicationUser )
				.WithMany()
				.HasForeignKey( lv => lv.ApplicationUserID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawVote>()
							.HasRequired( lv => lv.Law )
							.WithMany( l => l.LawVotes )
							.HasForeignKey( lv => lv.LawID )
							.WillCascadeOnDelete( true );


			modelBuilder.Entity<LawSectionVote>()
				.HasRequired( lv => lv.ApplicationUser )
				.WithMany()
				.HasForeignKey( lv => lv.ApplicationUserID )
				.WillCascadeOnDelete( false );

			//modelBuilder.Entity<LawSectionVote>()
			//	.HasRequired( f => f.Law )
			//	.WithMany()
			//	.HasForeignKey( lsv => lsv.LawID )
			//	.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawComment>()
				.HasRequired( lc => lc.ApplicationUser )
				.WithMany()
				.HasForeignKey( lc => lc.ApplicationUserID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawCommentLike>()
				.HasRequired( v => v.LawComment )
				.WithMany( c => c.LawCommentVotes )
				.HasForeignKey( v => v.LawCommentID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<LawCommentLike>()
				.HasRequired( v => v.Law )
				.WithMany()
				.HasForeignKey( v => v.LawID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<LawCommentLike>()
				.HasRequired( lv => lv.ApplicationUser )
				.WithMany()
				.HasForeignKey( lv => lv.ApplicationUserID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<UserRepresentativeQuestion>()
				.HasRequired( x => x.ApplicationUser )
				.WithMany()
				.HasForeignKey( x => x.ApplicationUserID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<UserRepresentativeQuestion>()
				.HasRequired( x => x.Representative )
				.WithMany( r => r.UserRepresentativeQuestions )
				.HasForeignKey( x => x.RepresentativeID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<UserRepresentativeQuestion>()
				.HasRequired( x => x.Question )
				.WithMany( q => q.UserRepresentativeQuestions )
				.HasForeignKey( x => x.QuestionID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<Answer>()
				.HasRequired( x => x.Representative )
				.WithMany()
				.HasForeignKey( x => x.RepresentativeID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<Answer>()
				.HasRequired( x => x.Question )
				.WithMany( a => a.Answers )
				.HasForeignKey( x => x.QuestionID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<AnswerLike>()
				.HasRequired( x => x.Answer )
				.WithMany( a => a.Likes )
				.HasForeignKey( x => x.AnswerID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<AnswerLike>()
				.HasRequired( x => x.ApplicationUser )
				.WithMany()
				.HasForeignKey( x => x.ApplicationUserID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<QuestionLike>()
				.HasRequired( x => x.Question )
				.WithMany( a => a.Likes )
				.HasForeignKey( x => x.QuestionID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<QuestionLike>()
				.HasRequired( x => x.ApplicationUser )
				.WithMany()
				.HasForeignKey( x => x.ApplicationUserID )
				.WillCascadeOnDelete( false );


			modelBuilder.Entity<AnswerToken>()
				.HasRequired( x => x.Representative )
				.WithMany()
				.HasForeignKey( x => x.RepresentativeID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<AnswerToken>()
				.HasRequired( x => x.Question )
				.WithMany()
				.HasForeignKey( x => x.QuestionID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<PetitionProgress>()
				.HasRequired( x => x.Parliament )
				.WithMany()
				.HasForeignKey( x => x.ParliamentID )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<PetitionProgress>()
				.HasOptional( x => x.Representative )
				.WithMany()
				.HasForeignKey( x => x.RepresentativeID )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<Petition>()
				.HasRequired( x => x.TargetInstitution )
				.WithMany()
				.HasForeignKey( x => x.TargetInstitutionId )
				.WillCascadeOnDelete( true );

			modelBuilder.Entity<Law>()
				.HasRequired( x => x.Category )
				.WithMany( x => x.Laws )
				.HasForeignKey( x => x.CategoryId )
				.WillCascadeOnDelete( true );

		}

		#endregion
	}
}