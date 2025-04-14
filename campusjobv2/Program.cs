using campusjobv2;
using campusjobv2.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    )
);

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "CampusJob.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var method = context.Request.Method;

    var excludedPaths = new[] {
        "/Login",
        "/Account",
        "/lib",
        "/css",
        "/js",
        "/_framework",
        "/favicon.ico",
        "/Home",
        "/"
    };

    if (excludedPaths.Any(p => path.StartsWithSegments(p)) ||
        (path == "/Login/Index" && method == "POST"))
    {
        await next();
        return;
    }

    var userId = context.Session.GetInt32("UserId");
    
    if (userId == null)
    {
        context.Response.Redirect("/Login");
        return;
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
