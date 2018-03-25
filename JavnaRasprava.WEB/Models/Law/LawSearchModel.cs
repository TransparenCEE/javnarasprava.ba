using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Models
{
	public class LawSearchModel
	{
		public string QueryString { get; set; }

		public int? ParliamentId { get; set; }
		public List<SelectListItem> Parliaments { get; set; }

		public int? CategoryId { get; set; }
		public List<SelectListItem> Categories { get; set; }

		public LawSort? LawSort { get; set; }

		public string Order { get; set; }
		
		public string SortBy { get; set; }

		public int? page { get; set; }

		public int PageItemCount { get; set; }
	}
}