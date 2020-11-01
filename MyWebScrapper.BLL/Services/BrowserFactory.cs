using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebScrapper.BLL.Services
{
    public class BrowserFactory
    {
        public static ScrapingBrowser _browser;
        public BrowserFactory()
        {
            _browser = new ScrapingBrowser();
        }
    }
}
