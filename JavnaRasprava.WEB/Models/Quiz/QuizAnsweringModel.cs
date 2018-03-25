using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizAnsweringModel
	{
		public int QuizId { get; set; }

		public DateTime TimeCreated { get; set; }

		[Display( Name = "Quiz_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[Display( Name = "Quiz_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Description { get; set; }

		public int? NextQuestionId { get; set; }

		public DomainModels.QuizItemType? QuestionType { get; set; }

		public int? UserVoteId { get; set; }
		public string CustomUserVoteText { get; set; }

		public int LawId { get; set; }

		public int? SectionId { get; set; }

		public string ImageRelativePath { get; set; }

		[Display( Name = "Quiz_LawTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string LawTitle { get; set; }

		[Display( Name = "Quiz_SectionTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string SectionTitle { get; set; }

		[Display( Name = "Quiz_QuestionDescription", ResourceType = typeof( GlobalLocalization ) )]
		public string QuestionDescription { get; set; }

		public Models.LawCustomVoteListModel LawVotes { get; set; }

		public Models.LawSectionCustomVoteListModel SectionVotes { get; set; }
		public int? CurrentItemIndex { get; internal set; }
		public int TotalItems { get; internal set; }
		public string ProgressPercentage { get; internal set; }

		public string VotingDisabledAttribute()
		{
			if ( UserVoteId.HasValue )
				return "disabled";

			return "";
		}
	}
}