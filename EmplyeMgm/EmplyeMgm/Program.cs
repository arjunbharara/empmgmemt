using EmplyeMgm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EmplyeMgm.Services;
using EmplyeMgm.Service;
using EmplyeMgm.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDb")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<IAdminService, AdminService>();

var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await ApplicationDbContext.SeedRolesAndSuperAdmin(services);
    }
    catch (Exception ex)
    {
        // Log any errors
        Console.Error.WriteLine($"Error seeding roles and SuperAdmin user: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//adding middleware for handling the unhandled exceptions.
app.UseMiddleware<CustsomExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/Base/Error", "?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
