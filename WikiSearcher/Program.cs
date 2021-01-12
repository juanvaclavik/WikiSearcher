using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WikiSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome in WikiSearch Application");
            Console.WriteLine("*********************************");
            Console.WriteLine("Insert Wikipedia url in format: https://en.wikipedia.org/wiki/[Article_Name]");
            var url = Console.ReadLine();

            var visitedUrls = new List<string>();
            string keywordUrl;
            var keyword = "Philosophy";
            var appStart = DateTime.Now;

            IWebDriver driver = new ChromeDriver();

            try
            {
                while (true)
                {
                    driver.Navigate().GoToUrl(url);
                    visitedUrls.Add(url);
                    var page = new Pages.ArticleWikiPage(driver);

                    var paragraphs = page.PageParagraphs;
                    if (paragraphs.Any())
                    {
                        keywordUrl = page.ReturnKeywordUrl(paragraphs, keyword);
                        if (!String.IsNullOrEmpty(keywordUrl))
                            // If keywordUrl found, stops looping and prints result
                            break;

                        // Gets next page url
                        var nextPageUrl = page.GetNextPageUrl(paragraphs);
                        if (String.IsNullOrEmpty(nextPageUrl))
                        {
                            Console.WriteLine("Could not find next page url");
                            break;
                        }

                        // Infinite looping check
                        if (visitedUrls.Contains(nextPageUrl))
                        {
                            Console.WriteLine("Application finished, infinite looping started");
                            break;
                        }

                        url = nextPageUrl;
                    }
                }

                Console.WriteLine("*********************************");
                Console.WriteLine("Keyword \'{0}\' URL:\t{1}", keyword, keywordUrl);
                Console.WriteLine($"No. of redirects:\t{visitedUrls.Count}");
                foreach (var visitedUrl in visitedUrls)
                {
                    Console.WriteLine(visitedUrl);
                }
                Console.WriteLine("Application finished successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                // Closes browser window
                driver.Close();
                // Quits webdriver and ends session
                driver.Quit();

                Console.WriteLine($"Total run time: {DateTime.Now - appStart}");
                Console.ReadKey();
            }  
        }
    }
}
