using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.News
{
	public class NewsListModel
	{
		public IPagedList<NewsModel> Results { get; set; }
		public NewsSearchModel SearchModel { get; set; }
	}
}