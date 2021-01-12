using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WikiSearcher.Pages
{
    public class ArticleWikiPage : IArticleWikiPage
    {
        // Properties
        private readonly IWebDriver _webdriver;
        private static readonly string elementType = "p";
        private static readonly string elementContentClass = "mw-parser-output";

        // Constructor
        public ArticleWikiPage(IWebDriver webDriver)
        {
            _webdriver = webDriver;
        }

        // Elements
        public List<IWebElement> PageParagraphs => _webdriver.FindElement(By.ClassName(elementContentClass)).FindElements(By.TagName(elementType)).ToList();

        // Methods
        public string GetNextPageUrl(List<IWebElement> pageParagraphs)
        {
            string result = null;
            var paragraphElementList = new List<IWebElement>();
            var urlList = new List<string>();
        
            try
            {
                // Loops through paragraphs until finds first nextPageUrl
                for (int i = 0; i < pageParagraphs.Count; i++)
                {
                    paragraphElementList = pageParagraphs[i].FindElements(By.TagName("a")).ToList();

                    if (paragraphElementList.Any())
                    {
                        urlList = GetUrlList(paragraphElementList);
                        break;
                    }
                }

                result = urlList.First();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ReturnKeywordUrl(List<IWebElement> pageParagraphs, string keyword)
        {
            var paragraphElementList = new List<IWebElement>();

            try
            {
                // Loops through paragraphs until finds first keywordUrl
                for (int i = 0; i < pageParagraphs.Count; i++)
                {
                    paragraphElementList = pageParagraphs[i].FindElements(By.CssSelector($"a[title*= {keyword}]")).ToList();
                    if (paragraphElementList.Any())
                        return paragraphElementList.First().GetAttribute("href");
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static List<string> GetUrlList(List<IWebElement> paragraphElementList)
        {
            var urlList = new List<string>();

            foreach (var element in paragraphElementList)
            {
                var url = element.GetAttribute("href");
                if (url.Contains("https://en.wikipedia.org/wiki/"))
                {
                    urlList.Add(url);
                }
            }

            return urlList;
        }
    }
}
