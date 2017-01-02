namespace CareerVisa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareerVisa_Configurations_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configurations", "ConfigurationType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "ConfigurationType");
        }
    }
}
