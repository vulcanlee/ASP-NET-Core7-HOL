using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AN008;

#region JWT 的支援類別 - 用於產生 JWT 權杖
public class JwtHelper
{
    public string GenerateAccessToken(string account)
    {
        #region 建立該 JWT 權杖擁有的聲明 Claim 有哪些
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Sid, account),
            };
        #endregion

        #region 建立 JwtSecurityToken 物件 - 設計用於表示 JSON Web 權杖 (JWT) 的 SecurityToken
        var token = new JwtSecurityToken
        (
            issuer: "http://localhost/",
            audience: "http://localhost/",
            claims: claims,
            expires: DateTime.Now.AddMinutes(60 * 4),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes("!2+#oHfX>L$E#K'^XG>a")),
                    SecurityAlgorithms.HmacSha512)
        );
        #endregion

        #region 為創建和驗證 Json Web 權杖而設計的 SecurityTokenHandler
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        #endregion

        return tokenString;
    }

}
#endregion
