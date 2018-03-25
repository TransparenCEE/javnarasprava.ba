using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;

namespace JavnaRasprava.WEB.BLL
{
	public class ExpertService
	{
		public int CreateExpert( Expert expert )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				if ( context.Experts
					.Any( x => x.FirstName == expert.FirstName && x.LastName == expert.LastName ) )
					return 0;

				context.Experts.Add( expert );

				context.SaveChanges();

				return expert.ExpertID;
			}
		}

		public Expert Get( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetInternal( id, context );
			}
		}

		

		public List<Expert> GetAll()
		{
			using (var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create())
			{
				return context.Experts.ToList();
			}
		}

		public Expert UpdateExpert(Expert model)
		{
			using(var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create())
			{
				var existing = context.Experts
					.Where( x => x.ExpertID == model.ExpertID )
					.FirstOrDefault();

				if ( existing == null )
					return null;

				existing.About = model.About;
				existing.FirstName = model.FirstName;
				existing.LastName = model.LastName;

				context.SaveChanges();

				return GetInternal( model.ExpertID, context );
			}
		}
		public void Delete( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{

				context.Experts.Where( x => x.ExpertID == id ).Delete();
				context.SaveChanges();
			}
		}

		internal static Expert GetInternal( int id, ApplicationDbContext context )
		{
			return context.Experts.Where( x => x.ExpertID == id ).FirstOrDefault();
		}
	}
}