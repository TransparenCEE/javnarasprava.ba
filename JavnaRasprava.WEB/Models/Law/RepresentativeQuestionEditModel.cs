using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class RepresentativeQuestionEditModel
	{
		public int QuestionID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		public string Text { get; set; }

		public DateTime CreateTimeUtc { get; set; }

		[Display( Name = "Global_Verified", ResourceType = typeof( GlobalLocalization ) )]
		public bool Verified { get; set; }

		[Display( Name = "Global_AdminIgnore", ResourceType = typeof( GlobalLocalization ) )]
		public bool AdminIgnore { get; set; }

		public int RepresentativeId { get; set; }

		public string RperesentativeName { get; set; }

		public string ParliamentName { get; set; }

		public int ParliamentId { get; set; }
	}
}