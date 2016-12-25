namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarrerVisa_JobSeekerDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Documents", "ApplicationUser_Id");
            AddForeignKey("dbo.Documents", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Documents", "ApplicationUser_Id");
        }
    }
}
