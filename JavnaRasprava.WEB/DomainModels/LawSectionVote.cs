using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawSectionVote
	{
		public int LawSectionVoteID { get; set; }

		public bool? Vote { get; set; }

		public DateTime? Time { get; set; }

		public string ClientAddress { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public int LawSectionID { get; set; }
		public virtual LawSection LawSection { get; set; }

		//public int LawID { get; set; }
		//public virtual Law Law { get; set; }

		public int? LawSectionCustomVoteID { get; set; }
		public virtual LawSectionCustomVote LawSectionCustomVote { get; set; }
	}
}