using System.Collections.Generic;

namespace JavnaRasprava.WEB.Models
{
	public class AskLawQuestionModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int LawID { get; set; }

		public List<AskRepresentativeModel> SuggestedRepresentatives { get; set; }

		public List<AskRepresentativeModel> OtherRepresentatives { get; set; }

		public List<AskPredefinedQuestionsModel> Questions { get; set; }

		public string Text { get; set; }

		#endregion

		#region == Constructors ==

		public AskLawQuestionModel()
		{
			SuggestedRepresentatives = new List<AskRepresentativeModel>();
		}

		#endregion

		#region == Methods ==

		#endregion
	}
}