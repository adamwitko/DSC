using TeamSauce.Models;

namespace TeamSauce.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(string connectionId);
        void CreateUser(string connectionId, string username, string password);
    }
}