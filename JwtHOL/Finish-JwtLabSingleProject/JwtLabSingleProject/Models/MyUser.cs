using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLabSingleProject.Models
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class MyUser
    {
        public static List<MyUser> GetMyUsers()
        {
            List<MyUser> myUsers = new List<MyUser>()
            {
                new MyUser()
                {
                    Id = 1,
                    Account = "Tom",
                    Password = "123",
                    Name= "Tom"
                },
                new MyUser()
                {
                    Id = 2,
                    Account = "Emily",
                    Password = "123",
                    Name= "Emily"
                }
            };
            return myUsers;
        }
        public MyUser()
        {
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "帳號 不可為空白")]
        public string Account { get; set; } = String.Empty;
        [Required(ErrorMessage = "密碼 不可為空白")]
        public string Password { get; set; } = String.Empty;
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; } = String.Empty;
        public string? Salt { get; set; }
        public bool Status { get; set; }
    }
}
