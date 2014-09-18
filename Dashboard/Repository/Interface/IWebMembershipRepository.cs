using DashboardSite.Model;
using System.Collections.Generic;
using System;

namespace DashboardSite.Repository.Interface
{
    public interface IWebMembershipRepository
    {
        void UnlockUserAccount(int userId);
        int? GetUserIdByToken(string userToken);
        int? GetUserIdByResetPwdToken(string resetPwdToken);
        void InitForceChangePassword(int userId);
    }
}
