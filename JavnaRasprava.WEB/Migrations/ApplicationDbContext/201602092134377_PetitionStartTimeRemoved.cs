namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetitionStartTimeRemoved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Petitions", "CreateTime", c => c.DateTime(nullable: true ));
			Sql( "update dbo.Petitions set CreateTime = StartTime" );
			AlterColumn( "dbo.Petitions", "CreateTime", c => c.DateTime( nullable: false ) );
			DropColumn("dbo.Petitions", "StartTime");
            DropColumn("dbo.Petitions", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Petitions", "EndTime", c => c.DateTime());
            AddColumn("dbo.Petitions", "StartTime", c => c.DateTime());
            DropColumn("dbo.Petitions", "CreateTime");
        }
    }
}
