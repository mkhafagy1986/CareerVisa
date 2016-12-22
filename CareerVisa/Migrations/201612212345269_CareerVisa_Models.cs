namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_Models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CareerFields",
                c => new
                    {
                        CareerFieldId = c.Int(nullable: false, identity: true),
                        CareerFieldName = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.CareerFieldId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        DocumentDescription = c.String(maxLength: 500),
                        DocumentLocation = c.String(maxLength: 500),
                        DocumentTypeId = c.Int(nullable: false),
                        DocumentStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentId);
            
            CreateTable(
                "dbo.DocumentStatus",
                c => new
                    {
                        DocumentStatusId = c.Int(nullable: false, identity: true),
                        DocumentStatusDescription = c.String(),
                    })
                .PrimaryKey(t => t.DocumentStatusId);
            
            CreateTable(
                "dbo.DocumentsTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        DocumentTypeDescription = c.String(),
                    })
                .PrimaryKey(t => t.DocumentTypeId);
                   
            CreateTable(
                "dbo.JobSeekerCareerFields",
                c => new
                    {
                        JobSeekerCareerFieldsId = c.Int(nullable: false, identity: true),
                        JobSeekerId = c.String(),
                        CareerFieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobSeekerCareerFieldsId);
            
            
            AddColumn("dbo.AspNetUsers", "Address",c=>c.String());
            AddColumn("dbo.AspNetUsers", "LinkedInURL", c=>c.String());
            AddColumn("dbo.AspNetUsers", "PersonalPhotoPath", c=>c.String());
            AddColumn("dbo.AspNetUsers", "WebsiteURL", c=>c.String());
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobSeekerCareerFields");
            DropTable("dbo.DocumentsTypes");
            DropTable("dbo.DocumentStatus");
            DropTable("dbo.Documents");
            DropTable("dbo.CareerFields");

            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "LinkedInURL");
            DropColumn("dbo.AspNetUsers", "PersonalPhotoPath");
            DropColumn("dbo.AspNetUsers", "WebsiteURL");
        }
    }
}
