using JwtMaui.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.Dtos;

namespace JwtMaui.Services
{
    public class UserService
    {
        private readonly JwtTokenHelper jwtTokenHelper;

        public UserService(JwtTokenHelper jwtTokenHelper)
        {
            this.jwtTokenHelper = jwtTokenHelper;
        }

        public async Task LoginAsync(string account, string password)
        {
            HttpClient client = new HttpClient();

            LoginRequestDto loginRequestDto = new LoginRequestDto()
            {
                Account = account,
                Password = password,
            };
            var responseMessage = await client.PostAsJsonAsync<LoginRequestDto>("http://172.21.192.1:5199/api/Login", loginRequestDto);
            if (responseMessage.IsSuccessStatusCode)
            {
                APIResult apiResult = await responseMessage.Content.ReadFromJsonAsync<APIResult>();
                if (apiResult.Status == true)
                {
                    LoginResponseDto loginResponseDto = JsonConvert
                        .DeserializeObject<LoginResponseDto>(apiResult.Payload.ToString());
                    await jwtTokenHelper.WriteAsync(loginResponseDto.Token);
                }
                else
                {
                    await jwtTokenHelper.WriteAsync(string.Empty);
                }
            }
            else
            {
                await jwtTokenHelper.WriteAsync(string.Empty);
            }
        }

        public async Task<(string users,string message)> GetUsersAsync()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtTokenHelper.Token);
            var responseMessage = await client.GetAsync("http://172.21.192.1:5199/api/JwtAuth/NeedAuth");
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
                    return (result,string.Empty);
                }
                else
                    return (string.Empty, apiResult.Message);
            }
            else
            {
                APIResult apiResult = await responseMessage.Content.ReadFromJsonAsync<APIResult>();
                Console.WriteLine($"發生錯誤 : {apiResult.Message}");
                return (string.Empty, apiResult.Message);
            }
        }
    }
}
