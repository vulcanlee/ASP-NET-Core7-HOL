using CommonDomainLayer.Magics;
using DataTransferObjects.Dtos;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// https://www.syncfusion.com/blogs/post/how-to-build-crud-rest-apis-with-asp-net-core-3-1-and-entity-framework-core-create-jwt-tokens-and-secure-apis.aspx

namespace JwtLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtAuthController : ControllerBase
    {
        [Route("NeedAuth")]
        [Authorize(AuthenticationSchemes = MagicObject.JwtBearerAuthenticationScheme)]
        [HttpGet]
        public ActionResult<APIResult> NeedAuth()
        {
            APIResult aPIResult = new APIResult();
            List<MyUserDto> myUsersDto = new();
            foreach (var item in MyUser.GetMyUsers())
            {
                MyUserDto myUserDto = new MyUserDto()
                {
                    Account = item.Account,
                    Name = item.Name,
                    Password = item.Password,
                };
                myUsersDto.Add(myUserDto);
            }
            aPIResult.Payload = myUsersDto;
            return aPIResult;
        }

        [Route("EveryOne")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<APIResult> EveryOne()
        {
            APIResult aPIResult = new APIResult();
            List<MyUserDto> myUsersDto = new();
            foreach (var item in MyUser.GetMyUsers())
            {
                MyUserDto myUserDto = new MyUserDto()
                {
                    Account = item.Account,
                    Name = item.Name,
                    Password = item.Password,
                };
                myUsersDto.Add(myUserDto);
            }
            aPIResult.Payload = myUsersDto;
            return aPIResult;
        }

    }
}
