using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class RepresentativeAssignment
	{
		public int RepresentativeAssignmentID { get; set; }

		public string Title { get; set; }

		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }
	}
}