using JwtLabSingleProject.Enums;
using JwtLabSingleProject.Interfaces;
using JwtLabSingleProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace JwtLabSingleProject.Services
{
    public class MyUserService : IMyUserService
    {
        private readonly ILogger<MyUserService> logger;
        #region 欄位與屬性
        #endregion

        #region 建構式
        public MyUserService(ILogger<MyUserService> logger)
        {
            this.logger = logger;
        }
        #endregion

        #region 其他服務方法
        public async Task<MyUser> GetAsync(int id)
        {
            List<MyUser> users = MyUser.GetMyUsers();
            var checkUser = users.FirstOrDefault(x =>
            x.Id == id);
            await Task.Yield();
            if (checkUser == null)
            {
                return new MyUser();
            }
            else
            {
                return checkUser;
            }

        }
        public async Task<(MyUser, string)>
            CheckUserAsync(string account, string password)
        {
            List<MyUser> users = MyUser.GetMyUsers();
            var checkUser = users.FirstOrDefault(x =>
            x.Account.ToLower() == account.ToLower() &&
            x.Password == password);

            await Task.Yield();

            if (checkUser != null)
            {

            }
            else
            {
                return (null, "帳號或密碼不正確");
            }
            return (checkUser, "");
        }

        #endregion
    }
}
