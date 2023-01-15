using DomainLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IMyUserService
    {
        Task<(MyUser, string)> CheckUser(string account, string password);
    }
}