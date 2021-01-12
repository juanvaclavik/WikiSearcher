using OpenQA.Selenium;
using System.Collections.Generic;

namespace WikiSearcher.Pages
{
    public interface IArticleWikiPage
    {
        string GetNextPageUrl(List<IWebElement> pageParagraphs);

        string ReturnKeywordUrl(List<IWebElement> pageParagraphs, string keyword);
    }
}
