namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_AssignDocuments : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "ApplicationUser_Id", newName: "User_Id");
            RenameIndex(table: "dbo.Documents", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            CreateTable(
                "dbo.AssignedDocuments",
                c => new
                    {
                        AssignedDocumentId = c.Int(nullable: false, identity: true),
                        OwnerUserId = c.String(),
                        AssignedDate = c.DateTime(nullable: false),
                        AdministratorUserId = c.String(),
                        DocumentId = c.Int(nullable: false),
                        ReviewerUserId = c.String(),
                    })
                .PrimaryKey(t => t.AssignedDocumentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AssignedDocuments");
            RenameIndex(table: "dbo.Documents", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Documents", name: "User_Id", newName: "ApplicationUser_Id");
        }
    }
}
