using TooGoodToGoAvans.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.UI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStaffMemberRepository, StaffMemberRepository>();
builder.Services.AddScoped<ICanteenRepository, CanteenRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Logging.AddConsole();

string? connectionString;
string? authConnectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("LocalConnection");
    authConnectionString = builder.Configuration.GetConnectionString("LocalAuthConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("AzureConnection");
    authConnectionString = builder.Configuration.GetConnectionString("AzureAuthConnection");
}

// Configureer de DbContexts
builder.Services.AddDbContext<TooGoodToGoAvansDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<TooGoodToGoAvansDBContext_IF>(options =>
    options.UseSqlServer(authConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<IdentityRole>()// <-- Support voor role based authz
.AddEntityFrameworkStores<TooGoodToGoAvansDBContext_IF>()
.AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in Enum.GetValues(typeof(UserRole)))
    {
        var roleName = role.ToString();
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

