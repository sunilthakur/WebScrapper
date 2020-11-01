using MyWebScrapper.BLL.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScrapper.BLL.Core.Interfaces
{
    public interface ISiteScraping
    {
        Task<SiteDetails> ScrapSiteDetails(string url);
    }
}
