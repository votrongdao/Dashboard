using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages.Html;

namespace DashboardSite.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString SimpleLink(this System.Web.Mvc.HtmlHelper html, string url, string text)
        {
            return new HtmlString(string.Format(@"<a href='{0}' target='_blank'>{1}</a>", url, text));
        }

        public static string Currency(this System.Web.Mvc.HtmlHelper html, decimal data)
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            return data.ToString("C0", culture);
        }

        public static IHtmlString PopupLink(this System.Web.Mvc.HtmlHelper html, string displayName, string paramId)
        {
            var anchor = new TagBuilder("a");
            //anchor.AddCssClass("class");
            anchor.Attributes["src"] = "#";
            anchor.Attributes["onclick"] = string.Format(
                "@TempData['paramId']={0}; $('#dialogCommon').dialog('open'); return false;", paramId);
            anchor.SetInnerText(displayName);
            return new HtmlString(anchor.ToString());
        }
        public static IHtmlString PopupLink2(this System.Web.Mvc.HtmlHelper html, string displayName, string paramId)
        {
            var anchor = new TagBuilder("a");
            //anchor.AddCssClass("class");
            anchor.Attributes["src"] = "#";
            anchor.Attributes["onclick"] = string.Format("openPopupLink({1},'{0}'); return false;", paramId, "'@TempData['" + "paramId" + "']'");
                //"debugger;@TempData['paramId']={0}; $('#dialogCommon').dialog('open'); return false;", paramId);
            anchor.SetInnerText(displayName);
            return new HtmlString(anchor.ToString());
        }
        public static IHtmlString PopupLink3(this System.Web.Mvc.HtmlHelper html, string displayName, string paramId)
        {
            var anchor = new TagBuilder("a");
            //anchor.AddCssClass("class");
            anchor.Attributes["src"] = "#";
            anchor.Attributes["onclick"] = string.Format(
                "debugger;$('#dialogCommon').dialog('open'); return false;", paramId);
            anchor.SetInnerText(displayName);
            return new HtmlString(anchor.ToString());
        }

        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteAction(ControllerContext context, string actionName, string controllerName, object routeValues = null)
        {
            string scheme = context.RequestContext.HttpContext.Request.Url.Scheme;
            var urlHelper = new UrlHelper(context.RequestContext);
            return urlHelper.Action(actionName, controllerName, routeValues, scheme);
        }

        public static IHtmlString ButtonAction(ControllerContext context, string strController, string strAction, string strText)
        {
            string link = AbsoluteAction(context, strAction, strController);
            return new HtmlString(string.Format("<input type='button' value='{0}' onclick='location.href=\"{1}\"' />", strText, link));
        }

        public static MvcHtmlString ImageActionLink(
           this System.Web.Mvc.HtmlHelper helper,
           string imageUrl,
           string altText,
           string actionName,
           string controllerName,
           object routeValues,
           object linkHtmlAttributes,
           object imgHtmlAttributes)
        {
            var linkAttributes = AnonymousObjectToKeyValue(linkHtmlAttributes);
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            //tack on a GUID to prevent AJAX caching
            var routeDictionary = new RouteValueDictionary(routeValues);
            routeDictionary.Add("guid", Guid.NewGuid());
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeDictionary));
            linkBuilder.MergeAttributes(linkAttributes, true);
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this System.Web.Mvc.HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues,
            object imgHtmlAttributes)
        {
            var imgAttributes = AnonymousObjectToKeyValue(imgHtmlAttributes);
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            imgBuilder.MergeAttributes(imgAttributes, true);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this System.Web.Mvc.HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName,
            object routeValues)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName, routeValues));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString ImageActionLink(
            this System.Web.Mvc.HtmlHelper helper,
            string imageUrl,
            string altText,
            string actionName)
        {
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imageUrl);
            imgBuilder.MergeAttribute("alt", altText);
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var linkBuilder = new TagBuilder("a");
            linkBuilder.MergeAttribute("href", urlHelper.Action(actionName));
            var text = linkBuilder.ToString(TagRenderMode.StartTag);
            text += imgBuilder.ToString(TagRenderMode.SelfClosing);
            text += linkBuilder.ToString(TagRenderMode.EndTag);
            return MvcHtmlString.Create(text);
        }

        private static Dictionary<string, object> AnonymousObjectToKeyValue(object anonymousObject)
        {
            var dictionary = new Dictionary<string, object>();
            if (anonymousObject != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
                {
                    dictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(anonymousObject));
                }
            }
            return dictionary;
        }
    }

    public class HtmlHelperExt
    {
        public static IHtmlString ScoreColorFormatter(decimal? scoreValue)
        {
            string sColor = scoreValue >= 20 ? "FF7C80" : scoreValue >= 10 ? "FFFF00" : scoreValue >= 0 ? "92D050" : "FFFFFF";
            return new HtmlString(string.Format("<div class='color' style='display: none;'>#" + sColor + "</div><text>" + scoreValue + "</text>"));
        }

        public static IHtmlString ScoreColorFormatter(decimal? scoreValue, string sFormatter)
        {
            string sColor = scoreValue >= 20 ? "FF7C80" : scoreValue >= 10 ? "FFFF00" : scoreValue >= 0 ? "92D050" : "FFFFFF";
            return new HtmlString(string.Format("<div class='color' style='display: none;'>#" + sColor + "</div><text>" + sFormatter + "</text>", scoreValue));
        }

        /// <summary>
        /// negative number displays with red within brackets
        /// </summary>
        /// <param name="scoreValue"></param>
        /// <param name="sFormatter"></param>
        /// <returns></returns>
        public static IHtmlString NumberFormatter(decimal? value, string sFormatter)
        {
            if (!value.HasValue)
                return new HtmlString(string.Empty);
            string numberFormat = string.Format("#,##{0}; (#,##{0})", sFormatter);
            string sColor = value < 0 ? @"neg-number" : string.Empty;
            return new HtmlString(string.Format("<span class='{0}'>{1}</span>", sColor, value.Value.ToString(numberFormat)));
        }

        public static IHtmlString CurrencyFormatter(decimal? value, string sFormatter)
        {
            if (!value.HasValue)
                return new HtmlString(string.Empty);
            string numberFormat = string.Format("$#,##{0}; ($#,##{0})", sFormatter);
            string sColor = value < 0 ? @"neg-number" : string.Empty;
            return new HtmlString(string.Format("<span class='{0}'>{1}</span>", sColor, value.Value.ToString(numberFormat)));
        }

        public static IHtmlString NumberFormatterStr(string sValue, string sFormatter)
        {
            decimal dValue;
            if (decimal.TryParse(sValue, out dValue))
                return NumberFormatter(dValue, sFormatter);
            else
                return new HtmlString(string.Empty);
        }

        public static IHtmlString CurrencyFormatterStr(string sValue, string sFormatter)
        {
            decimal dValue;
            if (decimal.TryParse(sValue, out dValue))
                return CurrencyFormatter(dValue, sFormatter);
            else
                return new HtmlString(string.Empty);
        }

        public static IHtmlString DateFormatter(DateTime value, string sFormatter)
        {
            return new HtmlString(value.ToString(sFormatter));
        }

        public static IHtmlString DateFormatterStr(string sDateTime, string sFormatter)
        {
            DateTime dt;
            if (DateTime.TryParse(sDateTime, out dt))
                return DateFormatter(dt, sFormatter);
            else
                return new HtmlString(string.Empty);
        }
    }
}