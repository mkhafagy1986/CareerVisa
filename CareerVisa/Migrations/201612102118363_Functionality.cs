namespace MVCTutorial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Functionality : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.Functionalities",
                c => new
                    {
                        FunctionalityId = c.Int(nullable: false, identity: true),
                        FunctionalityName = c.String(),
                        FunctionalityDescription = c.String(),
                        FunctionalityImagePath = c.String(),
                    })
                .PrimaryKey(t => t.FunctionalityId);
            
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Functionalities");
        }
    }
}
