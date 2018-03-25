using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class ExternalLink
	{
		public int ExternalLinkID { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }

		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }
	}
}