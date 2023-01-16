using AN006.Services;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorPages();

    #region 在此 註冊 Registration 需要用到的客製服務
    builder.Services.AddTransient<MyService>();
    builder.Services.AddTransient<YourService>();
    #endregion

    #region 要在此宣告註冊 NLog 會用到的相關服務
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    var app = builder.Build();

    #region 注入 ILogger 物件，並且使用 日誌 功能
    var loggerApp = app.Services.GetRequiredService<ILogger<Program>>();
    loggerApp.LogInformation($"已經完成 WebApplication 物件建立");
    #endregion

    #region 注入 MyService 物件，觀察 ILogger 物件運作情況
    var myService = app.Services.GetService<MyService>();
    myService.DoSomething();
    #endregion

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}