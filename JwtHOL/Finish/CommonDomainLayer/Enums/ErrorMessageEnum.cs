using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomainLayer.Enums
{
    public enum ErrorMessageEnum
    {
        None = 0,
        SecurityTokenExpiredException,
        SecurityTokenReplayDetectedException,
        SecurityTokenNotYetValidException,
        SecurityTokenValidationException,
        AuthenticationFailed,
        客製化文字錯誤訊息,
        // Web API 使用到的錯誤訊息
        帳號或密碼不正確 = 1000,
        權杖中沒有發現指定使用者ID,
        沒有發現指定的該使用者資料,
        傳送過來的資料有問題,
        沒有任何符合資料存在,
        權杖Token上標示的使用者與傳送過來的使用者不一致,
        原有密碼不正確,
        新密碼不能為空白,
        密碼不能為空白,
        Exception = 2147483647,
    }
}
