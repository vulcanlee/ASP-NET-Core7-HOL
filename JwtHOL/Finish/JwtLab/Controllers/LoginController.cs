using BussinessLayer.Factories;
using CommonDomainLayer.Enums;
using CommonDomainLayer.Magics;
using DataAccessLayer.Interfaces;
using DataTransferObjects.Dtos;
using DomainLayer.Models;
using JwtLab.Models;
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
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly IMyUserService myUserService;
        private readonly JwtConfiguration jwtConfiguration;

        public LoginController(Microsoft.Extensions.Configuration.IConfiguration configuration,
            IMyUserService myUserService, IOptions<JwtConfiguration> tokenConfiguration)
        {
            this.configuration = configuration;
            this.myUserService = myUserService;
            this.jwtConfiguration = tokenConfiguration.Value;
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
                    StatusCodes.Status400BadRequest,"帳號或密碼不正確");
                return BadRequest(apiResult);
            }

            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToken(user);

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

        [Authorize(AuthenticationSchemes = MagicObject.JwtBearerAuthenticationScheme, Roles = MagicObject.RoleRefreshToken)]
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

            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToken(user);

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

        string GenerateToken(MyUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),

            };

            var token = new JwtSecurityToken
            (
                issuer: jwtConfiguration.ValidIssuer,
                audience: jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtConfiguration.JwtExpireMinutes),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(jwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        string GenerateRefreshToken(MyUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, $"RefreshToken"),
            };

            var token = new JwtSecurityToken
            (
                issuer: jwtConfiguration.ValidIssuer,
                audience: jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwtConfiguration.JwtRefreshExpireDays),
                //expires: DateTime.Now.AddMinutes(1),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(jwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}