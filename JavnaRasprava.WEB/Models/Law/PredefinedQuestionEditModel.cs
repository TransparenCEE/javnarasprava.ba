using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class PredefinedQuestionEditModel
	{
		public int QuestionID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		public string Text { get; set; }

		[Display( Name = "Global_IsSuggested", ResourceType = typeof( GlobalLocalization ) )]
		public bool IsSuggested { get; set; }

		public DateTime CreateTimeUtc { get; set; }

		[Display( Name = "Global_Verified", ResourceType = typeof( GlobalLocalization ) )]
		public bool Verified { get; set; }

		[Display( Name = "Global_AdminIgnore", ResourceType = typeof( GlobalLocalization ) )]
		public bool AdminIgnore { get; set; }

		public int LawID { get; set; }

		public string LawTitle { get; set; }

		public List<string> AskedRepresentatives { get; set; }

		public bool IsDirectQuestion { get; set; }
	}
}