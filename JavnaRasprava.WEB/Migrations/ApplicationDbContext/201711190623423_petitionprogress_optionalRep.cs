namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class petitionprogress_optionalRep : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PetitionProgresses", new[] { "RepresentativeID" });
            AlterColumn("dbo.PetitionProgresses", "RepresentativeID", c => c.Int());
            CreateIndex("dbo.PetitionProgresses", "RepresentativeID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PetitionProgresses", new[] { "RepresentativeID" });
            AlterColumn("dbo.PetitionProgresses", "RepresentativeID", c => c.Int(nullable: false));
            CreateIndex("dbo.PetitionProgresses", "RepresentativeID");
        }
    }
}
