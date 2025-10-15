using EmployeeManager.API.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.AddConfig()
    .AddValidation()
    .AddMediatR()
    .AddMigrations()
    .AddDbConnection()
    .AddAllRepositories();
    

var app = builder.Build();

app.UseMigrations();

app.MapOpenApi();
app.UseSwaggerUI(options => 
    options.SwaggerEndpoint("/openapi/v1.json", "EmployeeManager"));

app.UseMiddlewares();

app.UseAuthorization();

app.MapControllers();

app.Run();
