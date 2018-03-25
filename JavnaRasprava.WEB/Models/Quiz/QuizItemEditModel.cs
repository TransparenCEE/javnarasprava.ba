using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizItemEditModel
	{
		public int QuizItemId { get; set; }

		public int QuizId { get; set; }

		public string QuizTitle { get; set; }

		public int Order { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "QuizItem_LawTitle", ResourceType = typeof( GlobalLocalization ) )]
		public int LawId { get; set; }

		public List<System.Web.Mvc.SelectListItem> Laws { get; set; }

		[Display( Name = "QuizItem_SectionTitle", ResourceType = typeof( GlobalLocalization ) )]
		public int? LawSectionId { get; set; }

		public List<System.Web.Mvc.SelectListItem> LawSections { get; set; }
	}
}