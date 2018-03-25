using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class PartyModel
	{
		public int PartyID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Party_Name", ResourceType = typeof( GlobalLocalization ) )]
		public string Name { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Party_FullName", ResourceType = typeof( GlobalLocalization ) )]
		public string FullName { get; set; }
	}
}