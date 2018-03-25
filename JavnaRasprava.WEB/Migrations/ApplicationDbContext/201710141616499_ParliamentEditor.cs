namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParliamentEditor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParliamentHouses", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Parliaments", "Code", c => c.String());
            AddColumn("dbo.Parliaments", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Parliaments", "RepresentativesScreenTitle", c => c.String());
            AddColumn("dbo.Parliaments", "IsExclusive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parliaments", "IsExclusive");
            DropColumn("dbo.Parliaments", "RepresentativesScreenTitle");
            DropColumn("dbo.Parliaments", "Order");
            DropColumn("dbo.Parliaments", "Code");
            DropColumn("dbo.ParliamentHouses", "Order");
        }
    }
}
