namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionMessages",
                c => new
                    {
                        QuestionMessageId = c.Int(nullable: false, identity: true),
                        MessageCreatedTime = c.DateTime(nullable: false),
                        MessageSentTime = c.DateTime(nullable: false),
                        Recipients = c.String(),
                        Subject = c.String(),
                        AnswerTokenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionMessageId)
                .ForeignKey("dbo.AnswerTokens", t => t.AnswerTokenId, cascadeDelete: true)
                .Index(t => t.AnswerTokenId);

            Sql( @"Insert into QuestionMessages ([MessageCreatedTime], [MessageSentTime], [Recipients], [Subject], [AnswerTokenId])
                    select at.CreateTimeUtc, at.CreateTimeUtc, 'N/A', 'N/A', at.AnswerTokenID
                    from AnswerTokens at
                    left join QuestionMessages qm on at.AnswerID = qm.AnswerTokenId
                    where ( qm.AnswerTokenId is null or qm.Recipients <> 'N/A' ) and at.Processed = 1" );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionMessages", "AnswerTokenId", "dbo.AnswerTokens");
            DropIndex("dbo.QuestionMessages", new[] { "AnswerTokenId" });
            DropTable("dbo.QuestionMessages");
        }
    }
}
