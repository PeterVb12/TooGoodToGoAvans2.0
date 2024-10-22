using TooGoodToGoAvans.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Domain.Models; 


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<TooGoodToGoAvansDBContext>(options => options.UseSqlServer(connectionString));
var authConnectionString = builder.Configuration.GetConnectionString("LocalAuthConnection");
builder.Services.AddDbContext<TooGoodToGoAvansDBContext_IF>(options => options.UseSqlServer(authConnectionString));

// IF gedeelte
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Opties voor IF biedt, kies zelf wat relevant is -->
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<IdentityRole>()// <-- Support voor role based authz
// Welke context voor persistentie -->
.AddEntityFrameworkStores<TooGoodToGoAvansDBContext_IF>()
// Tokens voor wawo reset, tfa, etc -->
.AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Voor elk element in de UserRole enum, controleer of de rol al bestaat
    foreach (var role in Enum.GetValues(typeof(UserRole)))
    {
        var roleName = role.ToString();
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            // Creëer de rol als deze niet bestaat
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

