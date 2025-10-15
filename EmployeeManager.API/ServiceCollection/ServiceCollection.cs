using EmployeeManager.API.Config;
using EmployeeManager.API.Middleware;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.UseCases.Company.Commands;
using EmployeeManager.Application.Validators.Company;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Infrastructure.Repositories;
using FluentMigrator.Runner;
using FluentValidation;
using Npgsql;
using System.Data;

namespace EmployeeManager.API.ServiceCollection
{
    public static class ServiceCollection
    {
        public static WebApplicationBuilder AddConfig(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
            return builder;
        }


        public static WebApplicationBuilder AddMigrations(this WebApplicationBuilder builder)
        {
            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                  rb.AddPostgres()
                .WithGlobalConnectionString(
                    builder.Configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(EmployeeManager.Infrastructure.Migrations.InfrastructureMarker).Assembly)
                .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            return builder;
        }

        public static WebApplicationBuilder AddValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyCommandValidator>();
            return builder;
        }

        public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
             {
                 cfg.RegisterServicesFromAssembly(
                     typeof(CreateCompanyCommand).Assembly
                 ); 
             });
            return builder;
        }

        public static WebApplicationBuilder AddDbConnection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                return connection;
            });
            return builder;
        }


        public static WebApplicationBuilder AddAllRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();            
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IPassportRepository, PassportRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            return builder;
        }


        public static WebApplication? UseMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            return app;
        }


        public static WebApplication? UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();

            return app;
        }
    }
}
