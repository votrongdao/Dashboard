using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Text;

namespace DashboardSite.Core.Utilities
{
    public class HtmlUtils
    {
        #region private html tag constants
        private const string REG_TAG_PATTERN = @"<(.|\n)*?>";
        #endregion

        public static readonly string NewLine = @"<br/>";

        #region public Methods
        /// <summary>
        /// strips html tags and returns plain text
        /// </summary>
        /// <param name="sBodyHtml">html body</param>
        /// <returns></returns>
        public static string GetHtmlPlainText(string sBodyHtml)
        {
            return HttpUtility.HtmlDecode(Regex.Replace(sBodyHtml, REG_TAG_PATTERN, string.Empty));
        }
        #endregion
    }
}