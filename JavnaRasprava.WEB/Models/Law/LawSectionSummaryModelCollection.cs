using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawSectionSummaryModelCollection
	{
		public int Count { get; set; }

		public List<LawSectionSummaryModel> Sections { get; set; }

		public string Title { get; set; }
	}
}