using Prism.Ioc;
using JwtMaui.ViewModels;
using JwtMaui.Views;
using JwtMaui.Helpers;
using JwtMaui.Services;

namespace JwtMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UsePrism(prism =>
                prism.RegisterTypes(container =>
                      {
                          container.RegisterForNavigation<MainPage, MainPageViewModel>();

                          container.RegisterSingleton<JwtTokenHelper>();
                          container.Register<UserService>();
                      })
                     .OnInitialized(() =>
                      {
                         // Do some initializations here
                      })
                     .OnAppStart(async navigationService =>
                     {
                         // Navigate to First page of this App
                         var result = await navigationService
                         .NavigateAsync("NavigationPage/MainPage");
                         if (!result.Success)
                         {
                             System.Diagnostics.Debugger.Break();
                         }
                     })
            )
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
