using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.Parliament;
using JavnaRasprava.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Runtime.Caching;

namespace JavnaRasprava.WEB.BLL
{
	public class ParliamentService
	{
		#region == Fields ==


		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public int GetParliamentId( string pCode )
		{
			var parliament = GetAllParliamentsCached().SingleOrDefault( x => x.Code.Equals( pCode, StringComparison.InvariantCultureIgnoreCase ) );
			if ( parliament == null )
			{
				parliament = GetDefaultParliament();
			}
			return parliament.ParliamentID;
		}

		internal void GetTenantData( string requestedSubDomain, out string tenantSubDomain, out string pCode )
		{
			requestedSubDomain = requestedSubDomain ?? "www";
			var parliament = GetAllParliamentsCached().SingleOrDefault( x => x.TenantSubDomain.Equals( requestedSubDomain, StringComparison.InvariantCultureIgnoreCase ) && x.IsExclusive );

			if ( parliament == null )
			{
				parliament = GetDefaultParliament();
			}

			tenantSubDomain = parliament.TenantSubDomain;
			pCode = parliament.Code;
		}

		public void GetTenantData( ref string pCode, out string tenantName, out int parliamentId, out string tenantSubDomain )
		{
			string code = pCode;
			Parliament parliament = null;
			parliament = GetAllParliamentsCached().SingleOrDefault( x => x.Code.Equals( code, StringComparison.InvariantCultureIgnoreCase ) );

			if ( parliament == null )
			{
				parliament = GetDefaultParliament();
			}

			pCode = parliament.Code;
			tenantName = parliament.Tenant;
			parliamentId = parliament.ParliamentID;
			tenantSubDomain = parliament.TenantSubDomain;
		}

		public Parliament GetDefaultParliament()
		{
			return GetAllParliamentsCached().SingleOrDefault( x => x.TenantSubDomain.Equals( "www", StringComparison.InvariantCultureIgnoreCase ) && x.IsExclusive );
		}

		public List<Parliament> GetAllParliamentsCached()
		{
			var result = MemoryCache.Default.AddOrGetExisting<List<Parliament>>( "Parliaments", () => { return GetAllParliaments(); },
				new CacheItemPolicy
				{
					AbsoluteExpiration = DateTimeOffset.Now.AddHours( 1 ),
				} );

			return result;
		}


		public List<Parliament> GetAllParliaments()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliaments = context.Parliaments.ToList();

				return parliaments;
			}

		}

		public void ClearCache()
		{
			MemoryCache.Default.Remove( "Parliaments" );
		}

		public List<ParliamentEditModel> GetAllParliamentEditModels()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliaments = context.Parliaments
					.Include( x => x.ParliamentHouses )
					.ToList()
					.Select( x => AutoMapper.Mapper.Map<ParliamentEditModel>( x ) )
					.ToList();

				return parliaments;
			}
		}

		internal void GetTenantData( int parliamentID, out string parliamentCode )
		{
			var parliament = GetAllParliamentsCached().SingleOrDefault( x => x.ParliamentID == parliamentID );
			if ( parliament == null )
			{
				parliament = GetDefaultParliament();
			}

			parliamentCode = parliament.Code;
		}

		public ParliamentEditModel GetParliamentEditModel( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = context.Parliaments.Include( x => x.ParliamentHouses ).SingleOrDefault( x => x.ParliamentID == id );

				if ( parliament == null )
					return null;

				var result = AutoMapper.Mapper.Map<ParliamentEditModel>( parliament );
				return result;
			}
		}

		public void CreateParliament( ParliamentEditModel parliamentModel )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = AutoMapper.Mapper.Map<Parliament>( parliamentModel );

				context.Parliaments.Add( parliament );

				context.SaveChanges();
			}
		}

		public void UpdateParliament( int id, ParliamentEditModel parliamentModel )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = context.Parliaments.SingleOrDefault( x => x.ParliamentID == id );

				if ( parliament == null )
					return;

				AutoMapper.Mapper.Map( parliamentModel, parliament, typeof( ParliamentEditModel ), typeof( Parliament ) );

				context.SaveChanges();
			}
		}

		public void DeleteParliament( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = context.Parliaments.SingleOrDefault( x => x.ParliamentID == id );

				if ( parliament == null )
					return;

				context.Parliaments.Remove( parliament );

				context.SaveChanges();
			}
		}

		public ParliamentListModel GetParliaments()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new ParliamentListModel
				{
					Parliaments = context.Parliaments.Select( x => x ).ToList()
				};

				return result;
			}
		}

		internal object InitializeParliamentEditModel( int currentParliamentId )
		{
			return new ParliamentEditModel();
		}

		internal ParliamentHouseEditModel InitializeEditParliamentHouseModel( int parliamentId )
		{
			return new ParliamentHouseEditModel()
			{
				ParliamentID = parliamentId
			};
		}

		internal ParliamentHouseEditModel InitializeEditParliamentHouseModel( int parliamentId, int parliamentHouseId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliamentHouse = context.ParliamentHouses.SingleOrDefault( x => x.ParliamentHouseID == parliamentHouseId );

				if ( parliamentHouse == null )
					return null;

				var result = AutoMapper.Mapper.Map<ParliamentHouseEditModel>( parliamentHouse );

				return result;
			}
		}

		internal void CreateNewParliamentHouseModel( ParliamentHouseEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliamentHouse = AutoMapper.Mapper.Map<ParliamentHouse>( model );

				context.ParliamentHouses.Add( parliamentHouse );

				context.SaveChanges();
			}
		}

		internal void UpdateParliamentHouse( int id, ParliamentHouseEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = context.ParliamentHouses.SingleOrDefault( x => x.ParliamentHouseID == id );

				if ( parliament == null )
					return;

				AutoMapper.Mapper.Map( model, parliament, typeof( ParliamentHouseEditModel ), typeof( ParliamentHouse ) );

				context.SaveChanges();
			}
		}

		internal int DeleteParliamentHouse( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliamentHouse = context.ParliamentHouses.SingleOrDefault( x => x.ParliamentHouseID == id );

				if ( parliamentHouse == null )
					return 0;

				context.ParliamentHouses.Remove( parliamentHouse );

				context.SaveChanges();

				return parliamentHouse.ParliamentID;
			}
		}

		#endregion
	}
}