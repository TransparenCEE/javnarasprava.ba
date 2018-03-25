using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawSectionCustomVote
	{
		public int LawSectionCustomVoteID { get; set; }

		public string Text { get; set; }

		public string Description { get; set; }

		public bool? Vote { get; set; }

        public bool AdminIgnore { get; set; }

		public bool IsSuggested { get; set; }

		public int LawSectionID { get; set; }

		public virtual LawSection LawSection { get; set; }

		public virtual ICollection<LawSectionVote> Votes { get; set; }
	}
}