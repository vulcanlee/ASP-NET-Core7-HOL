using DataTransferObjects.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace JwtConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var token = await LoginAsync("Emily", "123");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"身分驗證失敗");
                return;
            }

            var allUsers = await GetUsers(token + "");

            if (string.IsNullOrEmpty(allUsers) == false)
                Console.WriteLine($"取得使用者清單 {allUsers}");
        }

        static async Task<string> GetUsers(string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await client.GetAsync("https://localhost:7129/api/JwtAuth/NeedAuth");
            if (responseMessage.IsSuccessStatusCode)
            {
                APIResult apiResult = await responseMessage.Content.ReadFromJsonAsync<APIResult>();
                if (apiResult.Status == true)
                {
                    List<MyUserDto> myUsersDto = JsonConvert
                        .DeserializeObject<List<MyUserDto>>(apiResult.Payload.ToString());
                    string result = string.Empty;
                    foreach (var item in myUsersDto)
                    {
                        result += $"{item.Account},";
                    }
                    return result;
                }
                else
                    return string.Empty;
            }
            else
            {
                APIResult apiResult = await responseMessage.Content.ReadFromJsonAsync<APIResult>();
                Console.WriteLine($"發生錯誤 : {apiResult.Message}");
                return string.Empty;
            }
        }

        static async Task<string> LoginAsync(string account, string password)
        {
            HttpClient client = new HttpClient();

            LoginRequestDto loginRequestDto = new LoginRequestDto()
            {
                Account = account,
                Password = password,
            };
            var responseMessage = await client.PostAsJsonAsync<LoginRequestDto>("https://localhost:7129/api/Login", loginRequestDto);
            if (responseMessage.IsSuccessStatusCode)
            {
                APIResult apiResult = await responseMessage.Content.ReadFromJsonAsync<APIResult>();
                if (apiResult.Status == true)
                {
                    LoginResponseDto loginResponseDto = JsonConvert
                        .DeserializeObject<LoginResponseDto>(apiResult.Payload.ToString());
                    Console.WriteLine($"Token : {loginResponseDto.Token}");
                    return loginResponseDto.Token;
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }
    }
}