using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawStatModel
	{
		public int LawId { get; set; }

		public int TotalQuestions { get; set; }

		public int MostActiveLawId { get; set; }
		public string MostActiveLawTitle { get; set; }

		public int MostActiveRepresentativeId { get; set; }
		public string MostActiveRepresentativeName { get; set; }


		public int MostActiveRepresentativeCount { get; set; }

		public int MostActiveLawCount { get; set; }
	}
}