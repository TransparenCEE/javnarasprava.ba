using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class InfoBoxItem
	{
		public int InfoBoxItemId { get; set; }

		public int Reference { get; set; }

		public string BoxName { get; set; }

		public int Partition { get; set; }

		public InfoBoxItemType Type { get; set; }

		public int Position { get; set; }
	}
}