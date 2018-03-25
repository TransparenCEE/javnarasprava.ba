namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subdomain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Parliaments", "TenantSubDomain", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parliaments", "TenantSubDomain");
        }
    }
}
