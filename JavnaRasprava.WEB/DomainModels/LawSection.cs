using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawSection
	{
		public int LawSectionID { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string Description { get; set; }

		public DateTime? PointedOutUtc { get; set; }

		public string ImageRelativePath { get; set; }

		public virtual Law Law { get; set; }
		public int LawID { get; set; }

		public virtual ICollection<LawSectionVote> LawSectionVotes { get; set; }

		public virtual ICollection<LawSectionCustomVote> CustomVotes { get; set; }
		
	}
}