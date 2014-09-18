using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using DashboardSite.Filters;
using RootModel = DashboardSite.Model;
using System.Data;
using System.Configuration;
using DashboardSite.BusinessService;
using DashboardSite.Model;
using DashboardSite.Helpers;
using Core;
using DashboardSite.Core;
using DashboardSite.BusinessService.Interface;  
using DashboardSite.Core.Utilities;
using System.Web;
using DashboardSite.Helpers;
using System.Data.Entity.Validation;
using DashboardSite.Core.ExceptionHandling;



namespace DashboardSite.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        IAccountManager accountMgr;
        ILookupManager lookupMgr;
        IWebSecurityWrapper webSecurity;

        public AccountController() : this(new AccountManager(), new LookupManager(), new WebSecurityWrapper())
        {

        }

        public AccountController(IAccountManager accountManager, ILookupManager lookupManager, IWebSecurityWrapper webSec)
        {
            accountMgr = accountManager;
            lookupMgr = lookupManager;
            webSecurity = webSec;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LogOff();
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel(); 
            return View(model);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [HandleAntiForgeryError]
        [ValidateAntiForgeryToken]
        public ActionResult Login(RootModel.LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            if (webSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                Session["HcpMenu"] = null;
                return RedirectToLocal(string.IsNullOrEmpty(returnUrl) ? "/Chart/Index" : returnUrl);
            }

            if (webSecurity.IsAccountLockedOut(model.UserName, AccountManager.MaxLoginTrials, AccountManager.LockAccountInSeconds))
                return RedirectToAction("SingleMessage", new { message = string.Format("Your account is locked out after {0} failed logins, please contact administrator to unlock your account.", AccountManager.MaxLoginTrials+1) });

            if (webSecurity.UserExists(model.UserName) && !webSecurity.IsConfirmed(model.UserName))
                return RedirectToAction("SingleMessage", new { message = "Please complete registration first. You should have received an email to complete registration." });

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // this corresponds to form authenticate in log in
            FormsAuthentication.SignOut();
            if(Session != null && Session["HcpMenu"] != null)
                Session["HcpMenu"] = null;
            if(Session != null)
                Session.RemoveAll();

            webSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult SingleMessage(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [AllowAnonymous]
        public ActionResult SingleMessageWithButton(string message, string strController, string strAction, string strButton)
        {
            var html = HtmlHelpers.ButtonAction(this.ControllerContext, strController, strAction, strButton);
            ViewBag.Message = message;
            ViewBag.HtmlRaw = html;
            //Elmah.ErrorLog.GetDefault(this.ControllerContext.HttpContext.ApplicationInstance.Context).Log(new Elmah.Error(new Exception(html.ToString())));
            return View("SingleMessage");
        }

        [HttpGet]
        public JsonResult UnlockUser(int userId)
        {
            bool result = false;
            try
            {
                accountMgr.UnlockUser(userId);
                result = true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                throw new InvalidOperationException(string.Format("Unlock user id: {0} failed.", userId), ex.InnerException);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteUser(int userId)
        {
            bool result = false;
            try
            {
                accountMgr.DeleteUser(userId);
                result = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Unlock user id: {0} failed.", userId), ex.InnerException);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult ListUsers()
        //{
        //    var model = accountMgr.GetDataAdminManageUser();
        //    return View(model);
        //}

        [HttpGet]
        public ActionResult ChangePasswordFromMenu()
        {
            return View("ChangePassword");
        }

        public ActionResult ChangePassword() 
        {
            return PartialView("_ChangePasswordPartial");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        { 
            bool result = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (accountMgr.IsPasswordInHistory(UserPrincipal.Current.UserName, model.NewPassword))
                    {
                        model.IsChangePasswordSuccess = false;
                    }
                    else
                    {
                        model.IsChangePasswordSuccess = webSecurity.ChangePassword(UserPrincipal.Current.UserName, model.OldPassword, model.NewPassword);
                        TempData["ChangePasswordSuccess"] = model.IsChangePasswordSuccess;
                        result = model.IsChangePasswordSuccess;
                    }

                    // FIFO password history pipe
                    if(model.IsChangePasswordSuccess)
                    {
                        accountMgr.SavePasswordHist(UserPrincipal.Current.UserName, model.NewPassword);
                    }
               }
                catch (MembershipPasswordException e)
                {
                    ModelState.AddModelError("ChangePasswordErr", e.Message);
                    throw new InvalidOperationException(string.Format("Error happened when changing password"), e.InnerException);
                }
            }
            if (result)
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            else
                return PartialView("_ChangePasswordPartial", model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new UserViewModel();
            //model.AllRoles.Add(new SelectListItem() { Text = "Select Role", Value = "Select Role" });
            //Roles.GetAllRoles().ToList().ForEach(p => model.AllRoles.Add(new SelectListItem() { Text = p, Value = p }));
            
            var newRoles = SessionHelper.SessionGet<List<string>>(SessionHelper.SESSION_KEY_NEW_ROLES);
               
            model.AllStates.Add(new SelectListItem() { Text = "Select State", Value = "Select" });
            accountMgr.GetAllStates().ToList().ForEach(p => model.AllStates.Add(new SelectListItem() { Text = p.Value, Value = p.Key }));
            return View(model);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model)
        {
            if (webSecurity.UserExists(model.UserName))
            {
                ModelState.AddModelError("UserExistsErr", "User with same user name already exists.");
                TempData["UserExists"] = true;
            }
            if (ModelState.IsValid)
            { 
                // Attempt to register the user
                try
                {
                    // randomly generate strong password, 9 characters long
                    model.Password = RandomPassword.Generate(9); 
                    string userToken = accountMgr.CreateUser(model);
                    int userId = webSecurity.GetUserId(model.UserName);
                    accountMgr.InitForceChangePassword(userId);
                    // save encrypted password history
                    accountMgr.SavePasswordHist(model.UserName, model.Password);
                    var emailMgr = new EmailManager();
                    string confirmLink = this.Url.Action("AgreementConfirm", "Account", new { id = userToken }, this.Request.Url.Scheme);
                    //emailMgr.SendRegisterUserEmail(model, confirmLink);
                    return RedirectToAction("ListUsers"); 
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        ExceptionManager.WriteToEventLog(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State), "Application", System.Diagnostics.EventLogEntryType.Error);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            ExceptionManager.WriteToEventLog(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage), "Application", System.Diagnostics.EventLogEntryType.Error);
                        }
                    }
                    throw;
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(string.Format("Error from creating user: {0}", e.InnerException.ToString()), e.InnerException);
                }
            }

            // If we got this far, something failed, redisplay form
            model = new UserViewModel();
            ModelState.Clear();

            //Roles.GetAllRoles().ToList().ForEach(p => model.AllRoles.Add(new SelectListItem() { Text = p, Value = p }));

            // get all time zone
            model.AllStates.Add(new SelectListItem() { Text = "Select State", Value = "Select" });
            accountMgr.GetAllStates().ToList().ForEach(p => model.AllStates.Add(new SelectListItem() { Text = p.Value, Value = p.Key }));
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RegisterConfirm()
        {
            if(!Url.RequestContext.RouteData.Values.ContainsKey("id"))
                return RedirectToAction("SingleMessage", new { message = "Your are not allowed to access this page." });
            string userToken = Url.RequestContext.RouteData.Values["id"].ToString();
            var model = new SecurityQuestionViewModel();
            lookupMgr.GetAllSecurityQuestions().ToList()
                .ForEach(p => model.QuestionList.Add(new SelectListItem() { Text = p.SecurityQuestionDescription, Value = p.SecurityQuestionID.ToString() }));
            // if user token empty, then it is from first register confirm when user hasn't entered answers yet
            if (string.IsNullOrEmpty(userToken))
            {            
                ModelState.AddModelError("", "User tried to access register confirmation page without token");
                return RedirectToAction("SingleMessage", new { message = "Your are not allowed to access this page." });
            }            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterConfirm(SecurityQuestionViewModel model)
        {
            lookupMgr.GetAllSecurityQuestions().ToList()
                .ForEach(p => model.QuestionList.Add(new SelectListItem() { Text = p.SecurityQuestionDescription, Value = p.SecurityQuestionID.ToString() }));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                string userToken = Url.RequestContext.RouteData.Values["id"].ToString();
                // get user name from user token
                var userId = accountMgr.GetUserIdByToken(userToken);
                if (!userId.HasValue)
                    return RedirectToAction("SingleMessage", new { message = "Your url is wrong." });
                var user = accountMgr.GetUserById(userId.Value);
                if (webSecurity.IsConfirmed(user.UserName))
                    return RedirectToAction("SingleMessage", new { message = "You finished registration confirmation already." });
                // model.Username = user.UserName;
                if (webSecurity.ConfirmAccount(userToken))
                {
                    // save security question answers
                    accountMgr.SaveUserSecQuestions(userId.Value, model);
                    //return RedirectToAction("SingleMessage", new { message = "Registration complete!" });
                    return RedirectToAction("SingleMessageWithButton", new
                    {
                        message = "Registration complete!",
                        strController = "Account",
                        strAction = "Login",
                        strButton = "Go to login"
                    });
                }
                else
                {
                    //return RedirectToAction("SingleMessage", new { message = "Registration failed." });
                    return RedirectToAction("SingleMessageWithButton", new
                    {
                        message = "Registration failed.",
                        strController = "Account",
                        strAction = "Login",
                        strButton = "Go to login"
                    });
                }
            }
        }

        [HttpPost]
        public ActionResult RegisterUserStep1(UserViewModel model)
        {          
            
            return View(model);
        }

        [HttpGet]
        public ActionResult RegisterServicer()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterLenderAcctRep1()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterLenderAcctRep2()
        {
            return View();
        }

        public ActionResult SelectFhasWithinLenders()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AgreementConfirm()
        {
            LogOff(); 
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShowAgreement()
        {
            string agreemntFilePath = Server.MapPath(Url.Content("~/Content/AppFiles/RulesOfBehavior.docx"));
            return File(agreemntFilePath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgetPassword(string userName)
        {
            var userId = webSecurity.GetUserId(userName);
            if(userId < 0)
            {
                ViewBag.Message = "Please enter the User Name.";
                return View();
            }
            var model = accountMgr.GetSecQuestionsByUserId(userId);
            model.IsAnswerStep = false;
            model.UserName = userName;
            lookupMgr.GetAllSecurityQuestions().ToList()
                .ForEach(p => model.QuestionList.Add(new SelectListItem() { Text = p.SecurityQuestionDescription, Value = p.SecurityQuestionID.ToString() }));
            return View("RetrievePassword", model);
        }

        [HttpPost]
        [AllowAnonymous]        
        [ValidateAntiForgeryToken]
        public ActionResult RetrievePassword(SecurityQuestionViewModel model)
        {
            model.IsAnswerStep = false;
            lookupMgr.GetAllSecurityQuestions().ToList()
                .ForEach(p => model.QuestionList.Add(new SelectListItem() { Text = p.SecurityQuestionDescription, Value = p.SecurityQuestionID.ToString() }));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (model.FirstAnswer.Equals(model.UserAnswer1)
                     && model.SecondAnswer.Equals(model.UserAnswer2)
                     && model.ThirdAnswer.Equals(model.UserAnswer3))
                {
                    // reset password token expires in 2 days
                    string resetPasswordToken = webSecurity.GeneratePasswordResetToken(model.UserName, 2880);
                    string resetLink = this.Url.RouteUrl("ResetPwdRoute", new { controller = "Account", action = "ResetPassword", id = resetPasswordToken, usr = model.UserName }, this.Request.Url.Scheme);
                    var emailMgr = new EmailManager();
                    emailMgr.SendResetPasswordEmail(model.UserName, resetLink);
                    //return RedirectToAction("SingleMessage", new { message = "An Email is sent to your account to reset your password." });
                    return RedirectToAction("SingleMessageWithButton", new
                    {
                        message = "An Email is sent to your account to reset your password.",
                        strController = "Account",
                        strAction = "Login",
                        strButton = "Go to login"
                    });
                }
                else
                {
                    ViewBag.Message = "One or more answers are incorrect."; 
                    return View(model);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        { 
            if (!Url.RequestContext.RouteData.Values.ContainsKey("id"))
                return RedirectToAction("SingleMessageWithButton", new { message = "Your are not allowed to access this page.",
                        strController = "Account", strAction = "Login", strButton = "Go to login"});  
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(NewPasswordModel model)
        {
            ViewBag.Message = string.Empty;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                string resetPwdToken = Url.RequestContext.RouteData.Values["id"].ToString();
                string userName = Url.RequestContext.RouteData.Values["usr"].ToString();
                if (accountMgr.IsPasswordInHistory(userName, model.Password))
                {
                    ViewBag.Message = "Cannot use past 8 passwords in history.";
                    return View(model);
                }
                
                if (webSecurity.ResetPassword(resetPwdToken, model.Password))
                {
                    // save FIFO password in history
                    accountMgr.SavePasswordHist(userName, model.Password);
                    return RedirectToAction("SingleMessageWithButton", new
                    {
                        message = "Password reset.",
                        strController = "Account",
                        strAction = "Login",
                        strButton = "Go to login"
                    });
                }
                else
                {
                    return RedirectToAction("SingleMessageWithButton", new
                    {
                        message = "Reset password failed.",
                        strController = "Account",
                        strAction = "Login",
                        strButton = "Go to login"
                    });
                }
            }
        }

        /// <summary>
        /// to make breadcrumb display properly on second level
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DoNothing()
        {
            return new EmptyResult();
        }


        //autocomplete for search by firstname/lastname
        [HttpGet]
        public JsonResult SearchByFirstOrLastName(String searchText, int maxResults)
        {
            
            List<UserViewModel> dataSet = new List<UserViewModel>();
            dataSet = accountMgr.GetAllUsers().ToList();
            var results = dataSet.Where(p => (p.FirstName.StartsWith(searchText) || p.LastName.StartsWith(searchText))).Take(maxResults).OrderBy(p => p.FirstName)
                    .Select(p => new { FirstName = p.FirstName , LastName = p.LastName});
            var json = Json(results, JsonRequestBehavior.AllowGet);
            return json;
        }

        //Get details of the selected user from autocomplete search
        [HttpGet]
        public ActionResult GetUserByFirstAndLastName(String firstAndLastName)
        {
            List<UserViewModel> dataSet = new List<UserViewModel>();            
            string [] name = firstAndLastName.Split(new Char[]{','});
            dataSet = accountMgr.GetAllUsers().ToList();
            var results = dataSet.Where(p => (p.FirstName == name[0] && p.LastName == name[1]));
            return PartialView("ListUsers", results);
        }

        //Go to EditUser partial view
        [HttpGet]
        public ActionResult GoToEditUserView(String id)
        {
            List<UserViewModel> dataSet = new List<UserViewModel>();
            var results = accountMgr.GetUserById(Convert.ToInt32(id));
            
            results.AllStates.Add(new SelectListItem() { Text = "Select State", Value = "Select" });
            accountMgr.GetAllStates().ToList().ForEach(p => results.AllStates.Add(new SelectListItem() { Text = p.Value, Value = p.Key }));
            return View("EditUser", results);
        }

        //saves the updated user details
        [HttpPost]
        public ActionResult SaveEditUser(UserViewModel model)
        {
            accountMgr.UpdateUser(model);
            return RedirectToAction("ListUsers");
        }

        #region Helpers
        
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        //internal class ExternalLoginResult : ActionResult
        //{
        //    public ExternalLoginResult(string provider, string returnUrl)
        //    {
        //        Provider = provider;
        //        ReturnUrl = returnUrl;
        //    }

        //    public string Provider { get; private set; }
        //    public string ReturnUrl { get; private set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //    }
        //}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
