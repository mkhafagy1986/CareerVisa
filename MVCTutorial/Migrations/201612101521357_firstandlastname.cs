namespace MVCTutorial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstandlastname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Lastname", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Lastname");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
