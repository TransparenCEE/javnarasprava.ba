using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawSectionSummaryModel
	{
		public int LawSectionID { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string ImageRelativePath { get; set; }

		public int LawID { get; set; }

		public string LawTitle { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }

		public double VotesDownPercentage { get; set; }

		public double VotesUpPercentage { get; set; }

		public string Description { get; set; }

		public DateTime? LawExpetedVotingDayDateTime { get; internal set; }
		public DateTime LawCreateDateTimeUtc { get; internal set; }

		public int LawAskedCount { get; internal set; }
	}
}