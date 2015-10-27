namespace LoggerLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        EntryId = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEntries");
        }
    }
}
