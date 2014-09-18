using DashboardSite.Model;
using System;
using System.Collections.Generic;

namespace DashboardSite.Repository.Interface
{
    public interface IUserSecQuestionRepository
    {
        SecurityQuestionViewModel GetSecQuestionsByUserId(int userId);
        void SaveUserSecQuestions(int userid, SecurityQuestionViewModel securityQuesionModel);
    }
}
