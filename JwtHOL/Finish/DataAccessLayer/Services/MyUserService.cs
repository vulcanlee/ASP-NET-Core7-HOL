using CommonDomainLayer.Enums;
using DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
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
        public async Task<(MyUser, string)>
            CheckUser(string account, string password)
        {
            List<MyUser> users = MyUser.GetMyUsers();
            var checkUser = users.FirstOrDefault(x =>
            x.Account.ToLower() == account &&
            x.Password.ToLower() == password.ToLower());

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
