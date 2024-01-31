using Application_Security_Assignment.NewModels;
using Microsoft.AspNetCore.Identity;
using WebApp_Core_Identity.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Lockout.MaxFailedAccessAttempts = 3; // max failed attempts
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10); // lockout time (advanced feature)
	options.Lockout.AllowedForNewUsers = true;

})
	.AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddDataProtection();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options => //session options
{
	options.IdleTimeout = TimeSpan.FromSeconds(5); // session timeout
});

builder.Services.ConfigureApplicationCookie(Config => // cookie options, ensure that the path is set to login page
{
	Config.Cookie.HttpOnly = true;
	Config.LoginPath = "/Login";
	Config.ExpireTimeSpan = TimeSpan.FromSeconds(5); // cookie timeout so you remove the cookies along with the session ending
	Config.SlidingExpiration = true;
	Config.Cookie.SameSite = SameSiteMode.Strict;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
