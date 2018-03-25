using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace JavnaRasprava.WEB.Models
{
	public class AllLawsModel
	{
		public IPagedList<LawSummaryModel> Results { get; internal set; }
		public LawSearchModel SearchModel { get; internal set; }
	}
}