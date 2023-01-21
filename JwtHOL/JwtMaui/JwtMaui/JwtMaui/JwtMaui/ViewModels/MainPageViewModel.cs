using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JwtMaui.Helpers;
using JwtMaui.Services;

namespace JwtMaui.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware
{
    public MainPageViewModel(UserService userService,
        JwtTokenHelper jwtTokenHelper)
    {
        this.userService = userService;
        this.jwtTokenHelper = jwtTokenHelper;

#if DEBUG
        Account = "Emily";
        Password = "123";
#endif
    }

    [ObservableProperty]
    string title = "JWT 認證與授權使用範例";

    [ObservableProperty]
    string text = "Click me";

    [ObservableProperty]
    string jwt = string.Empty;

    [ObservableProperty]
    string users = string.Empty;

    [ObservableProperty]
    string message = string.Empty;

    [ObservableProperty]
    bool showLoginUI = true;

    [ObservableProperty]
    bool showLogoutUI = false;

    [ObservableProperty]
    string account = string.Empty;

    [ObservableProperty]
    string password = string.Empty;
    private readonly UserService userService;
    private readonly JwtTokenHelper jwtTokenHelper;

    [RelayCommand]
    private async Task LoginAsync()
    {
        Message = "";
        await userService.LoginAsync(account, password);
        if (string.IsNullOrEmpty(jwtTokenHelper.Token))
            Message = $"身分驗證失敗，無法取得存取權杖";
        else
        {
            ShowLoginUI = false;
            ShowLogoutUI = true;
            Jwt = jwtTokenHelper.Token;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        Message = "";
        await jwtTokenHelper.WriteAsync(string.Empty);
        ShowLoginUI = true;
        ShowLogoutUI = false;
        Jwt = string.Empty;
        Users= string.Empty;
    }

    [RelayCommand]
    private async Task GetUsersAsync()
    {
        Message = "";
        Users = "";
        (string returnUsers,string returnMessage) = await userService.GetUsersAsync();
        if (string.IsNullOrEmpty(returnUsers))
        {
            Message = returnMessage;
        }
        else
        {
            Users = returnUsers;
        }
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public async void OnNavigatedTo(INavigationParameters parameters)
    {
        await jwtTokenHelper.ReadAsync();
        if (string.IsNullOrEmpty(jwtTokenHelper.Token))
            return;
        else
        {
            ShowLoginUI = false;
            ShowLogoutUI = true;
            Jwt = jwtTokenHelper.Token;
        }
    }
}
