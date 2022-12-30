using System.Security.Cryptography;
using CloudStorage.BLL.MappingProfiles;
using CloudStorage.BLL.Options;
using CloudStorage.BLL.Services;
using CloudStorage.BLL.Services.Interfaces;
using CloudStorage.DAL;
using CloudStorage.Web.Areas.Identity.Data;
using CloudStorage.Web.Filters;
using CloudStorage.Web.MappingProfiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();

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
        .AddTransient<IAesEncryptor, AesEncryptor>()
        .AddDbContext<CloudStorageUnitOfWork>(optionsBuilder =>
            optionsBuilder.UseNpgsql(databaseConnectionString));


    var fileStorageOptions = builder.Configuration.GetSection(nameof(FileStorageOptions)).Get<FileStorageOptions>();

    var aes = Aes.Create();
    aes.GenerateKey();

    services.Configure<FileStorageOptions>(opt =>
    {
        opt.FilesDirectoryPath = Path.Combine(builder.Environment.WebRootPath, fileStorageOptions.FilesDirectoryPath);
        opt.FFmpegExecutablesPath = Path.Combine(builder.Environment.WebRootPath, fileStorageOptions.FFmpegExecutablesPath);
    });

    //services.Configure<FileEncryptionOptions>(builder.Configuration.GetSection(nameof(FileEncryptionOptions)));
    services.Configure<FileEncryptionOptions>(opt => opt.EncryptionKey = aes.Key);

    services.AddAutoMapper(
        typeof(FilesMappingProfile),
        typeof(FileProfile));

    services.AddRazorPages();
    services.AddMvc();
    services.AddControllersWithViews(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });
});

var app = builder.Build();

/*app.MapTus("/files", httpContext => Task.FromResult<DefaultTusConfiguration>(new()
{
    // This method is called on each request so different configurations can be returned per user, domain, path etc.
    // Return null to disable tusdotnet for the current request.

    // Where to store data?
    Store = new tusdotnet.Stores.TusDiskStore(@"C:\tusfiles\"),
    Events = new()
    {
        // What to do when file is completely uploaded?
        OnFileCompleteAsync = async eventContext =>
        {
            ITusFile file = await eventContext.GetFileAsync();
            //Dictionary<string, Metadata> metadata = await file.GetMetadataAsync(eventContext.CancellationToken);
            await using var content = await file.GetContentAsync(eventContext.CancellationToken);

            //await DoSomeProcessing(content, metadata);
            var fileStorageService = app.Services.GetRequiredService<IFileStorageService>();
            await fileStorageService.UploadStream("name", content);
        }
    }
}));*/

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.MapGet("/Files/get-content", async (int id, string contentType) =>
{
    var scope = app.Services.CreateScope();

    var cloudStorageManager = scope.ServiceProvider.GetRequiredService<ICloudStorageManager>();
    var fileStream = await cloudStorageManager.GetFileStreamAsync(id);
  

    return Results.File(fileStream, contentType, enableRangeProcessing: true);
});

app.Run();