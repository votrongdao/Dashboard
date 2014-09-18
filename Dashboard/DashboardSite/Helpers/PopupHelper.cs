using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashboardSite.Helpers
{
    public class PopupHelper
    {
        public static void ConfigPopup(TempDataDictionary dict, int winWidth, int winHeight, string sTitle,
            bool bResizable, bool bAutoOpen, bool bModal, bool bDraggable, bool bShowCloseBtn, bool bOpenTilFinish, string sLoadUrl)
        {
            dict["winWidth"] = winWidth;
            dict["winHeight"] = winHeight;
            dict["Title"] = sTitle;
            dict["bResizable"] = bResizable;
            dict["bAutoOpen"] = bAutoOpen;
            dict["bModal"] = bModal;
            dict["bDraggable"] = bDraggable;
            dict["bShowCloseBtn"] = bShowCloseBtn;
            dict["sLoadUrl"] = sLoadUrl;
            HttpContext.Current.Session["bOpenTilFinish"] = bOpenTilFinish;
        }

        public static void ConfigPopup(TempDataDictionary dict, int winWidth, int winHeight, string sTitle,
            bool bResizable, bool bAutoOpen, bool bModal, bool bDraggable, bool bShowCloseBtn, string sLoadUrl)
        {
            dict["winWidth"] = winWidth;
            dict["winHeight"] = winHeight;
            dict["Title"] = sTitle;
            dict["bResizable"] = bResizable;
            dict["bAutoOpen"] = bAutoOpen;
            dict["bModal"] = bModal;
            dict["bDraggable"] = bDraggable;
            dict["bShowCloseBtn"] = bShowCloseBtn;
            dict["sLoadUrl"] = sLoadUrl;
        }
    }
}