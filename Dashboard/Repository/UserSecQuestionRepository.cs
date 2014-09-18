using DashboardSite.Core;
using DashboardSite.Model;
using DashboardSite.Repository.Interface;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public class UserSecQuestionRepository : BaseRepository<User_SecurityQuestion>, IUserSecQuestionRepository
    {
        public UserSecQuestionRepository()
            : base(new UnitOfWork())
        {
        }
        public UserSecQuestionRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public SecurityQuestionViewModel GetSecQuestionsByUserId(int userId)
        {
            var secQuestions = this.Find(p => p.User_ID == userId && p.Deleted_Ind != false)
                .OrderBy(p => p.SecurityQuestionID)
                .Take(3).ToList();   // only get first 3 security question answers
            var secQuestionViewModel = new SecurityQuestionViewModel();
            if(secQuestions.Count() >= 1)
            {
                secQuestionViewModel.FirstQuestionId = secQuestions[0].SecurityQuestionID.ToString();
                secQuestionViewModel.FirstAnswer = secQuestions[0].Answer;
            }
            if (secQuestions.Count() >= 2)
            {
                secQuestionViewModel.SecondQuestionId = secQuestions[1].SecurityQuestionID.ToString();
                secQuestionViewModel.SecondAnswer = secQuestions[1].Answer;
            }
            if (secQuestions.Count() >= 3)
            {
                secQuestionViewModel.ThirdQuestionId = secQuestions[2].SecurityQuestionID.ToString();
                secQuestionViewModel.ThirdAnswer = secQuestions[2].Answer;
            }
            return secQuestionViewModel;
        }

        public void SaveUserSecQuestions(int userid, SecurityQuestionViewModel securityQuesionModel)
        {
            if(!string.IsNullOrEmpty(securityQuesionModel.FirstQuestionId) && !string.IsNullOrEmpty(securityQuesionModel.FirstAnswer))
            {
                var firstAnswer = new User_SecurityQuestion();
                firstAnswer.SecurityQuestionID = int.Parse(securityQuesionModel.FirstQuestionId);
                firstAnswer.Answer = securityQuesionModel.FirstAnswer;
                firstAnswer.User_ID = userid;
                firstAnswer.ModifiedBy = UserPrincipal.Current.UserData == null ? -1 : UserPrincipal.Current.UserId;
                firstAnswer.ModifiedOn = DateTime.UtcNow;
                this.InsertNew(firstAnswer);
            }
            if (!string.IsNullOrEmpty(securityQuesionModel.SecondQuestionId) && !string.IsNullOrEmpty(securityQuesionModel.SecondAnswer))
            {
                var secondAnswer = new User_SecurityQuestion();
                secondAnswer.SecurityQuestionID = int.Parse(securityQuesionModel.SecondQuestionId);
                secondAnswer.Answer = securityQuesionModel.SecondAnswer;
                secondAnswer.User_ID = userid;
                secondAnswer.ModifiedBy = UserPrincipal.Current.UserData == null ? -1 : UserPrincipal.Current.UserId;
                secondAnswer.ModifiedOn = DateTime.UtcNow;
                this.InsertNew(secondAnswer);
            }
            if (!string.IsNullOrEmpty(securityQuesionModel.ThirdQuestionId) && !string.IsNullOrEmpty(securityQuesionModel.ThirdAnswer))
            {
                var thirdAnswer = new User_SecurityQuestion();
                thirdAnswer.SecurityQuestionID = int.Parse(securityQuesionModel.ThirdQuestionId);
                thirdAnswer.Answer = securityQuesionModel.ThirdAnswer;
                thirdAnswer.User_ID = userid;
                thirdAnswer.ModifiedBy = UserPrincipal.Current.UserData == null ? -1 : UserPrincipal.Current.UserId;
                thirdAnswer.ModifiedOn = DateTime.UtcNow;
                this.InsertNew(thirdAnswer);
            }
        }
    }
}
