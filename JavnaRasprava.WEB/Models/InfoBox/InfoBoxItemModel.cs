using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.InfoBox
{
	public class InfoBoxItemModel
	{
		public int InfoBoxItemId { get; set; }

		public int Reference { get; set; }

		public int Partition { get; set; }

		public string BoxName { get; set; }

		public InfoBoxItemType Type { get; set; }

		public int Position { get; set; }

		public List<System.Web.Mvc.SelectListItem> Positions{ get; set; }

	}
}