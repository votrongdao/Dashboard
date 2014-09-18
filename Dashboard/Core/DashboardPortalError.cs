using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Core
{
    [Serializable]
    public class DashboardError : Exception
    {
        public string DashboardUserContext
        {
            get
            {
                if (UserPrincipal.Current.UserData == null)
                    return string.Format("UserName: {0}, UserId: {1}", "Anonymous", -1);
                else
                    return string.Format("UserName: {0}, UserId: {1}", UserPrincipal.Current.UserName, UserPrincipal.Current.UserId);
            }
        }

        public DashboardError()
        {
        }

        public DashboardError(string message)
            : base(message)
        {
        }

        public DashboardError(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DashboardError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("DashboardUserContext", DashboardUserContext);
            base.GetObjectData(info, context);
        }

    }
}
