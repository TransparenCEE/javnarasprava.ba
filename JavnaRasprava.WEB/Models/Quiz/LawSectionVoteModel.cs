using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class LawSectionVoteModel
	{
		public int SectionId { get; set; }

		public int QuizId { get; set; }

		public int? NextQuestionId { get; set; }

		public int sectionVote { get; set; }

		public string customSectionVoteAnswer { get; set; }
	}
}