using JavnaRasprava.WEB.Models.Representative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
    public class LawHomeModel
    {
        public LawSummaryListModel LatestLaws { get; set; }
        public LawSummaryListModel NextLawsInVote { get; set; }
        public LawSummaryListModel MostActive { get; set; }
        public LawSummaryListModel PointedOut { get; set; }

        public LawSectionSummaryModelCollection PointedOutSections { get; set; }

		public LawSearchModel Search { get; set; }

		public TopRepresentativesModel TopRepresentatives { get; set; }
	}
}