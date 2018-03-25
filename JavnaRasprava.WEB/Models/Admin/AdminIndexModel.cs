using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Admin
{
    public class AdminIndexModel
    {
		public int UnverifiedCustomVotesCount { get; internal set; }
		public int UnverifiedLawSectionCustomVoteCount { get; internal set; }
		public int UnverifiedQuestionsCount { get; internal set; }
		public int UnverifiedRepresentativeQuestionsCount { get; internal set; }
		public int UnverifiedPetitionsCount { get; internal set; }
	}
}