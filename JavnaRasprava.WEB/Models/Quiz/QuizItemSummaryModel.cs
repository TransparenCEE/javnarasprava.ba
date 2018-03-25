using JavnaRasprava.Resources;
using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizItemSummaryModel
	{
		public int QuizItemId { get; set; }

		public int QuizId { get; set; }

		public QuizItemType Type { get; set; }

		public int LawId { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "QuizItem_LawTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string LawTitle { get; set; }

		public int? SectionId { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "QuizItem_SectionTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string SectionTitle { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "QuizItem_Order", ResourceType = typeof( GlobalLocalization ) )]
		public int Order { get; internal set; }
	}
}