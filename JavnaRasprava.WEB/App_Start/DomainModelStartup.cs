using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Migrations.ApplicationDbContext;

namespace JavnaRasprava.WEB
{
	public class DomainModelStartup
	{
		public static void Initialize()
		{
			Database.SetInitializer( new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>() );
			AutoMapper.Mapper.CreateMap<Models.PetitionModel, DomainModels.Petition>();
			AutoMapper.Mapper.CreateMap<DomainModels.Petition, Models.PetitionModel>();

			AutoMapper.Mapper.CreateMap<DomainModels.Parliament, Models.Parliament.ParliamentEditModel>();
			AutoMapper.Mapper.CreateMap<Models.Parliament.ParliamentEditModel, DomainModels.Parliament>();

			AutoMapper.Mapper.CreateMap<DomainModels.ParliamentHouse, Models.Parliament.ParliamentHouseEditModel>();
			AutoMapper.Mapper.CreateMap<Models.Parliament.ParliamentHouseEditModel, DomainModels.ParliamentHouse>();

			//Database.SetInitializer( new DropCreateDatabaseAlways<ApplicationDbContext>() );
		}
	}
}