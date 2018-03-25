using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Parliament
{
	public class ParliamentEditModel
	{
		public int? ParliamentID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Parliament_Name", Description = "Parliament_Name_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Name { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Parliament_Code", Description = "Parliament_Code_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Code { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Parliament_RepScreenName", Description = "Parliament_RepScreenName_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string RepresentativesScreenTitle { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Parliament_Order", Description = "Parliament_Order_Description", ResourceType = typeof( GlobalLocalization ) )]
		public int Order { get; set; }

		[Display( Name = "Parliament_IsExclusive", Description = "Parliament_IsExclusive_Description", ResourceType = typeof( GlobalLocalization ) )]
		public bool IsExclusive { get; set; }

		public List<ParliamentHouseEditModel> ParliamentHouses { get; set; }
	}
}