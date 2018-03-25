using System;

namespace JavnaRasprava.WEB.DomainModels
{
	public class UserRepresentativeQuestion
	{
		public int UserRepresentativeQuestionID { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public int QuestionID { get; set; }
		public virtual Question Question { get; set; }

		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }

		public DateTime CreateTimeUtc { get; set; }

		public bool Processed { get; set; }

		public int? AnswerTokenID { get; set; }
		public AnswerToken AnswerToken { get; set; }
	}
}