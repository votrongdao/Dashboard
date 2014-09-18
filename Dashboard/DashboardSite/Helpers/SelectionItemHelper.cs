using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashboardSite.Helpers
{
    public static class SelectionItemHelper
    {
        public static IEnumerable<SelectListItem> GetSelectItemsFrom(IList<EnumType> enumTypes)
        {
            foreach (var item in enumTypes.ToList())
                yield return new SelectListItem()
                {
                    Value = item.Key,
                    Text = item.Value
                };
        }
    }
}