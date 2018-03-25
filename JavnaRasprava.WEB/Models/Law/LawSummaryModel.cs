using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models
{
	public class LawSummaryModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public DateTime CreateDateTimeUtc { get; set; }

		public string ExpetedVotingDay
		{
			get
			{
				return ExpetedVotingDayDateTime.HasValue ? ExpetedVotingDayDateTime.Value.ToString( "dd.MM.yyyy." ) : GlobalLocalization.UnknownExpectedVotingDay;
			}
		}

		public string ImageRelativePath { get; set; }

		public int AskedCount { get; set; }

		public int AnswersCount { get; set; }

		public int VotesUp { get; set; }

		public double VotesUpPercentage { get; set; }

		public int VotesDown { get; set; }

		public double VotesDownPercentage { get; set; }

		public IEnumerable<LawSectionSummaryModel> Sections { get; set; }

		public DateTime? ExpetedVotingDayDateTime { get; set; }
	}
}
