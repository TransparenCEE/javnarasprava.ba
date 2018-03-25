using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawRepresentativeAssociation
	{
		public int LawRepresentativeAssociationID { get; set; }

		public string Reason { get; set; }
		
		public int RepresentativeID { get; set; }
		public virtual Representative Representative { get; set; }

		public int LawID { get; set; }
		public virtual Law Law { get; set; }
	}
}