using AutoMapper;
using DashboardSite.Core;
using DashboardSite.Model;
using DashboardSite.Repository.Interface;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public class WebMembershipRepository : BaseRepository<webpages_Membership>, IWebMembershipRepository
    {
        public WebMembershipRepository()
            : base(new UnitOfWork())
        {
        }
        public WebMembershipRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int? GetUserIdByToken(string userToken)
        {
            var webmember = this.Find(p => p.ConfirmationToken == userToken).FirstOrDefault();
            if (webmember == null)
                return null;
            else
                return webmember.UserId;
        }

        public int? GetUserIdByResetPwdToken(string resetPwdToken)
        {
            var webmember = this.Find(p => p.PasswordVerificationToken == resetPwdToken).FirstOrDefault();
            if (webmember == null)
                return null;
            else
                return webmember.UserId;
        }

        public void UnlockUserAccount(int userId)
        {
            try
            {
                var membershipAccount = this.GetByIDFresh(userId);
                membershipAccount.PasswordFailuresSinceLastSuccess = 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Unlock user id: {0} failed.", userId), ex.InnerException);
            }
        }

        public void InitForceChangePassword(int userId)
        {
            try
            {
                var membershipAccount = this.GetByIDFresh(userId);
                membershipAccount.PasswordChangedDate = new DateTime(2000, 1, 1);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("InitForceChangePassword user id: {0} failed.", userId), ex.InnerException);
            }
        }
    }
}