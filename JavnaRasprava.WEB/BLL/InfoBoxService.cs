using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Models.InfoBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL
{
	public class InfoBoxService
	{
		public List<InfoBoxItem> GetItemsForInfoBox( string boxName, int partition )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetItemsForInfoBox( boxName, partition, context );
			}
		}

		private static List<InfoBoxItem> GetItemsForInfoBox( string boxName, int partition, ApplicationDbContext context )
		{
			return context.InfoBoxItems
								.Where( x => x.BoxName == boxName )
								.Where( x => x.Partition == partition )
								.OrderBy( x => x.Position )
								.ToList();
		}

		public InfoBoxItemModel InitInfoBoxItemModelForBox( string boxName )
		{
			// < add key = "JavnaRasprava.Index.PointedOutSections" value = "4" />
			// < add key = "JavnaRasprava.Index.PointedOut" value = "4" />

			var maxItems = AppConfig.GetIntValue( string.Format( "JavnaRasprava.Index.{0}", boxName ), 4 );

			var result = new InfoBoxItemModel
			{
				Positions = new List<System.Web.Mvc.SelectListItem>()
			};

			for ( int i = 1; i <= maxItems; i++ )
			{
				result.Positions.Add( new System.Web.Mvc.SelectListItem { Value = i.ToString(), Text = "Pozicija:" + i.ToString() } );
			}

			return result;
		}

		public InfoBoxItemModel GetInfoBox( string boxName, int reference, int partition, InfoBoxItemType type )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetInfoBoxInternl( boxName, reference, partition, type, context );
			}
		}

		private InfoBoxItemModel GetInfoBoxInternl( string boxName, int reference, int partition, InfoBoxItemType type, ApplicationDbContext context )
		{
			var item = context.InfoBoxItems
								.Where( x => x.BoxName == boxName )
								.Where( x => x.Reference == reference )
								.Where( x => x.Type == type )
								.Where( x => x.Partition == partition )
								.FirstOrDefault();

			var result = InitInfoBoxItemModelForBox( boxName );

			result.Reference = reference;
			result.Type = type;
			result.Partition = partition;
			result.BoxName = boxName;

			if ( item == null )
				return result;

			// This is the only value we are getting.
			result.Position = item.Position;

			return result;
		}

		internal object UpdateInfoBox( string boxName, int reference, int partition, InfoBoxItemType type, int position )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var item = context.InfoBoxItems
								.Where( x => x.BoxName == boxName )
								.Where( x => x.Reference == reference )
								.Where( x => x.Type == type )
								.Where( x => x.Partition == partition )
								.FirstOrDefault();

				var result = InitInfoBoxItemModelForBox( boxName );

				result.Reference = reference;
				result.Type = type;
				result.Partition = partition;
				result.BoxName = boxName;

				if ( item == null )
				{
					item = new InfoBoxItem
					{
						Reference = reference,
						Type = type,
						Partition = partition,
						BoxName = boxName
					};
					context.InfoBoxItems.Add( item );

					var existingItem = context.InfoBoxItems
								.Where( x => x.BoxName == boxName )
								.Where( x => x.Position == position )
								.Where( x => x.Partition == partition )
								.FirstOrDefault();
					if ( existingItem != null )
					{
						context.InfoBoxItems.Remove( existingItem );
					}
				}

				// This is the only value we are getting.
				result.Position = position;
				item.Position = position;

				context.SaveChanges();
				return result;
			}
		}

		internal void DeleteItems( int reference, InfoBoxItemType type, ApplicationDbContext context )
		{
			context.InfoBoxItems
				.Where( x => x.Reference == reference && x.Type == type )
				.Select( x => x )
				.ToList()
				.ForEach( x => context.InfoBoxItems.Remove( x ) );
		}
	}
}