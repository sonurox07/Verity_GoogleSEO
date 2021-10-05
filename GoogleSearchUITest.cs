using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;
using System;

namespace GoogleSEO
{
    [TestClass]
    public class GoogleSearchUITest

    {
        private const string url = "https://www.britannica.com/art/literature";
        private readonly By googleSearchBar = By.Name("q");
        private readonly By GoogleSearchResultSoftware = By.PartialLinkText("Wikipedia");
        private readonly By GoogleSearchResultLiterature = By.PartialLinkText("literature | Definition, Characteristics, Genres, Types, & Facts");
        private readonly IWebDriver webDriver = new ChromeDriver();

        [TestInitialize]
        public void Setup()
        {
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl("https://www.google.com/");
        }

        [TestMethod]
        public void TestSoftwareWikepediaPageIsReturned()
        {
            //Given Google Search Engine is Open
            //When 'Software' is entered in Search bar
            webDriver.FindElement(googleSearchBar).SendKeys("Software" + Keys.Enter);

            //Then Wikipedia page is present in the search result
            var actualResult = webDriver.FindElement(GoogleSearchResultSoftware);
            Assert.IsTrue(actualResult.Text.Equals("Software - Wikipedia\r\nhttps://en.wikipedia.org › wiki › Software"));
        }


        [TestMethod]
        public void TestBritannicaPageIsReturned()
        {
            // Given Google Search Engine is Open
            //When 'Literature' is entered in Search bar
            webDriver.FindElement(googleSearchBar).SendKeys("Literature" + Keys.Enter);

            //Then Britannica.com is returned
            var actualResult = webDriver.FindElement(GoogleSearchResultLiterature);
            Assert.IsTrue(actualResult.Text.Equals("literature | Definition, Characteristics, Genres, Types, & Facts\r\nhttps://www.britannica.com › Literature › Literary Terms"));

            //And validates Britannica.com is part of the search result
            var webElements = webDriver.FindElements(By.XPath("//div[@class='g']/div/div/div/a"));
            Assert.IsNotNull(webElements.First(element => element.GetAttribute("href") == url).GetAttribute("href"));

            //And displays the index of the Url
            var index = ReturnIndexOfAnUrl(webElements, url);         

            Console.WriteLine($"The index is {index} for the url {url}");
        }

        //search the url and return its index
        public int ReturnIndexOfAnUrl(IList<IWebElement> webElements, string searchUrl)
        {
            return webElements.ToList().FindIndex(x => x.GetAttribute("href").Equals(searchUrl));
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            webDriver.Quit();
        }
    }
}
