using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavnaRasprava.WEB.Models.Representative;

namespace JavnaRasprava.WEB.Models
{
	public class RepresentativeModel
	{
		#region == Properties ==

		public JavnaRasprava.WEB.DomainModels.Representative Representative { get; set; }

		public int TotalQuestions { get; set; }

		public int TotalAnswers { get; set; }

		public double PercentageAnswered { get; set; }

		public bool Green { get; set; }

		public bool Yellow { get; set; }

		public bool Red { get; set; }

		public bool Gray { get; set; }

		public List<RepresentativeLawModel> Laws { get; set; }

		public List<RepresentativeQuestionModel> Questions { get; set; }
		public bool IsSingleHouseParliament { get; internal set; }

		#endregion

	}
}