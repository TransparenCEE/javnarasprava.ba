using System;
using System.Collections.Generic;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Answer
	{
		public int AnswerID { get; set; }

		public int QuestionID { get; set; }
		public virtual Question Question { get; set; }

		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }

		public string Text { get; set; }

		public DateTime AnsweredTimeUtc { get; set; }

		public virtual ICollection<AnswerLike> Likes { get; set; }
	}
}