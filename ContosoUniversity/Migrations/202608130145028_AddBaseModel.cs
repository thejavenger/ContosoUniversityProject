namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaseModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Course", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Course", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.Department", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Department", "DateDeleted", c => c.DateTime());
            AddColumn("dbo.Person", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Person", "DateDeleted", c => c.DateTime());
            AlterStoredProcedure(
                "dbo.Department_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        InstructorID = p.Int(),
                        IsActive = p.Boolean(),
                        DateDeleted = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Department]([Name], [Budget], [StartDate], [InstructorID], [IsActive], [DateDeleted])
                      VALUES (@Name, @Budget, @StartDate, @InstructorID, @IsActive, @DateDeleted)
                      
                      DECLARE @DepartmentID int
                      SELECT @DepartmentID = [DepartmentID]
                      FROM [dbo].[Department]
                      WHERE @@ROWCOUNT > 0 AND [DepartmentID] = scope_identity()
                      
                      SELECT t0.[DepartmentID], t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
            AlterStoredProcedure(
                "dbo.Department_Update",
                p => new
                    {
                        DepartmentID = p.Int(),
                        Name = p.String(maxLength: 50),
                        Budget = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        StartDate = p.DateTime(),
                        InstructorID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        IsActive = p.Boolean(),
                        DateDeleted = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [InstructorID] = @InstructorID, [IsActive] = @IsActive, [DateDeleted] = @DateDeleted
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "DateDeleted");
            DropColumn("dbo.Person", "IsActive");
            DropColumn("dbo.Department", "DateDeleted");
            DropColumn("dbo.Department", "IsActive");
            DropColumn("dbo.Course", "DateDeleted");
            DropColumn("dbo.Course", "IsActive");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
