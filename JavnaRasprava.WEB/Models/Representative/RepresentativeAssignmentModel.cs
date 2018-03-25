using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Representative
{
	public class RepresentativeAssignmentModel
	{
		public int RepresentativeAssignmentID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Name", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		public int RepresentativeID { get; set; }
	}
}