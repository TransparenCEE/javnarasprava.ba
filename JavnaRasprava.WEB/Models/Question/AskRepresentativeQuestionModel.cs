using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class AskRepresentativeQuestionModel
	{
		public int RepresentativeID { get; set; }

		public string RepresentativeFullName { get; set; }

		public string RepresentativePartyName { get; set; }

        [Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
        public string Text { get; set; }
	}
}