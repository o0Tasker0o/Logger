namespace LoggerLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodoList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoEntries",
                c => new
                    {
                        EntryId = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.EntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TodoEntries");
        }
    }
}
