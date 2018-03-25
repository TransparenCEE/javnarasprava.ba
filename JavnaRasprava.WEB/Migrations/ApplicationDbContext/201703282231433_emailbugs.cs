namespace JavnaRasprava.WEB.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailbugs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionMessages", "MessageSentTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionMessages", "MessageSentTime", c => c.DateTime(nullable: false));
        }
    }
}
