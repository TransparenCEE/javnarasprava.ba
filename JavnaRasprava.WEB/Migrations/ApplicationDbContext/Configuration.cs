namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
	using JavnaRasprava.WEB.DomainModels;
	using JavnaRasprava.WEB.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<JavnaRasprava.WEB.DomainModels.ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
			MigrationsDirectory = @"Migrations\ApplicationDbContext";
		}

		protected override void Seed( JavnaRasprava.WEB.DomainModels.ApplicationDbContext context )
		{

			//if ( System.Diagnostics.Debugger.IsAttached == false )
			//	System.Diagnostics.Debugger.Launch();

			if ( System.Configuration.ConfigurationManager.AppSettings["RunSeed"] == "true" )
				Helper.Seed( context );

			

		}

		
	}
}
