using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizListModel
	{
		public IPagedList<QuizSummaryModel> Results { get; set; }
		public QuizSearchModel SearchModel { get; set; }
	}
}