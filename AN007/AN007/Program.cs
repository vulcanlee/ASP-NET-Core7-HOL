// See https://aka.ms/new-console-template for more information
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

#region 為指定使用者建立一個 JWT 權杖
JwtHelper jwtHelper = new JwtHelper();
var jwt = jwtHelper.GenerateToken(new MyUser()
{
    Account = "001",
    Name = "Vulcan Lee",
    Password = "",
});

Console.WriteLine($"JWT is : {jwt}");
#endregion

#region 使用者類別宣告
public class MyUser
{
    public string Account { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
}
#endregion

#region JWT 的支援類別 - 用於產生 JWT 權杖
public class JwtHelper
{
    public string GenerateToken(MyUser user)
    {
        #region 建立該 JWT 權杖擁有的聲明 Claim 有哪些
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Account.ToString()),
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
