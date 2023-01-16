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

    #region �b�� ���U Registration �ݭn�Ψ쪺�Ȼs�A��
    builder.Services.AddTransient<MyService>();
    builder.Services.AddTransient<YourService>();
    #endregion

    #region �n�b���ŧi���U NLog �|�Ψ쪺�����A��
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    var app = builder.Build();

    #region �`�J ILogger ����A�åB�ϥ� ��x �\��
    var loggerApp = app.Services.GetRequiredService<ILogger<Program>>();
    loggerApp.LogInformation($"�w�g���� WebApplication ����إ�");
    #endregion

    #region �`�J MyService ����A�[�� ILogger ����B�@���p
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