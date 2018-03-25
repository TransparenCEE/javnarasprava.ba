using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace JavnaRasprava.WEB.Models.Landing
{
	public class AllSectionsModel
	{
		public IPagedList<LawSectionSummaryModel> Results { get; internal set; }
		public LawSearchModel SearchModel { get; internal set; }
	}
}