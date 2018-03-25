namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoteDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LawVotes", "Time", c => c.DateTime());
            AddColumn("dbo.LawVotes", "ClientAddress", c => c.String());
            AddColumn("dbo.LawSectionVotes", "Time", c => c.DateTime());
            AddColumn("dbo.LawSectionVotes", "ClientAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LawSectionVotes", "ClientAddress");
            DropColumn("dbo.LawSectionVotes", "Time");
            DropColumn("dbo.LawVotes", "ClientAddress");
            DropColumn("dbo.LawVotes", "Time");
        }
    }
}
