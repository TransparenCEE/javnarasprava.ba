using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class CreateLawRepresentativeModel
	{
		public int LawID { get; set; }
		public int ParliamentID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Representative_Reason", ResourceType = typeof( GlobalLocalization ) )]
		public string Reason { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Representative", ResourceType = typeof( GlobalLocalization ) )]
		public int RepresentativeID { get; set; }

		public List<RepresentativeSummaryModel> Representatives { get; set; }
	}
}