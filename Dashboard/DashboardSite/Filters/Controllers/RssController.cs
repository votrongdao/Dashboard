using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Security;
using System.IO;

namespace HUDHealthCarePortal.Controllers
{
    public class RssController : Controller
    {
        public ActionResult Index(string feed)
        {
            feed = @"http://www.hud.gov/news/hudrss.xml";
            string errorString = "";

            try
            {
                if (String.IsNullOrEmpty(feed))
                {
                    throw new ArgumentNullException("feed");
                }

                using (XmlReader reader = XmlReader.Create(feed))
                {
                    SyndicationFeed rssData = SyndicationFeed.Load(reader);

                    return View("RssView", rssData);
                }
            }
            catch (ArgumentNullException)
            {
                errorString = "No url for Rss feed specified.";
            }
            catch (SecurityException)
            {
                errorString = "You do not have permission to access the specified Rss feed.";
            }
            catch (FileNotFoundException)
            {
                errorString = "The Rss feed was not found.";
            }
            catch (UriFormatException)
            {
                errorString = "The Rss feed specified was not a valid URI.";
            }
            catch (Exception)
            {
                errorString = "An error occured accessing the RSS feed.";
            }

            var errorResult = new ContentResult();
            errorResult.Content = errorString;
            return errorResult;
        }
    }
}
