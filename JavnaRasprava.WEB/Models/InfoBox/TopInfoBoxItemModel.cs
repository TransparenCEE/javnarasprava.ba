using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.InfoBox
{
	public class TopInfoBoxItemModel
	{
		public object Item { get; private set; }
		public InfoBoxItemType Type { get; private set; }

		public TopInfoBoxItemModel( object item, InfoBoxItemType type )
		{
			Item = item;
			Type = type;
		}
	}
}