using DashboardSite.BusinessService.Interface;
using DashboardSite.Core;
using DashboardSite.Core.ExceptionHandling;
using DashboardSite.Core.Utilities;
using DashboardSite.Model;
using DashboardSite.Repository;
using DashboardSite.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using WebMatrix.WebData;

namespace DashboardSite.BusinessService
{
    public class AccountManager : IAccountManager
    {
        private IAddressRepository addressRepository;
        private IUserRepository userRepository;
        private IStatesRepository statesRepository;
        private IWebMembershipRepository webMembershipRepository;
        private IUserSecQuestionRepository userSecQuestionRepository;
        // don't use static unit of work, aka context, context should only last per http request due to entity framework identity pattern
        // may get stale data
        private UnitOfWork unitOfWork;
        private const string PASSWORD_DELIMITER = @"{{}}";
        private const int PASSWORD_HIST_COUNT = 8;

        public static readonly int MaxLoginTrials = int.Parse(ConfigurationManager.AppSettings["MaxLoginTrials"]);
        public static readonly int LockAccountInSeconds = int.Parse(ConfigurationManager.AppSettings["LockAccountInSeconds"]);
        public static readonly int ChangePasswordInDays = int.Parse(ConfigurationManager.AppSettings["ChangePasswordInDays"]);
        public static readonly int DaysToAskChangePassword = int.Parse(ConfigurationManager.AppSettings["DaysToAskChangePassword"]);

        public AccountManager() 
            {
            unitOfWork = new UnitOfWork();
            addressRepository = new AddressRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
            statesRepository = new StatesRepository(unitOfWork);
            webMembershipRepository = new WebMembershipRepository(unitOfWork);
            userSecQuestionRepository = new UserSecQuestionRepository(unitOfWork);
        }        

        /// <summary>
        /// return value is user token for email confirmation of created user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string CreateUser(UserViewModel userModel)
        {
            try
            {
                ExceptionManager.WriteToEventLog(userModel.ToString(), "Application", System.Diagnostics.EventLogEntryType.Error);
                var addressId = addressRepository.AddNewAddress(userModel.AddressModel);
                userModel.AddressID = addressId;

                // store hashed and salted password
                string passwordHist = PasswordHash.CreateHash(userModel.Password);
                ExceptionManager.WriteToEventLog("passwordHist:" + passwordHist, "Application", System.Diagnostics.EventLogEntryType.Error);
                string userToken = WebSecurity.CreateUserAndAccount(userModel.UserName, userModel.Password,
                                                 propertyValues: new
                                                 {
                                                     FirstName = userModel.FirstName,
                                                     LastName = userModel.LastName,
                                                     AddressID = userModel.AddressID,
                                                     PreferredTimeZone = userModel.SelectedTimezone,
                                                     ModifiedBy = UserPrincipal.Current.UserId,
                                                     ModifiedOn = DateTime.UtcNow,
                                                     PasswordHist = passwordHist,
                                                     IsRegisterComplete = false
                                                 },
                                                 requireConfirmationToken: true);
                ExceptionManager.WriteToEventLog("usertoken:" + userToken, "Application", System.Diagnostics.EventLogEntryType.Error);
                //Roles.AddUsersToRole(new string[] { userModel.UserName }, userModel.SelectedRole);

                // retrieve roles from session and save update user roles
                var newRoles = SessionHelper.SessionExtract<List<string>>(SessionHelper.SESSION_KEY_NEW_ROLES);
                if (newRoles.Count > 0)
                    Roles.AddUserToRoles(userModel.UserName, newRoles.ToArray());                
                // retrieve lenders assigned to servicer if user is in role of servicer
                List<int> lenderIdsAssigned = new List<int>();
                List<string> FhasAssigned = new List<string>();
                if(newRoles.Contains("Servicer"))
                {
                    var lendersAssigned = SessionHelper.SessionExtract<IDictionary<int, string>>(SessionHelper.SESSION_KEY_NEW_LENDERS);
                    if (lendersAssigned != null)
                        lendersAssigned.ToList().ForEach(p => lenderIdsAssigned.Add(p.Key));
                }
                if(newRoles.Contains("LenderAccountRep"))
                {
                    var fhasSelected = SessionHelper.SessionExtract<IDictionary<string, string>>(SessionHelper.SESSION_KEY_LAR_FHA_LINKS);
                    if (fhasSelected != null)
                        fhasSelected.ToList().ForEach(p => FhasAssigned.Add(p.Key));
                }

                unitOfWork.Save();
                return userToken;
            }
            catch(Exception e)
            {
                throw new InvalidOperationException("Error happened while creating new user", e.InnerException);
            }            
        }

