using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class WeeklyReportModel
	{
		public List<LawModel> Laws { get; set; }

		public List<QuestionModel> Questions { get; set; }

		public int RepresentativeId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string BaseUrl { get; set; }

		public int TotalLawQuestions { get { return Laws.SelectMany( x => x.Questions ).Count(); } }

		public int TotalDirectQuestions { get { return Questions.Count; } }
	}
}