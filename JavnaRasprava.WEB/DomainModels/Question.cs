using System;
using System.Collections.Generic;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Question
	{
		public int QuestionID { get; set; }

		public string Text { get; set; }

		public bool IsSuggested { get; set; }

		public DateTime CreateTimeUtc { get; set; }

		public bool Verified { get; set; }

		public bool AdminIgnore { get; set; }

		public int? LawID { get; set; }
		public virtual Law Law { get; set; }

		public virtual ICollection<UserRepresentativeQuestion> UserRepresentativeQuestions { get; set; }

		public virtual ICollection<Answer> Answers { get; set; }

		public virtual ICollection<QuestionLike> Likes { get; set; }

	}
}