        public IEnumerable<UserViewModel> ListUsers()
        {
            var users = userRepository.GetAllActiveUsers().ToList(); 
            int maxLoginTrials = int.Parse(ConfigurationManager.AppSettings["MaxLoginTrials"]);
            foreach(var item in users)
            {
                item.IsAccountLocked = WebSecurity.IsAccountLockedOut(item.UserName, AccountManager.MaxLoginTrials, AccountManager.LockAccountInSeconds);
                var roles = Roles.GetRolesForUser(item.UserName);
                if(roles != null && roles.Count() > 0)
                    item.SelectedRoles = TextUtils.TokenDelimitedText(roles, ",");
                item.IsRegisterComplete = WebSecurity.IsConfirmed(item.UserName);
            }
            return users;
        }
        
        public List<KeyValuePair<string, string>> GetAllStates()
        {
            return statesRepository.GetAllStates();
        }

        public UserViewModel GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }

        public void UnlockUser(int userId)
        {
            webMembershipRepository.UnlockUserAccount(userId);
            unitOfWork.Save();
        }

        public void InitForceChangePassword(int userId)
        {
            webMembershipRepository.InitForceChangePassword(userId);
            unitOfWork.Save();
        }

        public int? GetUserIdByToken(string userToken)
        {
            return webMembershipRepository.GetUserIdByToken(userToken);
        }

        public int? GetUserIdByResetPwdToken(string resetPwdToken)
        {
            return webMembershipRepository.GetUserIdByResetPwdToken(resetPwdToken);
        }

        public void DeleteUser(int userId)
        {
            userRepository.DeleteUser(userId);
            unitOfWork.Save();
        }

        public void SavePasswordHist(string userName, string passwordNew)
        {
            int userId = WebSecurity.GetUserId(userName);
            var user = GetUserById(userId);
            string passwordHistHashDb = user.PasswordHist;
            List<string> passwordsHash = new List<string>();
            if(!string.IsNullOrEmpty(passwordHistHashDb))
                passwordsHash = passwordHistHashDb.Split(new string[] { PASSWORD_DELIMITER }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string passwordNewHash = PasswordHash.CreateHash(passwordNew);
            if (passwordsHash.Count < PASSWORD_HIST_COUNT)
            {                
                passwordsHash.Add(passwordNewHash);
            }
            else
            {
                // remove the oldest password hash 
                passwordsHash.RemoveAt(0);
                // add newest password hash
                passwordsHash.Add(passwordNewHash);
            }
            var passwordsHashString = TextUtils.TokenDelimitedText(passwordsHash, PASSWORD_DELIMITER);
            userRepository.SavePasswordHist(userId, passwordsHashString);
            unitOfWork.Save();
        }

        public bool IsPasswordInHistory(string userName, string passwordNew)
        {
            int userId = WebSecurity.GetUserId(userName);
            var user = GetUserById(userId);
            string passwordHistHashDb = user.PasswordHist;
             List<string> passwordsHash = new List<string>();
             if (!string.IsNullOrEmpty(passwordHistHashDb))
                 passwordsHash = passwordHistHashDb.Split(new string[] { PASSWORD_DELIMITER }, StringSplitOptions.RemoveEmptyEntries).ToList();
             else
                 return false;
            //string passwordNewHash = PasswordHash.CreateHash(passwordNew);
            foreach(var passwordHash in passwordsHash)
            {
                if (PasswordHash.ValidatePassword(passwordNew, passwordHash))
                    return true;
            }
            return false;
        }

        public SecurityQuestionViewModel GetSecQuestionsByUserId(int userId)
        {
            return userSecQuestionRepository.GetSecQuestionsByUserId(userId);
        }

        public void SaveUserSecQuestions(int userid, SecurityQuestionViewModel securityQuesionModel)
        {
            userSecQuestionRepository.SaveUserSecQuestions(userid, securityQuesionModel);
            unitOfWork.Save();
        }
        
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public void UpdateUser(UserViewModel model)
        {
                addressRepository.UpdateAddress(model.AddressModel);
                userRepository.UpdateUser(model);
                ExceptionManager.WriteToEventLog(model.ToString(), "Application", System.Diagnostics.EventLogEntryType.Error);  
                unitOfWork.Save();         
        }
    }
}
