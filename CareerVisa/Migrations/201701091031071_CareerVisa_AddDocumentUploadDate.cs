namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_AddDocumentUploadDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "UploadDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "UploadDate");
        }
    }
}
