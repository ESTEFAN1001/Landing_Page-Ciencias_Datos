using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LosCokis123.Areas.Identity.Data;
using LosCokis123.Data;
using LosCokis123.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LosCokis123Context>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("connection")
    )
);

builder.Services.AddDefaultIdentity<LosCokis123User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LosCokis123Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseCookiePolicy();

builder.Services.AddDistributedMemoryCache();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "admin", "common" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    }
}


app.Run();
