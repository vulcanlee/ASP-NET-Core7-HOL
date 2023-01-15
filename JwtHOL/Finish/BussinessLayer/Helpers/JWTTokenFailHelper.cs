using BussinessLayer.Factories;
using DataTransferObjects.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Helpers
{
    public class JWTTokenFailHelper
    {
        public static APIResult GetFailResult(Exception exception)
        {
            APIResult apiResult;
            // SecurityTokenValidationException 當接收到的安全性權杖無效時所擲回的例外狀況。
            // https://docs.microsoft.com/zh-tw/dotnet/api/system.identitymodel.tokens.securitytokenvalidationexception?view=netframework-4.8

            var nameis = exception.GetType().Name;
            if (exception is ArgumentException)
            {
                var message = exception.Message;
                if (message.Contains("Unable to decode the header"))
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                        $" 無法進行 JWT 權杖的表頭內容解碼，請與系統管理者聯絡", false);
                }
                else if (message.Contains("Unable to decode the payload"))
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                        $" JWT Payload 負載內容無法解碼、不正確，請與系統管理者聯絡", false);
                }
                else
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                        $" {nameis} {exception.Message}", false);
                }
            }
            else if (exception is SecurityTokenInvalidSignatureException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                         $"  JWT 權杖的數位簽名資訊不正確，請與系統管理者聯絡", false);
            }
            else if (exception is SecurityTokenNotYetValidException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenExpiredException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenReplayDetectedException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    $" {nameis} {exception.Message}", false);
            }
            else if (exception is SecurityTokenValidationException)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    $" {nameis} {exception.Message}", false);
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                    $" {nameis} {exception.Message}", false);
            }
            return apiResult;

        }
    }
}
