using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class LawVoteModel
	{
		public int LawId { get; set; }

		public int QuizId { get; set; }

		public int? NextQuestionId { get; set; }

		public int lawVote { get; set; }

		public string customVoteAnswer { get; set; }


	}
}