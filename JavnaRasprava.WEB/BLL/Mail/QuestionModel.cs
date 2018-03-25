using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class QuestionModel
	{
		public int QuestionId { get; set; }

		public string QuestionText { get; set; }

		public DateTime AskedOn { get; set; }

		public Guid AnswerToken { get; set; }

		public int AskedCount { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }
	}
}