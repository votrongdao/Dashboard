using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Model
{
    public class MenuNodeModel
    {
        public string Title { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public string ParentNode { get; private set; }
        public string Roles { get; private set; }

        public MenuNodeModel(string title, string controller, string action, string parentNode, string roles)
        {
            Title = title;
            Controller = controller;
            Action = action;
            ParentNode = parentNode;
            Roles = roles;
        }
    }
}
