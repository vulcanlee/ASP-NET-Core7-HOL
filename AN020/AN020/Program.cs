using AN020;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region �ϥ� Configuration �����Ū���]�w���e
// ConfigurationManager ���� key ���r��A�� value ���r�ꪺ��Ʀr��B��l
// �ϥ� �_�� : �Ӥ��j���P�ݩʶ��h���Y
// �ϥε�����|�覡�A�y�z�P���o�]�w�ݩʭ�
string ValidIssuer = builder.Configuration["JWT:ValidIssuer"];

// �ϥ� GetSection ��k���o�]�w���e�����Y�Ӥl���h�A�I�s�Ӥ�k
// �N�|�o ConfigurationSection ���O������A���� key ���r��A�� value ���r�ꪺ��Ʀr��B��l
IConfigurationSection configurationSection = builder
    .Configuration.GetSection("JWT");
// ���o�l���h�U���Y�ӳ]�w�ݩʭ�(�۹���|�y�z�覡)
string ValidAudience = configurationSection["ValidAudience"];
string IssuerSigningKey = builder.Configuration["JWT:IssuerSigningKey"];
// ���o IssuerSigningKey �]�w�ݩʭȡA���L�A�o�ӳ]�w�ݩʭȪ����O���ӭn�����
string ExpireMinutesContent = builder.Configuration["JWT:ExpireMinutes"];
// �N���o���r�ꪺ�]�w�ݩʭȡA�j���૬�������
int ExpireMinutes = int.Parse(ExpireMinutesContent);
string RefreshExpireDaysContent = configurationSection["RefreshExpireDays"];
int RefreshExpireDays = int.Parse(RefreshExpireDaysContent);

Console.WriteLine($"Configuration ValidIssuer : {ValidIssuer}");
Console.WriteLine($"Configuration ValidAudience : {ValidAudience}");
Console.WriteLine($"Configuration ExpireMinutes : {ExpireMinutes}");
Console.WriteLine($"Configuration RefreshExpireDays : {RefreshExpireDays}");
Console.WriteLine($"Configuration IssuerSigningKey : {IssuerSigningKey}");
#endregion

#region �[�J �]�w �j���O �`�J�ŧi
builder.Services.Configure<JwtConfiguration>(builder.Configuration
    .GetSection("JWT"));
#endregion

#region �ϥ� ServiceProvider �i��j���OŪ�� JWT �]�w��
using ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
JwtConfiguration jwtConfiguration = serviceProvider
    .GetRequiredService<IOptions<JwtConfiguration>>().Value;
#endregion

#region ��� JWT �]�w��
Console.WriteLine($"ValidIssuer: {jwtConfiguration.ValidIssuer}");
Console.WriteLine($"ValidAudience: {jwtConfiguration.ValidAudience}");
Console.WriteLine($"ExpireMinutes: {jwtConfiguration.ExpireMinutes}");
Console.WriteLine($"RefreshExpireDays: {jwtConfiguration.RefreshExpireDays}");
Console.WriteLine($"IssuerSigningKey: {jwtConfiguration.IssuerSigningKey}");
#endregion

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
