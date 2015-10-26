namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Person", name: "FirstName", newName: "FirstName");
            AlterColumn("dbo.OfficeAssignment", "Location", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OfficeAssignment", "Location", c => c.String(maxLength: 50));
            RenameColumn(table: "dbo.Person", name: "First Name", newName: "FirstName");
        }
    }
}
