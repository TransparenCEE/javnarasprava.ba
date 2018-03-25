using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models
{
	public class RepresentativeExternalLinkModel
	{
		public int ExternalLinkID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Description { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Url", ResourceType = typeof( GlobalLocalization ) )]
		[Url]
		public string Url { get; set; }
	}
}