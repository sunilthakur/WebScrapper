using HtmlAgilityPack;
using MyWebScrapper.BLL.Core.Interfaces;
using MyWebScrapper.BLL.Core.Models;
using MyWebScrapper.BLL.Models;
using MyWebScrapper.DAL.Entities;
using MyWebScrapper.DAL.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ScrapySharp.Html;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScrapper.BLL.Services
{
    public class SiteScraping : BrowserFactory, ISiteScraping
    {
        public async Task<Core.Models.SiteDetails> ScrapSiteDetails(string url)
        {
            Core.Models.SiteDetails details = new Core.Models.SiteDetails();
            WebPage page = await NavigateToWebPage(url);
            details.Title = GetSiteTitle(page);
            details.SiteName = page.AbsoluteUrl.ToString();
            details.Hyperlinks = GetSiteLinks(page);
            details.SocialMediaLinks = GetSocialMediaLinks(page);
            details.ScreenShot = CaptureScreenShot(url);
            var meta = GetMetaData(page);
            if (meta.Count() > 0)
            {
                details.MetaKeywords = meta.Where(x => x.Name == "keywords").Select(x => x.Content).FirstOrDefault();
                details.MetaDescription = meta.Where(x => x.Name == "description").Select(x => x.Content).FirstOrDefault();
            }
            SaveSite(new MyWebScrapper.DAL.Entities.SiteDetails
            {
                MetaKeywords = details.MetaKeywords,
                MetaDescription = details.MetaDescription,
                SiteImage = details.ScreenShot,
                SiteLinks = details.Hyperlinks,
                SiteSocialLinks = details.SocialMediaLinks,
                SiteName = details.SiteName,
                SiteTitle = details.Title
            });
            return details;
        }

        private async Task<WebPage> NavigateToWebPage(string url)
        {
            return await _browser.NavigateToPageAsync(new Uri(url));
        }

        private string GetSiteTitle(WebPage page)
        {
            HtmlNode title = page.Html.SelectSingleNode("//title");
            if (title != null)
                return title.InnerText;
            return "";
        }
        private List<string> GetSiteLinks(WebPage page)
        {
            List<string> siteLinks = new List<string>();
            var links = page.Html.SelectNodes("//a[@href]").ToList();
            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains(page.AbsoluteUrl.Authority))
                {
                    siteLinks.Add(link.Attributes["href"].Value);
                }
            }
            return siteLinks;
        }
        private List<string> GetSocialMediaLinks(WebPage page)
        {
            var socialFiletr = new string[] {
                    "facebook",
                    "linkedin",
                    "twitter",
                    "youtube",
                    "github",
                    "google plus",
                    "pinterest",
                    "instagram",
                    "snapchat",
                    "flipboard",
                    "flickr",
                    "weibo",
                    "periscope",
                    "telegram",
                    "soundcloud",
                    "feedburner",
                    "vimeo",
                    "slideshare" ,
                    "vkontakte"  ,
                    "xing"};
            List<string> socialLinks = new List<string>();
            var links = page.Html.SelectNodes("//a[@href]").ToList();
            foreach (var link in links)
            {
                var value = link.Attributes["href"].Value.ToString();
                var slink = socialFiletr.FirstOrDefault(x => value.Contains(x));
                if (slink != null)
                    socialLinks.Add(value);
            }

            return socialLinks;
        }
        private string CaptureScreenShot(string url)
        {
            var guid = Guid.NewGuid().ToString() + ".png";
            var directory = Directory.GetCurrentDirectory();
            int sepPos = directory.LastIndexOf("\\");
            var folder = directory.Substring(0, sepPos);
            var actualFolder = folder + "\\MyWebScrapperApp\\ClientApp\\src\\assets";
            string folderPath = Path.Combine(actualFolder, "AppUploads", "Screenshots");
            var filePath = Path.Combine(folderPath, guid);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArgument("--window-position=-32000,-32000");
            IWebDriver driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(d => d.Url.Contains(url));
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(filePath);

            driver.Close();
            return "/AppUploads/Screenshots/" + guid;
        }
        private List<MetaModel> GetMetaData(WebPage page)
        {
            List<MetaModel> lst = new List<MetaModel>();
            var list = page.Html.SelectNodes("//meta");

            foreach (var node in list)
            {
                string content = node.GetAttributeValue("content", "");
                string name = node.GetAttributeValue("name", "");
                if (name == "description" || name == "keywords")
                {
                    var obj = new MetaModel();
                    obj.Name = name;
                    obj.Content = content;
                    lst.Add(obj);
                }
            }
            return lst;
        }
        public void SaveSite(MyWebScrapper.DAL.Entities.SiteDetails model)
        {
            MongoDbContext context = new MongoDbContext();
            context.BooksCollectionName = "SiteDetails";
            context.DatabaseName = "SiteDetailsDb";
            context.ConnectionString = "mongodb+srv://techy_2020:techy_2020@mywebscapercluster.ozi3m.mongodb.net/SiteDetailsDb?retryWrites=true&w=majority";
            MySiteService srv = new MySiteService(context);
            srv.Create(model);
        }
    }
}
