using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.News
{
	public class NewsSearchModel
	{
		public string QueryString { get; set; }

		public int? ParliamentId { get; set; }

		public NewsSort? Sort { get; set; }

		public int? page { get; set; }

		public int PageItemCount { get; set; }
	}
}