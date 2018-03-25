using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Expert
	{
		public int ExpertID { get; set; }

		[Display( Name = "Global_FirstName", ResourceType = typeof( GlobalLocalization ) )]
		public string FirstName { get; set; }

		[Display( Name = "Global_LastName", ResourceType = typeof( GlobalLocalization ) )]
		public string LastName { get; set; }

		[Display( Name = "Global_About", ResourceType = typeof( GlobalLocalization ) )]
		public string About { get; set; }
	}
}