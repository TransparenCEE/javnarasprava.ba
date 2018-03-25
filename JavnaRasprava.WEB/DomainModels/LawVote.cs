using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawVote
	{
		public int LawVoteID { get; set; }

		public bool? Vote { get; set; }

		public DateTime? Time { get; set; }

		public string ClientAddress { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public int? LawCustomVoteID { get; set; }
		public virtual LawCustomVote LawCustomVote { get; set; }

		public int LawID { get; set; }
		public virtual Law Law { get; set; }

	}
}