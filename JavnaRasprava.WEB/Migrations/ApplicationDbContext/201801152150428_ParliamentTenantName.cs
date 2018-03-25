namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParliamentTenantName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parliaments", "Tenant", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parliaments", "Tenant");
        }
    }
}
