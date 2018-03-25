using JavnaRasprava.WEB.DomainModels;
using System.Collections.Generic;

namespace JavnaRasprava.WEB.Models
{
	public class LawQuestionsModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int LawID { get; set; }

        public JavnaRasprava.WEB.DomainModels.Law Law { get; set; }

		public List<QuestionModel> Questions { get; set; }

		public int TotalQuestionsMade { get; set; }

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion
	}
}