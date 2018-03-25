using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models
{
	public class ParliamentHouseModel
	{
		#region == Properties ==

		public int ParliamentHouseID { get; set; }

		public string Name { get; set; }

		public List<RepresentativeModel> Representatives { get; set; }

		#endregion
	}
}
