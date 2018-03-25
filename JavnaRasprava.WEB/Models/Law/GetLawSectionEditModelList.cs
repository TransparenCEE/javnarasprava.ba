using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawSectionEditModelList
	{
		public int LawID { get; set; }

		public string LawTitle { get; set; }

		public List<LawSectionEditModel> Sections { get; set; }
	}
}