using JwtLabSingleProject.Enums;
using JwtLabSingleProject.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLabSingleProject.Factories
{
    public static class APIResultFactory
    {
        /// <summary>
        /// 直接指定字串作為錯誤訊息，不用透過錯誤訊息列舉
        /// </summary>
        public static APIResult<T> Build<T>(bool aPIResultStatus,
            int statusCodes = StatusCodes.Status200OK, string errorMessage = "",
            object payload = null, string exceptionMessage = "", bool replaceExceptionMessage = true)
        {
            APIResult<T> apiResult = new()
            {
                Status = aPIResultStatus,
                ErrorCode = 0,
                Message = $"{errorMessage}",
                HTTPStatus = statusCodes,
                Payload = (T)payload,
            };
            if (apiResult.ErrorCode == (int)ErrorMessageEnum.Exception)
            {
                apiResult.Message = $"{apiResult.Message}{exceptionMessage}";
            }
            else if (string.IsNullOrEmpty(exceptionMessage) == false)
            {
                if (replaceExceptionMessage == true)
                {
                    apiResult.Message = $"{exceptionMessage}";
                }
                else
                {
                    apiResult.Message += $"{exceptionMessage}";
                }
            }
            return apiResult;
        }
    }
}
