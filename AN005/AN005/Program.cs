using AN005.Services;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMyService, MyService>();

#region �b WebApplicationBuilder ���󤺶i��̩ۨʪ`�J
using (ServiceProvider serviceProvider = 
    builder.Services.BuildServiceProvider())
{
    var myServiceBeforeBuild = serviceProvider
        .GetRequiredService<IMyService>();
    var loggerBeforeBuild = serviceProvider
        .GetRequiredService<ILogger<Program>>();

    var hiBeforeBuild = myServiceBeforeBuild.Hi("Vulcan");

    loggerBeforeBuild.LogInformation($"In WebApplicationBuilder, Call Hi Method : {hiBeforeBuild}");
}
#endregion

var app = builder.Build();

#region �b WebApplication ���󤺶i��̩ۨʪ`�J
    var myService = app.Services.GetService<IMyService>();
    var logger = app.Services.GetService<ILogger<Program>>();

    var hi = myService.Hi("Lee");

    logger.LogInformation($"In WebApplication, Call Hi Method : {hi}");
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
