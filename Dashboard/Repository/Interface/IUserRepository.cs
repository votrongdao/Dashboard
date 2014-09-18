using System;
using DashboardSite.Model;

namespace DashboardSite.Repository.Interface
{
    public interface IUserRepository
    {
        System.Collections.Generic.IEnumerable<UserViewModel> GetAllUsers();
        System.Collections.Generic.IEnumerable<UserViewModel> GetAllActiveUsers();
        UserViewModel GetUserById(int userId);
        void DeleteUser(int userId);
        void SavePasswordHist(int userId, string passwordHashHist);
        void UpdateUser(UserViewModel model);
    }
}
