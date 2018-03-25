using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavnaRasprava.WEB.Models.Representative;

namespace JavnaRasprava.WEB.Models
{
	public class RepresentativeListModel
	{
		#region == Properties == 

		public string Title { get; set; }

		public ICollection<ParliamentHouseModel> ParliamentHouses { get; set; }

		public string ParliamentName { get; set; }

		public RepresentativeSearchModel SearchModel { get; set; }


		#endregion
	}
}