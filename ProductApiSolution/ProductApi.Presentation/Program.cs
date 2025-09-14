using ProductApi.Infrastructure.DependencyInjection; 
    
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureService(builder.Configuration); //Add
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseInfrastructurePolicy(); // Add
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();