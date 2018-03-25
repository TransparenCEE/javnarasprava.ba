using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL
{
	public class PartyService
	{
		public Models.PartyModel GetParty( int id )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetPartyInternal( id, context );
			}
		}

        public Models.PartyModel CreateParty( Models.PartyModel model )
        {
            using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
            {
                var party = new DomainModels.Party();
                party.FullName = model.FullName;
                party.Name = model.Name;

                context.Parties.Add( party );
                context.SaveChanges();

                return GetPartyInternal( model.PartyID, context );
            }
        }

        public Models.PartyModel UpdateParty( Models.PartyModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var party =  context.Parties
					.Where( x => x.PartyID == model.PartyID )
					.FirstOrDefault();

				if ( party == null )
					return null;

				party.FullName = model.FullName;
				party.Name = model.Name;

				context.SaveChanges();

				return GetPartyInternal( model.PartyID, context );
			}
		}

		public bool DeleteParty( int partyID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var party =  context.Parties
					.Where( x => x.PartyID == partyID )
					.FirstOrDefault();

				if ( party == null )
					return false;

				context.Parties.Remove( party );

				context.SaveChanges();
				return true;
			}
		}

		public List<Models.PartyModel> GetAllParties()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetAllPartiesInternal( context );
			}
		}



		internal static List<Models.PartyModel> GetAllPartiesInternal( DomainModels.ApplicationDbContext context )
		{
			return context.Parties
									.Select( x => new Models.PartyModel { PartyID = x.PartyID, FullName = x.FullName, Name = x.Name } )
									.ToList();
		}

		internal static Models.PartyModel GetPartyInternal( int id, DomainModels.ApplicationDbContext context )
		{
			return context.Parties
							.Where( x => x.PartyID == id )
							.Select( x => new Models.PartyModel { PartyID = x.PartyID, Name = x.Name, FullName = x.FullName } )
							.FirstOrDefault();
		}
	}
}