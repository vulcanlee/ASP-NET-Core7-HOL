using JwtLabSingleProject.Models;

namespace JwtLabSingleProject.Interfaces
{
    public interface IMyUserService
    {
        Task<(MyUser, string)> CheckUserAsync(string account, string password);
        Task<MyUser> GetAsync(int id);
    }
}