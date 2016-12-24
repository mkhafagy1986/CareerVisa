namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.JobSeekerCareerFields");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobSeekerCareerFields",
                c => new
                    {
                        JobSeekerCareerFieldsId = c.Int(nullable: false, identity: true),
                        JobSeekerId = c.String(),
                        CareerFieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobSeekerCareerFieldsId);
            
        }
    }
}
