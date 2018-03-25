using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class AnswerToken
	{
		public int AnswerTokenID { get; set; }

		public int QuestionID { get; set; }
		public virtual Question Question { get; set; }

		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }

		public Guid Token { get; set; }

		public bool Processed { get; set; }

		public DateTime CreateTimeUtc { get; set; }

		public int? AnswerID { get; set; }
		public Answer Answer { get; set; }

        public virtual ICollection<QuestionMessage> Messages { get; set; }
    }
}