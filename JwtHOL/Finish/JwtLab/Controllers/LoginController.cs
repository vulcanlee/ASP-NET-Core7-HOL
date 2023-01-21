using BussinessLayer.Factories;
using BussinessLayer.Helpers;
using CommonDomainLayer.Configurations;
using CommonDomainLayer.Enums;
using CommonDomainLayer.Magics;
using DataAccessLayer.Interfaces;
using DataTransferObjects.Dtos;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtLab.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMyUserService myUserService;
        private readonly JwtGenerateHelper jwtGenerateHelper;
        private readonly JwtConfiguration jwtConfiguration;

        public LoginController(IMyUserService myUserService,
            JwtGenerateHelper jwtGenerateHelper, IOptions<JwtConfiguration> jwtConfiguration)
        {
            this.myUserService = myUserService;
            this.jwtGenerateHelper = jwtGenerateHelper;
            this.jwtConfiguration = jwtConfiguration.Value;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(LoginRequestDto loginRequestDTO)
        {
            APIResult apiResult;
            await Task.Yield();
            if (ModelState.IsValid == false)
            {
                apiResult = APIResultFactory.Build(false,
                    StatusCodes.Status200OK,
                    "傳送過來的資料有問題");
                return Ok(apiResult);
            }

            (MyUser user, string message) =
                await myUserService.CheckUserAsync(loginRequestDTO.Account,
                loginRequestDTO.Password);

            if (user == null)
            {
                apiResult = APIResultFactory.Build(false,
                    StatusCodes.Status400BadRequest, "帳號或密碼不正確");
                return BadRequest(apiResult);
            }

            #region 產生存取權杖與更新權杖
            var claims = new List<Claim>()
            {
                new Claim(MagicObject.ClaimTypeRoleNameSymbol, MagicObject.RoleUser),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };

            #region 授予 Emily 具有管理者角色
            if(user.Account.ToLower() == "emily")
            {
                claims.Add(new Claim(MagicObject.ClaimTypeRoleNameSymbol, MagicObject.RoleAdmin));
            }
            #endregion

            string token = jwtGenerateHelper.GenerateAccessToken(user,
                claims, jwtConfiguration);

            claims = new List<Claim>()
            {
                new Claim(MagicObject.ClaimTypeRoleNameSymbol, MagicObject.RoleRefreshToken),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };
            string refreshToken = jwtGenerateHelper.GenerateRefreshToken(user,
                claims, jwtConfiguration);
            #endregion

            LoginResponseDto LoginResponseDTO = new LoginResponseDto()
            {
                Account = loginRequestDTO.Account,
                Id = user.Id,
                Name = loginRequestDTO.Account,
                Token = token,
                TokenExpireMinutes = jwtConfiguration.JwtExpireMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = jwtConfiguration.JwtRefreshExpireDays,
            };

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
               "", payload: LoginResponseDTO);
            return Ok(apiResult);

        }

        [Authorize(AuthenticationSchemes = MagicObject.JwtBearerAuthenticationScheme,
            Roles = MagicObject.RoleRefreshToken)]
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            APIResult apiResult;
            await Task.Yield();
            LoginRequestDto loginRequestDTO = new LoginRequestDto()
            {
                Account = User.FindFirst(ClaimTypes.Sid)?.Value,
            };

            MyUser user = await myUserService.GetAsync(Convert.ToInt32(loginRequestDTO.Account));
            if (user.Id == 0)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                "沒有發現指定的該使用者資料");
                return BadRequest(apiResult);
            }

            #region 產生存取權杖與更新權杖
            var claims = new List<Claim>()
            {
                new Claim(MagicObject.ClaimTypeRoleNameSymbol, MagicObject.RoleUser),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };

            string token = jwtGenerateHelper.GenerateAccessToken(user,
                claims, jwtConfiguration);

            claims = new List<Claim>()
            {
                new Claim(MagicObject.ClaimTypeRoleNameSymbol, MagicObject.RoleRefreshToken),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };
            string refreshToken = jwtGenerateHelper.GenerateRefreshToken(user, 
                claims, jwtConfiguration);
            #endregion

            LoginResponseDto LoginResponseDTO = new LoginResponseDto()
            {
                Account = loginRequestDTO.Account,
                Id = 0,
                Name = loginRequestDTO.Account,
                Token = token,
                TokenExpireMinutes = jwtConfiguration.JwtExpireMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = jwtConfiguration.JwtRefreshExpireDays,
            };

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
               "", payload: LoginResponseDTO);
            return Ok(apiResult);

        }

    }
}