using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Models.Law
{
	public class CreateExpertcommentModel
	{
		public int LawID { get; set; }

        [Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
        [Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		[UIHint( "tinymce_jquery_min" ), AllowHtml]
		public string Text { get; set; }

        [Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
        [Display( Name = "Global_Expert", ResourceType = typeof( GlobalLocalization ) )]
        public int ExpertID { get; set; }
		public List<ExpertSummaryModel> Experts { get; set; }
	}
}