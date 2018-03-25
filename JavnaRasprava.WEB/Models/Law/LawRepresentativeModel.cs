using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawRepresentativeModel
	{
		public int LawRepresentativeAssociationID { get; set; }

		[Display( Name = "Representative_Reason", ResourceType = typeof( GlobalLocalization ) )]
		public string Reason { get; set; }

		public int RepresentativeID { get; set; }

		[Display( Name = "Global_FirstName", ResourceType = typeof( GlobalLocalization ) )]
		public string FirstName { get; set; }

		[Display( Name = "Global_LastName", ResourceType = typeof( GlobalLocalization ) )]
		public string LastName { get; set; }

		public string PartyName { get; set; }
	}
}
