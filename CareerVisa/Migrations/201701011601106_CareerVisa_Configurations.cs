namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_Configurations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        ConfigurationId = c.Int(nullable: false, identity: true),
                        ConfigurationKey = c.String(),
                        ConfigurationValue = c.String(),
                    })
                .PrimaryKey(t => t.ConfigurationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Configurations");
        }
    }
}
