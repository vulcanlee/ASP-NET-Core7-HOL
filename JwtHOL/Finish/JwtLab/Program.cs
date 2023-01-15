using CommonDomainLayer.Magics;
using JwtLab.Models;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;

#region NLog ���Ұʫŧi
// NLog �]�w���� : https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
#endregion

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region �[�J�A�Ȩ�e���� Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    #region NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    #region �����ﶵ�Ҧ�
    builder.Services.Configure<CustomNLogConfiguration>(builder.Configuration
        .GetSection(MagicObject.SectionNameOfCustomNLogConfiguration));
    #endregion

    #endregion

    var app = builder.Build();

    #region �ŧi�޹D�P�����n��

    #region �ŧi NLog �n�ϥΨ쪺�ܼƤ��e
    CustomNLogConfiguration optionsCustomNLogConfiguration = 
        app.Services.GetRequiredService<IOptions<CustomNLogConfiguration>>()
        .Value;
    LogManager.Configuration.Variables["LogRootPath"] =
        optionsCustomNLogConfiguration.LogRootPath;
    LogManager.Configuration.Variables["AllLogMessagesFilename"] =
        optionsCustomNLogConfiguration.AllLogMessagesFilename;
    LogManager.Configuration.Variables["AllWebDetailsLogMessagesFilename"] =
        optionsCustomNLogConfiguration.AllWebDetailsLogMessagesFilename;
    #endregion

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    #endregion

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