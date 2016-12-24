namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_CareerField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserCareerFields",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CareerField_CareerFieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CareerField_CareerFieldId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CareerFields", t => t.CareerField_CareerFieldId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CareerField_CareerFieldId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserCareerFields", "CareerField_CareerFieldId", "dbo.CareerFields");
            DropForeignKey("dbo.ApplicationUserCareerFields", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserCareerFields", new[] { "CareerField_CareerFieldId" });
            DropIndex("dbo.ApplicationUserCareerFields", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserCareerFields");
        }
    }
}
