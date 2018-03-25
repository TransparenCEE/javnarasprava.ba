using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Representative
{
	public class TopRepresentativesModel
	{
		public List<RepresentativeModel> MostActive { get; set; }

		public List<RepresentativeModel> MostInactive { get; set; }
	}
}