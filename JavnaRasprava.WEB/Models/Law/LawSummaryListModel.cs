using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawSummaryListModel
	{
		#region == Properties

		public int Count { get; set; }

		public List<LawSummaryModel> Laws { get; set; }

        public string Title { get; set; }
		#endregion
	}
}