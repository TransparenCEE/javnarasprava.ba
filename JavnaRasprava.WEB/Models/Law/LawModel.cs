using System.Collections.Generic;

namespace JavnaRasprava.WEB.Models
{
	public class LawModel
	{
		#region == Properties ==

		public JavnaRasprava.WEB.DomainModels.Law Law { get; set; }

		public int VotesUp { get; set; }

		public double VotesUpPercentage { get; set; }

		public int VotesDown { get; set; }

		public double VotesDownPercentage { get; set; }

		public bool UserVoted { get; set; }

		public ICollection<LawSectionModel> Sections { get; set; }

		public int CommentsCount { get; set; }

		public bool HasComments { get { return CommentsCount > 0; } }

		public int ExpertCommentsCount { get; set; }

		public bool HasExpertComments { get { return ExpertCommentsCount > 0; } }

		public AnswersListModel RepresentativeAnswers { get; set; }

		public JavnaRasprava.WEB.Models.Law.LawStatModel Statistics { get; set; }

		#endregion

		public string FbCommentsPath { get; set; }
	}
}
