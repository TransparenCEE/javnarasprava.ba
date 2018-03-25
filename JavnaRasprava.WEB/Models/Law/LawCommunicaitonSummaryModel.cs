using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawCommunicaitonSummaryModel
	{
		public int LawID { get; set; }

		public int MostFrequentQuestionID { get; set; }

		public string MostFrequentQuestionText{ get; set; }

		public int MostFrequentQuestionCount { get; set; }

		public int MostPopularAnswerID{ get; set; }

		public string MostPopularAnswerRepresentativeName { get; set; }

		public int BestRepresentativeID { get; set; }

		public string BestRepresentativeName { get; set; }

		public int BestRepresentativeQuestionCount { get; set; }

		public int BestRepresentativeAnswerCount { get; set; }

		public int WorstRepresentativeID { get; set; }

		public string WorstRepresentativeName { get; set; }

		public int WorstRepresentativeQuestionCount { get; set; }

		public int WorstRepresentativeAnswerCount { get; set; }

	}
}