using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.User
{
	public class UserSearchModel
	{
		public string QueryString { get; set; }

		public string Order { get; set; }

		public string SortBy { get; set; }

		public int? Page { get; set; }

		public int PageItemCount { get; set; }
	}
}