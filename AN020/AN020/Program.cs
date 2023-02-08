using AN020;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region 使用 Configuration 物件來讀取設定內容
// ConfigurationManager 提供 key 為字串，而 value 為字串的資料字典運算子
// 使用 冒號 : 來分隔不同屬性階層關係
// 使用絕對路徑方式，描述與取得設定屬性值
string ValidIssuer = builder.Configuration["JWT:ValidIssuer"];

// 使用 GetSection 方法取得設定內容內的某個子階層，呼叫該方法
// 將會得 ConfigurationSection 型別的物件，提供 key 為字串，而 value 為字串的資料字典運算子
IConfigurationSection configurationSection = builder
    .Configuration.GetSection("JWT");
// 取得子階層下的某個設定屬性值(相對路徑描述方式)
string ValidAudience = configurationSection["ValidAudience"];
string IssuerSigningKey = builder.Configuration["JWT:IssuerSigningKey"];
// 取得 IssuerSigningKey 設定屬性值，不過，這個設定屬性值的型別應該要為整數
string ExpireMinutesContent = builder.Configuration["JWT:ExpireMinutes"];
// 將取得為字串的設定屬性值，強制轉型成為整數
int ExpireMinutes = int.Parse(ExpireMinutesContent);
string RefreshExpireDaysContent = configurationSection["RefreshExpireDays"];
int RefreshExpireDays = int.Parse(RefreshExpireDaysContent);

Console.WriteLine($"Configuration ValidIssuer : {ValidIssuer}");
Console.WriteLine($"Configuration ValidAudience : {ValidAudience}");
Console.WriteLine($"Configuration ExpireMinutes : {ExpireMinutes}");
Console.WriteLine($"Configuration RefreshExpireDays : {RefreshExpireDays}");
Console.WriteLine($"Configuration IssuerSigningKey : {IssuerSigningKey}");
#endregion

#region 加入 設定 強型別 注入宣告
builder.Services.Configure<JwtConfiguration>(builder.Configuration
    .GetSection("JWT"));
#endregion

#region 使用 ServiceProvider 進行強型別讀取 JWT 設定值
using ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
JwtConfiguration jwtConfiguration = serviceProvider
    .GetRequiredService<IOptions<JwtConfiguration>>().Value;
#endregion

#region 顯示 JWT 設定值
Console.WriteLine($"ValidIssuer: {jwtConfiguration.ValidIssuer}");
Console.WriteLine($"ValidAudience: {jwtConfiguration.ValidAudience}");
Console.WriteLine($"ExpireMinutes: {jwtConfiguration.ExpireMinutes}");
Console.WriteLine($"RefreshExpireDays: {jwtConfiguration.RefreshExpireDays}");
Console.WriteLine($"IssuerSigningKey: {jwtConfiguration.IssuerSigningKey}");
#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
