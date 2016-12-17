namespace CWMD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mymigration3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Role", c => c.String(nullable: false));
        }
    }
}
