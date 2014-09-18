using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using DashboardSite.Model;
using DashboardSite.Core;

namespace DashboardSite.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                try
                {
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Authentication", "UserID", "UserName", autoCreateTables: true);
                    var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
                    var membership = (SimpleMembershipProvider)System.Web.Security.Membership.Provider;

                    if (!roles.RoleExists("Administrator"))
                        roles.CreateRole("Administrator");
                    
                    // super user
                    if (!roles.RoleExists("SuperUser"))
                        roles.CreateRole("SuperUser");

                    // seed super user
                    // replace later
                    if (membership.GetUser("thomas.gan", false) == null)
                    {
                        WebSecurity.CreateUserAndAccount("thomas.gan", "Password1$",
                                                            propertyValues: new
                                             {
                                                 FirstName = "Super",
                                                 LastName = "User",    
                                                 PreferredTimeZone = "3",
                                                 ModifiedBy = -1,   // initial machine generated user
                                                 ModifiedOn = DateTime.UtcNow
                                             });
                        System.Web.Security.Roles.AddUsersToRole(new string[] { "thomas.gan" }, "SuperUser");
                    }
                    if (membership.GetUser("john.krishnappa", false) == null)
                    {
                        WebSecurity.CreateUserAndAccount("john.krishnappa", "Password1$",
                                                            propertyValues: new
                                                            {
                                                                FirstName = "Super",
                                                                LastName = "User",
                                                                PreferredTimeZone = "3",
                                                                ModifiedBy = -1,   // initial machine generated user
                                                                ModifiedOn = DateTime.UtcNow
                                                            });
                        System.Web.Security.Roles.AddUsersToRole(new string[] { "john.krishnappa" }, "SuperUser");
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
