using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizItemResultsModel
	{
		public bool? UserVote { get; set; }
		public string CustomUserVoteText { get; set; }

		public DomainModels.QuizItemType? QuestionType { get; set; }

		public int LawId { get; set; }

		public int? SectionId { get; set; }

		public string ImageRelativePath { get; set; }

		[Display( Name = "Quiz_LawTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string LawTitle { get; set; }

		[Display( Name = "Quiz_SectionTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string SectionTitle { get; set; }

		[Display( Name = "Quiz_QuestionDescription", ResourceType = typeof( GlobalLocalization ) )]
		public string QuestionDescription { get; set; }


		public double VotesUpPercentage { get; set; }

		public double VotesDownPercentage { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }
	}
}