using CloudStorage.BLL.MappingProfiles;
using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.DAL;
using CloudStorage.Web.Areas.Identity.Data;
using CloudStorage.Web.MappingProfiles;
using Microsoft.EntityFrameworkCore;

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
    var databaseConnectionString = builder.Configuration.GetConnectionString("CloudStorageDatabaseConnection");

    services
        .AddTransient<ICloudStorageManager, CloudStorageManager>()
        .AddTransient<ICloudStorageUnitOfWork, CloudStorageUnitOfWork>()
        .AddTransient<IUserService, UserService>()
        .AddSingleton<IDataHasher, Sha1DataHasher>()
        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
        .AddTransient<IFileStorageService, FileStorageService>()
        .AddDbContext<CloudStorageUnitOfWork>(optionsBuilder =>
            optionsBuilder.UseSqlServer(databaseConnectionString));

    var fileStorageOptions = builder.Configuration.GetSection(nameof(FileStorageOptions)).Get<FileStorageOptions>();

    services.Configure<FileStorageOptions>(opt =>
    {
        opt.FilesDirectoryPath = Path.Combine(builder.Environment.WebRootPath, fileStorageOptions.FilesDirectoryPath);
        opt.FFmpegExecutablesPath = Path.Combine(builder.Environment.WebRootPath, fileStorageOptions.FFmpegExecutablesPath);
    });

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