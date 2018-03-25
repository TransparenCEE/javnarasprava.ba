using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawSectionEditModel
	{
		public int LawSectionID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Title", ResourceType = typeof( GlobalLocalization ) )]
		[System.Web.Mvc.AllowHtml]
		public string Title { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		[System.Web.Mvc.AllowHtml]
		public string Text { get; set; }

		[Display( Name = "Section_Description", ResourceType = typeof( GlobalLocalization ) )]
		[System.Web.Mvc.AllowHtml]
		public string Description { get; set; }

		public int LawID { get; set; }

		public DateTime? PointedOutUtc { get; set; }

		public HttpPostedFileBase Image { get; set; }

		public string ImageRelativePath { get; set; }

		public List<LawSectionCustomVoteModel> LawSectionVotes { get; set; }

	}
}