using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Petition
{
	public class SearchPetitionModel
	{
		public int QueryString { get; set; }

		public bool OnlyActive { get; set; }

		public string SortOrder { get; set; }

		public int? PageNumber { get; set; }
	}
}