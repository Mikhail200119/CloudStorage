using CloudStorage.BLL.MappingProfiles;
using CloudStorage.BLL.Services;
using CloudStorage.DAL;
using CloudStorage.Web.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CloudStorage.Web.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureServices(services =>
{
    services
        .AddTransient<ICloudStorageManager, CloudStorageManager>()
        .AddTransient<ICloudStorageUnitOfWork, CloudStorageUnitOfWork>()
        .AddTransient<IUserService, UserService>()
        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
        .AddDbContext<CloudStorageUnitOfWork>(optionsBuilder => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CloudStorageDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

    services.AddAutoMapper(
        typeof(FilesMappingProfile),
        typeof(FileProfile));

    services.AddRazorPages();
    services.AddMvc();
    services.AddControllersWithViews();
});

var app = builder.Build();

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
    pattern: "{controller=Files}/{action=ViewAllFiles}/{id?}");

app.MapRazorPages();

app.Run();
