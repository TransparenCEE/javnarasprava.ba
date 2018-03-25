using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Law
	{
		#region == Properties ==

		public int LawID { get; set; }

		public bool IsActive { get; set; }

		public string Text { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Submitter { get; set; }

		public DateTime CreateDateTimeUtc { get; set; }

		public DateTime? ExpetedVotingDay { get; set; }
		
		[NotMapped]
		public string ExpectedVotingDayString
		{
			get
			{
				return ExpetedVotingDay.HasValue ? ExpetedVotingDay.Value.ToString( "dd.MM.yyyy." ) : GlobalLocalization.UnknownExpectedVotingDay;
			}
		}

		public DateTime? PointedOutUtc { get; set; }

		public string ImageRelativePath { get; set; }

		public string TextFileRelativePath { get; set; }

		//public int LawStatusID { get; set; }
		//public virtual LawStatus LawStatus { get; set; }

		public string StatusTitle { get; set; }

		public string StatusText { get; set; }

		public int ProcedureID { get; set; }
		public virtual Procedure Procedure { get; set; }

		public virtual Parliament Parliament { get; set; }
		public int ParliamentID { get; set; }

		public int CategoryId { get; set; }
		public virtual LawCategory Category { get; set; }

		public virtual ICollection<LawSection> LawSections { get; set; }

		public virtual ICollection<LawRepresentativeAssociation> LawRepresentativeAssociations { get; set; }

		public virtual ICollection<ExpertComment> ExpertComments { get; set; }

		public virtual ICollection<LawComment> LawComments { get; set; }

		public virtual ICollection<Question> Questions { get; set; }

		public virtual ICollection<LawVote> LawVotes { get; set; }

		public virtual ICollection<LawCustomVote> CustomVotes { get; set; }

		#endregion

	}
}