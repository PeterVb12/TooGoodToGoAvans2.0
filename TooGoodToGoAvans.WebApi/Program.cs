using Microsoft.EntityFrameworkCore;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;
using TooGoodToGoAvans.WebApi.GraphQl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Logging.AddConsole();

string? connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("AzureConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("AzureConnection");
}

// Configureer de DbContexts
builder.Services.AddDbContext<TooGoodToGoAvansDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<PackageQuery>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware volgorde
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.MapGraphQL("/graphql");

app.Run();
