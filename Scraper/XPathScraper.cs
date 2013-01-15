using HtmlAgilityPack;
using ScraperUtils;
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
            return _ScrapeAttribute(_comicXPath, "src");
        }

        public string ScrapeExtra()
        {
            return _ScrapeAttribute(_extraXPath, "title");
        }

        private string _ScrapeNextLink(HtmlDocument document)
        {
            return _ScrapeAttribute(_nextXPath, "href");
        }

        private string _ScrapeAttribute(string xPath, string attribute)
        {
            InputValidator.AssertThrowInputNotNullOrEmpty(xPath, "xPath");
            InputValidator.AssertThrowInputNotNullOrEmpty(attribute, "attribute");

            string scrapedAttribute = string.Empty;

            //  Get the current document. If we can't get it we can't scrape anything
            var doc = _GetCurrentDocument();
            if (null == doc)
                return scrapedAttribute;

            //  Scrape according to the xPath. If we dont get anything then we can't scrape
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            if (1 <= nodes.Count)
            {
                //  Try and narrow down based on what the attributes the nodes have
                var possibleNodes = from node in nodes
                                    where node.Attributes.Contains(attribute)
                                    select node;

                //  TODO: Need to narrow to 1
                if (1 <= possibleNodes.Count())
                {
                    scrapedAttribute = possibleNodes.First().Attributes[attribute].Value;
                }
            }

            return scrapedAttribute;
        }

        private HtmlDocument _GetCurrentDocument()
        {
            //  Form the page URI
            Uri page = new Uri(_domain, _currentLocation);
            HtmlDocument doc = null;

            //  If we have a well formed URI then load the page and return it
            if (page.IsWellFormedOriginalString())
            {
                doc = _web.Load(page.ToString());
            }

            return doc;
        }
    }
}
