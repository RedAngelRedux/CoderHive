using CoderHive.Data;
using CoderHive.Models;
using CoderHive.Services;
using CoderHive.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TheBlogProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Acquire or Build the Connection String Depending on Runtime Environment
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string? connectionString;

if(string.IsNullOrEmpty(databaseUrl) == true)
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    var databaseURI = new Uri("postgres://oswnaycnzmksbp:73a1d926d1f508a8b235b62cc3df951d7feae0ff7228ad232d8f5c9f6c33924e@ec2-34-236-199-229.compute-1.amazonaws.com:5432/d19grhiv4vn87\r\n");
    var userInfo = databaseURI.UserInfo.Split(':');

    connectionString = new NpgsqlConnectionStringBuilder()
    {
        Host = databaseURI.Host,
        Port = databaseURI.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseURI.LocalPath.TrimStart('/'),
        SslMode = SslMode.Prefer,
        TrustServerCertificate = true
    }.ToString();
}    

// Replace Default Reference to SQL Server with our Postgres Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register my custom Services
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<BlogSearchService>();

// Register a pre-configured instance of the MailSettings class
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<ICoderHiveEmailSender, EmailService>();

// Register our Image Service
builder.Services.AddScoped<IImageService, BasicImageService>();

// Register our Slug Service
builder.Services.AddScoped<ISlugService, BasicSlugService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "SlugRoute",
    pattern: "BlogPost/{slug}",
    defaults: new { controller = "Posts", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// This will create the database and/or Seed the Datbase if Necessary
// This is another way to get your service activated that is not Constructor Injected
var dataService = app.Services
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<DataService>();
await dataService.ManageDataAsync();

app.Run();
