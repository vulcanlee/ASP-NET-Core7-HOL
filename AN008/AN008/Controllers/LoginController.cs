using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AN008.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtHelper jwtHelper;

        public LoginController(JwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> GetAsync(MyUserRequestDto user)
        {
            await Task.Yield();
            if(!(user.Account.ToLower() == "user" && user.Password.ToLower() == "password"))
            {
                return "帳號或者密碼不正確";
            }
            
            string accessToken = jwtHelper.GenerateAccessToken(user.Account);
            return accessToken;
        }

        [Authorize(Roles = "Administrator")]
        [Route("NeedRootAccessToken")]
        [HttpGet]
        public string NeedRootAccessToken()
        {
            return $" ~~ 需要管理者存取權杖的 API 已經執行完畢";
        }

        [Authorize]
        [Route("NeedAccessToken")]
        [HttpGet]
        public string NeedAccessToken()
        {
            return $" ~~ 需要存取權杖的 API 已經執行完畢";
        }

        [AllowAnonymous]
        [Route("NotNeedAccessToken")]
        [HttpGet]
        public string NotNeedAccessToken()
        {
            return $"不需要存取權杖的 API 已經執行完畢";
        }

    }
}
