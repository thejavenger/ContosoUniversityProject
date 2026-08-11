namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxLengthOnNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Person", newName: "Student");
            DropForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Person");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Person");
            DropForeignKey("dbo.Course", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course");
            DropForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Person");
            DropIndex("dbo.Course", new[] { "DepartmentID" });
            DropIndex("dbo.Department", new[] { "InstructorID" });
            DropIndex("dbo.OfficeAssignment", new[] { "InstructorID" });
            DropIndex("dbo.CourseInstructor", new[] { "CourseID" });
            DropIndex("dbo.CourseInstructor", new[] { "InstructorID" });
            AddColumn("dbo.Student", "FirstMidName", c => c.String(maxLength: 50));
            AddColumn("dbo.Student", "Secret", c => c.String());
            AlterColumn("dbo.Course", "Title", c => c.String());
            AlterColumn("dbo.Student", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "EnrollmentDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Course", "DepartmentID");
            DropColumn("dbo.Student", "FirstName");
            DropColumn("dbo.Student", "HireDate");
            DropColumn("dbo.Student", "Discriminator");
            DropTable("dbo.Department");
            DropTable("dbo.OfficeAssignment");
            DropTable("dbo.CourseInstructor");
            DropStoredProcedure("dbo.Department_Insert");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Delete");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CourseInstructor",
                c => new
                    {
                        CourseID = c.Int(nullable: false),
                        InstructorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseID, t.InstructorID });
            
            CreateTable(
                "dbo.OfficeAssignment",
                c => new
                    {
                        InstructorID = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.InstructorID);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(nullable: false, storeType: "money"),
                        StartDate = c.DateTime(nullable: false),
                        InstructorID = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            AddColumn("dbo.Student", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Student", "HireDate", c => c.DateTime());
            AddColumn("dbo.Student", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Course", "DepartmentID", c => c.Int(nullable: false));
            AlterColumn("dbo.Student", "EnrollmentDate", c => c.DateTime());
            AlterColumn("dbo.Student", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Course", "Title", c => c.String(maxLength: 50));
            DropColumn("dbo.Student", "Secret");
            DropColumn("dbo.Student", "FirstMidName");
            CreateIndex("dbo.CourseInstructor", "InstructorID");
            CreateIndex("dbo.CourseInstructor", "CourseID");
            CreateIndex("dbo.OfficeAssignment", "InstructorID");
            CreateIndex("dbo.Department", "InstructorID");
            CreateIndex("dbo.Course", "DepartmentID");
            AddForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Person", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.Course", "DepartmentID", "dbo.Department", "DepartmentID", cascadeDelete: true);
            AddForeignKey("dbo.Department", "InstructorID", "dbo.Person", "ID");
            AddForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Person", "ID");
            RenameTable(name: "dbo.Student", newName: "Person");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
