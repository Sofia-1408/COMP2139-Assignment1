using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using COMP2139_Assignment1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 10,  
        shared: true)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog();

// Database connection 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Aspnet Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession(); //Adds a session (necessary for the number display next to the cart)


var app = builder.Build(); 

//  identity seeding creates roles + admin user if they dont exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    IdentitySeeder.SeedAsync(services).GetAwaiter().GetResult();
}

if (!app.Environment.IsDevelopment()) //HTTP request handling
{
    app.UseExceptionHandler("/Home/Error"); // Error sign will show if page crashes
    app.UseHsts(); // Forces the browser to always use HTTPS
}

app.UseSession(); //Adds a session (necessary for the number display next to the cart)

app.UseHttpsRedirection(); // This should convert HTTP to HTTPS
app.UseStaticFiles();

app.UseRouting();

// We authenticate then authorize 
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages();

// Error handling 500
app.UseExceptionHandler("/Home/Error500");
// Error handling like 404
app.UseStatusCodePagesWithReExecute("/Home/Error{0}");


app.Run();