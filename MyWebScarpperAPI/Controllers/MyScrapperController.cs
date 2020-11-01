using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebScrapper.BLL.Core.Interfaces;

namespace MyWebScrapperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyScrapperController : ControllerBase
    {
        private readonly ISiteScraping _siteScrapingService;
        public MyScrapperController(ISiteScraping siteScrapingService)
        {
            _siteScrapingService = siteScrapingService;
        }

        /// <summary>
        /// Get scrape details of give site
        /// </summary>
        /// <param name="url">target site</param>
        /// <returns></returns>
        [HttpGet("{url}")]
        [Route("GetSiteScrapeDetails")]
        public async Task<JsonResult> GetSiteScrapeDetails([FromQuery] string url)
        {
            return new JsonResult(await _siteScrapingService.ScrapSiteDetails(url));
        }
        [HttpGet]
        [Route("checkAPI")]
        public string CheckAPI()
        {
            return "Web Service is working fine";
        }

    }
}