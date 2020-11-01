using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebScrapper.BLL.Core.Models
{
    public class SiteDetails
    {
        public string SiteName { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public List<string> Hyperlinks { get; set; }
        public List<string> SocialMediaLinks { get; set; }
        public string ScreenShot { get; set; }
    }
}
