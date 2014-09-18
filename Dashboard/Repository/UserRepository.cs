using DashboardSite.Core;
using DashboardSite.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using DashboardSite.Repository.Interface;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public class UserRepository : BaseRepository<Authentication>, IUserRepository
    {
        public UserRepository()
            : base(new UnitOfWork())
        {
        }
        public UserRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public UserViewModel GetUserById(int userId)
        {
            var addressRepo = new AddressRepository(this.UnitOfWork);
            var user = this.GetByIDFast(userId);
            var userModel = Mapper.Map<UserViewModel>(user);
            userModel.AddressModel = userModel.AddressID.HasValue ? addressRepo.GetAddressByID(userModel.AddressID.Value) : new AddressModel();
            
            return userModel;
        }
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var context = this.Context as DashboardEntities;
            var addressRepo = new AddressRepository(this.UnitOfWork);
            if (context == null)
                throw new InvalidCastException("context is not from db live in GetDataUploadSummaryByUserId, please pass in correct context in unit of work.");
            var userAll = context.usp_Get_Admin_ManageUsers_View().ToList();
            // should just show active users, add deleted_ind to soft delete
            // var userAll = this.GetAll();
            var users = Mapper.Map<IEnumerable<UserViewModel>>(userAll);
            foreach (var item in users)
            {
                item.AddressModel = item.AddressID.HasValue ? addressRepo.GetAddressByID(item.AddressID.Value) : new AddressModel();                
            }
            return users;
        }

        public IEnumerable<UserViewModel> GetAllActiveUsers()
        {
            return GetAllUsers().Where(p => p.Deleted_Ind != true);
        }
        
        /// <summary>
        /// soft delete by setting deleted indicator to true
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUser(int userId)
        {
            //this.Delete(userId);
            var user = this.GetByIDFast(userId);
            user.Deleted_Ind = true;
        }

        public void SavePasswordHist(int userId, string passwordHashHist)
        {
            var user = this.GetByIDFast(userId); 
            user.PasswordHist = passwordHashHist;
        }

        public void UpdateUser(UserViewModel model)
        {
            var user = this.GetByIDFast(model.UserID);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            // is register complete is from websecurity, don't set
            //user.IsRegisterComplete = model.IsRegisterComplete;
            if(!string.IsNullOrEmpty(model.PasswordHist))
                user.PasswordHist = model.PasswordHist;
            user.PreferredTimeZone = model.SelectedTimezoneId;
            this.Update(user);
        }
    }
}