namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class petitionprogresscustom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetitionProgresses", "NumberOfVotes", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PetitionProgresses", "NumberOfVotes");
        }
    }
}
