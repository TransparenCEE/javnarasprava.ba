namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quiz : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfoBoxItems",
                c => new
                    {
                        InfoBoxItemId = c.Int(nullable: false, identity: true),
                        Reference = c.Int(nullable: false),
                        BoxName = c.String(),
                        Partition = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InfoBoxItemId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        TimeCreated = c.DateTime(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        ParliamentId = c.Int(nullable: false),
                        ImageRelativePath = c.String(),
                    })
                .PrimaryKey(t => t.QuizId)
                .ForeignKey("dbo.Parliaments", t => t.ParliamentId, cascadeDelete: true)
                .Index(t => t.ParliamentId);
            
            CreateTable(
                "dbo.QuizItems",
                c => new
                    {
                        QuizItemId = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        QuizId = c.Int(nullable: false),
                        Law_LawID = c.Int(),
                        LawSection_LawSectionID = c.Int(),
                    })
                .PrimaryKey(t => t.QuizItemId)
                .ForeignKey("dbo.Laws", t => t.Law_LawID)
                .ForeignKey("dbo.LawSections", t => t.LawSection_LawSectionID)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId)
                .Index(t => t.Law_LawID)
                .Index(t => t.LawSection_LawSectionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quizs", "ParliamentId", "dbo.Parliaments");
            DropForeignKey("dbo.QuizItems", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizItems", "LawSection_LawSectionID", "dbo.LawSections");
            DropForeignKey("dbo.QuizItems", "Law_LawID", "dbo.Laws");
            DropIndex("dbo.QuizItems", new[] { "LawSection_LawSectionID" });
            DropIndex("dbo.QuizItems", new[] { "Law_LawID" });
            DropIndex("dbo.QuizItems", new[] { "QuizId" });
            DropIndex("dbo.Quizs", new[] { "ParliamentId" });
            DropTable("dbo.QuizItems");
            DropTable("dbo.Quizs");
            DropTable("dbo.InfoBoxItems");
        }
    }
}
