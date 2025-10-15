using FluentMigrator;
using System.Data;

namespace EmployeeManager.Infrastructure.Migrations
{

    [Migration(1)]
    public class AddTables : Migration
    {
        public override void Up()
        {
            Create.Table("companies")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("address").AsString(200).Unique().NotNullable()
                .WithColumn("name").AsString(100).Unique().NotNullable();

            Create.Table("departments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(100).NotNullable()
                .WithColumn("phone").AsString(16).Unique().NotNullable()
                .WithColumn("company_id").AsInt32().NotNullable();

            Create.ForeignKey("fk_departments_companies")
                .FromTable("departments").ForeignColumn("company_id")
                .ToTable("companies").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.Table("passports")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("number").AsString(100).Unique().NotNullable()
                .WithColumn("type").AsString(30).NotNullable();
            
            Create.Table("employees")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(25).NotNullable()
                .WithColumn("surname").AsString(25).NotNullable()
                .WithColumn("phone").AsString(16).NotNullable()
                .WithColumn("department_id").AsInt32().NotNullable()
                .WithColumn("passport_id").AsInt32().Nullable().Unique(); // 1:1 связь

            Create.ForeignKey("fk_employees_departments")
                .FromTable("employees").ForeignColumn("department_id")
                .ToTable("departments").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("fk_employees_passports")
                .FromTable("employees").ForeignColumn("passport_id")
                .ToTable("passports").PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.SetNull);
        }


        public override void Down()
        {      
            Delete.Table("employees");
            Delete.Table("passports");
            Delete.Table("departments");
            Delete.Table("companies");
        }
    }
}
