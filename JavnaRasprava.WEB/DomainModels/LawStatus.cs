using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawStatus
	{
		public int LawStatusID { get; set; }

		public string Name { get; set; }

		public string StatusDescription { get; set; }
	}
}