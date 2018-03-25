using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Parliament
{
	public class ParliamentHouseEditModel
	{
		public int ParliamentHouseID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "ParliamentHouse_Name", ResourceType = typeof( GlobalLocalization ) )]
		public string Name { get; set; }

		public int ParliamentID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "ParliamentHouse_Order", Description = "ParliamentHouse_Order_Description", ResourceType = typeof( GlobalLocalization ) )]
		public int Order { get; set; }
	}
}