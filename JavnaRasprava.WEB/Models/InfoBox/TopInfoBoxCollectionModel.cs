using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.InfoBox
{
	public class TopInfoBoxCollectionModel
	{
		public string Title { get; set; }

		public List<TopInfoBoxItemModel> Items { get; set; }

	}
}