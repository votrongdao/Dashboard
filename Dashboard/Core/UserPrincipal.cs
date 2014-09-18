using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using Core;
using DashboardSite.Core.Interface;

namespace DashboardSite.Core
{
    // use IUserPrincipal is to facilitate unit tests
    public class UserPrincipal : System.Security.Principal.GenericPrincipal, IUserPrincipal
    {
        public const string ANONYMOUS_USER_NAME = "Anonymous";
        
        protected UserData m_oUserData = null;

        public UserPrincipal(IIdentity id, string[] roles)
            : base(id, roles)
        {
        }

        /// <summary>
        /// Get current existing UserPrincipal object.
        /// Create a new UserPrincipal object If it has not been set yet.
        /// </summary>
        public new static UserPrincipal Current
        {
            get
            {
                UserPrincipal oPrincipal = System.Threading.Thread.CurrentPrincipal as UserPrincipal;
                //Check if CurrentPrincipal is already set
                if (oPrincipal != null)
                {
                    return oPrincipal;
                }

                return CreateNewPrincipal();
            }
        }

        /// <summary>
        /// Get current existing UserPrincipal object.
        /// Return null if there is no UserPrincipal initialized
        /// </summary>
        /// <returns></returns>
        public static UserPrincipal ExistingCurrentPrincipal
        {
            get
            {
                UserPrincipal oPrincipal = System.Threading.Thread.CurrentPrincipal as UserPrincipal;
                //Check if CurrentPrincipal is already set
                if (oPrincipal != null)
                {
                    return oPrincipal;
                }
                return null;
            }
        }

        public int UserId
        {
            get { return m_oUserData.UserId; }
        }

        public string UserName
        {
            get { return m_oUserData.UserName; }
        }

        public string FullName
        {
            get { return m_oUserData.FirstName + ' ' +  m_oUserData.LastName; }
        }

        /// <summary>
        /// don't use this, use Roles.GetRolesForUser as we have multiple roles
        /// </summary>
        public string UserRole
        {
            get { return m_oUserData.UserRole; }
        }

        public UserData UserData
        {
            get { return m_oUserData; }
        }

        public bool IsAuthenticated
        {
            get { return System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated;  }
        }

        public override bool IsInRole(string role)
        {
            // will rework this, after change back to .net role provider
            return UserRole == role;
            //return base.IsInRole(role);
        }

        /// <summary>
        /// Initialization routine called to allow the principal object to cache and retrieve the internal UserData object
        /// </summary>
        public void Initialize()
        {
            string sUserName = this.Identity.Name;
            if (string.IsNullOrEmpty(sUserName))
                sUserName = ANONYMOUS_USER_NAME;
            if (UserDataCache.Contains(sUserName))
            {
                Debugger.OutputDebugString("UserPrincipal::Initialize. Get UserData [{0}] from cache.", sUserName);
                m_oUserData = UserDataCache.GetUserData(sUserName);
                if (m_oUserData.IsExpireFromCache())
                {
                    Debugger.OutputDebugString("UserData is expired. Reload from database again.", sUserName);
                    m_oUserData.Initialize(sUserName);
                    UserDataCache.AddUserData(sUserName, m_oUserData);
                }
            }
            else
            {
                Debugger.OutputDebugString("UserPrincipal::Initialize. Create a new UserData [{0}] and add into cache.", sUserName);
                m_oUserData = new UserData(sUserName);
                UserDataCache.AddUserData(sUserName, m_oUserData);
            }
        }

        #region Private Methods

        /// <summary>
        /// Create a new UserPrincipal from current WindowsIdentity
        /// and assign it to System.Threading.Thread.CurrentPrincipal
        /// </summary>
        /// <returns></returns>
        protected static UserPrincipal CreateNewPrincipal()
        {
            //CurrentPrincipal is not set, so create new UserPrincipal
            //UserPrincipal newPrincipal = new UserPrincipal(WindowsIdentity.GetCurrent());
            Debugger.OutputDebugString("Create a new user principal from current identity");
            UserPrincipal newPrincipal = new UserPrincipal(System.Threading.Thread.CurrentPrincipal.Identity, new string[] { });
            //Set CurrentPrincipal if the user is IsAuthenticated
            if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                Debugger.OutputDebugString("Set Thread.CurrentPrincipal to the new UserPrincipal.");
                newPrincipal.Initialize();

                //if (newPrincipal.UserData.DeletedInd.GetValueOrDefault(false))
                //    throw new ApplicationException(
                //        string.Format("User {0} marked as deleted/inactive", newPrincipal.UserName));

                System.Threading.Thread.CurrentPrincipal = newPrincipal;
            }
            return newPrincipal;
        }

        #endregion
    }
}
