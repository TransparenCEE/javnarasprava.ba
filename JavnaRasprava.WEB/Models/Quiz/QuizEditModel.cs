using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizEditModel
	{
		public int QuizId { get; set; }

		[Display( Name = "Quiz_TimeCreated", ResourceType = typeof( GlobalLocalization ) )]
		public DateTime TimeCreated { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Quiz_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[Display( Name = "Quiz_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Description { get; set; }

		public HttpPostedFileBase Image { get; set; }
		public string ImageRelativePath { get; set; }
		public int ParliamentId { get; set; }

		public List<QuizItemSummaryModel> Items { get; set; }
	}
}