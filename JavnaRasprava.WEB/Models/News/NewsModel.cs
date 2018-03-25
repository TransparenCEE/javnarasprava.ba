using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.News
{
	public class NewsModel
	{
		public int NewsId { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "News_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "News_Summary", ResourceType = typeof( GlobalLocalization ) )]
		public string Summary { get; set; }

		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "News_Text", ResourceType = typeof( GlobalLocalization ) )]
		public string Text { get; set; }

		public HttpPostedFileBase Image { get; set; }
		public string ImageRelativePath { get; set; }

		public int ParliamentId { get; set; }
		public DateTime? CreateDateTimeUtc { get; internal set; }
	}
}