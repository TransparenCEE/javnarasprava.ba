using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawComment
	{
		public int LawCommentID { get; set; }

        [Required(ErrorMessage="Molimo da upisete komentar") ]
		public string Text { get; set; }

		public DateTime DateTimeUtc { get; set; }

		public int LawID { get; set; }
		public virtual Law Law { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		//public int? ParrentCommentID { get; set; }
		//public virtual LawComment ParrentComment { get; set; }

		public virtual ICollection<LawCommentLike> LawCommentVotes { get; set; }
	}
}