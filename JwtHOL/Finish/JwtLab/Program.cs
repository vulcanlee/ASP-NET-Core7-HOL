using BussinessLayer.Helpers;
using CommonDomainLayer.Magics;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using DataTransferObjects.Dtos;
using JwtLab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using System.Text;

#region NLog 的啟動宣告
// NLog 設定說明 : https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
#endregion

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region 加入服務到容器內 Add services to the container.

    #region 專案範本預先建立的
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    #endregion

    #region 註冊本專案會用到的克制服務
    builder.Services.AddTransient<IMyUserService, MyUserService>();
    #endregion

    #region 相關選項模式
    builder.Services.Configure<CustomNLogConfiguration>(builder.Configuration
        .GetSection(MagicObject.SectionNameOfCustomNLogConfiguration));
    builder.Services.Configure<JwtConfiguration>(builder.Configuration
        .GetSection(MagicObject.SectionNameOfJwtConfiguration));
    #endregion

    #region 加入使用 Cookie & JWT 認證需要的宣告
    JwtConfiguration jwtConfiguration =
    builder.Services.BuildServiceProvider()
    .GetRequiredService<IOptions<JwtConfiguration>>()
    .Value;

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
    });
    builder.Services.AddAuthentication(
        MagicObject.CookieAuthenticationScheme)
        .AddCookie(MagicObject.CookieAuthenticationScheme)
        .AddJwtBearer(MagicObject.JwtBearerAuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(jwtConfiguration.ValidAudience)),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtConfiguration.ClockSkew),
            };
            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.HttpContext.Features
                    .Get<IHttpResponseFeature>().ReasonPhrase = 
                    context.Exception.Message;
                    APIResult apiResult = JWTTokenFailHelper.GetFailResult(context.Exception);

                    context.Response.ContentType = "application/json";
                    context.Response.WriteAsync(JsonConvert.SerializeObject(apiResult)).Wait();
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    ////context.HandleResponse();
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    //Console.WriteLine("OnTokenValidated: " +
                    //    context.SecurityToken);
                    return Task.CompletedTask;
                }

            };
        });
    #endregion

    #region NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    #endregion

    var app = builder.Build();

    #region 宣告管道與中介軟體

    #region 宣告 NLog 要使用到的變數內容
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

    #region Set X-FRAME-OPTIONS in ASP.NET Core
    // https://blog.johnwu.cc/article/asp-net-core-response-header.html
    // https://dotnetcoretutorials.com/2017/01/08/set-x-frame-options-asp-net-core/
    // https://developer.mozilla.org/zh-TW/docs/Web/HTTP/Headers/X-Frame-Options
    // https://blog.darkthread.net/blog/remove-iis-response-server-header/
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Remove("X-Frame-Options");
        context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
        await next();
    });
    #endregion

    #region 開發模式的設定
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        #region 啟用 Swagger 中介軟體
        app.UseSwagger();
        app.UseSwaggerUI();
        #endregion
    }
    #endregion

    #region 強制使用 HTTPS
    app.UseHttpsRedirection();
    #endregion

    #region 指定要使用使用者認證的中介軟體
    app.UseCookiePolicy();
    app.UseAuthentication();
    #endregion

    #region 指定使用授權檢查的中介軟體
    app.UseAuthorization();
    #endregion

    #region 將端點執行新增至中介軟體管線
    app.MapControllers();
    #endregion
  
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