using JavnaRasprava.Resources;
using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Infrastructure.Helpers
{
	public class DemographyHelper
	{
		public static List<SelectListItem> ConvertLocationsToDropdownList( List<Location> locations )
		{
			List<SelectListItem> items = new List<SelectListItem>();
			if ( locations == null )
				return items;

			foreach ( var location in locations )
			{
				items.Add( new SelectListItem
				{
					Text = location.Name,
					Value = location.LocationID.ToString()
				} );
			}

			return items;
		}

		public static List<SelectListItem> ConvertPartiesToDropdownList( List<Party> parties )
		{
			List<SelectListItem> items = new List<SelectListItem>();
			if ( parties == null )
				return items;

			foreach ( var party in parties )
			{
				items.Add( new SelectListItem
				{
					Text = party.Name,
					Value = party.PartyID.ToString()
				} );
			}

			return items;
		}

		public static List<SelectListItem> ConvertEnumToDropdownList( Type type )
		{
			List<SelectListItem> items = new List<SelectListItem>();
			foreach ( var item in Enum.GetValues( type ) )
			{
				items.Add( new SelectListItem
				{
					Text = Enum.GetName( type, item ),
					Value = ( (int)item ).ToString()
				} );
			}

			return items;
		}
	}
}