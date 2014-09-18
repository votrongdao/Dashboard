using Core;
using DashboardSite.Core;
using DashboardSite.Model;
using System;
using System.Collections.Generic;
namespace DashboardSite.BusinessService.Interface
{
    public interface IAccountManager
    {
        string CreateUser(global::DashboardSite.Model.UserViewModel userModel);
        global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>> GetAllStates();
        global::System.Collections.Generic.IEnumerable<global::DashboardSite.Model.UserViewModel> ListUsers();
        void UnlockUser(int userId);
        void DeleteUser(int userId);
        void InitForceChangePassword(int userId);
        global::DashboardSite.Model.UserViewModel GetUserById(int userId);
        void SavePasswordHist(string userName, string passwordNew);
        bool IsPasswordInHistory(string userName, string passwordNew);
        int? GetUserIdByToken(string userToken);
        int? GetUserIdByResetPwdToken(string resetPwdToken);        
        SecurityQuestionViewModel GetSecQuestionsByUserId(int userId);
        void SaveUserSecQuestions(int userid, SecurityQuestionViewModel securityQuesionModel);
        global::System.Collections.Generic.IEnumerable<UserViewModel> GetAllUsers();
        void UpdateUser(global::DashboardSite.Model.UserViewModel model);
    }
}
