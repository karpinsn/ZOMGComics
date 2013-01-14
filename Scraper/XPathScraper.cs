using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    class XPathScraper
    {
        private Uri _domain;
        private Uri _currentLocation;
        private HtmlWeb _web;

        //  Used for the XPath scraping
        private readonly string _comicXPath;
        private readonly string _nextXPath;
        private readonly string _extraXPath;

        public XPathScraper(string domain, string startingPath, string comicXPath, string extraXPath, string nextXPath)
        {
            _comicXPath = comicXPath;
            _extraXPath = extraXPath;
            _nextXPath = nextXPath;

            _web = new HtmlWeb();
            _domain = new Uri(domain, UriKind.Absolute);
            _currentLocation = new Uri(startingPath, UriKind.Relative);
        }

        public bool AdvanceNext()
        {
            var page = new Uri(_domain, _currentLocation);
            var doc = _web.Load(page.ToString());

            string location = _ScrapeNextLink(doc);
            _currentLocation = new Uri(location, UriKind.Relative);

            return !String.IsNullOrEmpty(location) && _currentLocation.IsWellFormedOriginalString();
        }

        public string ScrapeComic()
        {
            string comicURL = string.Empty;

            var doc = _GetCurrentDocument();
            if (null == doc)
            {
                return comicURL;
            }

            //  Start by scraping the xpath
            var nodes = doc.DocumentNode.SelectNodes(_comicXPath);
            if (1 <= nodes.Count)
            {
                //  Our nodes must have a src attribute (the actual comic)
                var possibleNodes = from node in nodes
                                    where node.Attributes.Contains("src")
                                    select node;

                //  We have our comic
                if (1 == possibleNodes.Count())
                {
                    comicURL = possibleNodes.First().Attributes["src"].Value;
                }
            }

            return comicURL;
        }

        public string ScrapeExtra()
        {
            string extra = string.Empty;

            var doc = _GetCurrentDocument();
            if (null == doc)
            {
                return extra;
            }

            var nodes = doc.DocumentNode.SelectNodes(_extraXPath);
            if (1 <= nodes.Count)
            {
                var links = from node in nodes
                            where node.Attributes.Contains("title")
                            select node;

                if (1 == links.Count())
                {
                    extra = links.First().Attributes["title"].Value;
                }
            }

            return extra;
        }

        private string _ScrapeNextLink(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectNodes(_nextXPath);
            string nextURL = string.Empty;

            if (1 <= nodes.Count)
            {
                var links = from node in nodes
                            where node.Attributes.Contains("href")
                            select node;

                //  Need to narrow to 1
                if (1 <= links.Count())
                {
                    nextURL = links.First().Attributes["href"].Value;
                }
            }

            return nextURL;
        }

        private HtmlDocument _GetCurrentDocument()
        {
            Uri page = new Uri(_domain, _currentLocation);
            HtmlDocument doc = null;

            if (page.IsWellFormedOriginalString())
            {
                doc = _web.Load(page.ToString());
            }

            return doc;
        }
    }
}
