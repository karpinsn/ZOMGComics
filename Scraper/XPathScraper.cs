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

        public XPathScraper()
        {
            _web = new HtmlWeb();
            _domain = new Uri(@"http://xkcd.com", UriKind.Absolute);
            _currentLocation = new Uri(@"/1/", UriKind.Relative);
        }

        public bool AdvanceNext()
        {
            var page = new Uri(_domain, _currentLocation);
            var doc = _web.Load(page.ToString());

            string location = _ScrapeNextLink(doc);
            _currentLocation = new Uri(location, UriKind.Relative);

            return !String.IsNullOrEmpty(location) && !location.Equals("#");
        }

        public string ScrapeComic()
        {
            var page = new Uri(_domain, _currentLocation);
            var doc = _web.Load(page.ToString());

            return _ScrapeComic(doc);
        }

        private string _ScrapeComic(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectNodes(@"//div[@id=""comic""]//img");
            string comicURL = string.Empty;

            //  Found our node and only our node
            if (1 == nodes.Count)
            {
                //  Make sure we have a source to scrape
                if (nodes[0].Attributes.Contains("src"))
                {
                    comicURL = nodes[0].Attributes["src"].Value;
                }
            }

            return comicURL;
        }

        private string _ScrapeNextLink(HtmlDocument document)
        {
            var nodes = document.DocumentNode.SelectNodes(@"//a[@accesskey=""n""]");
            string nextURL = string.Empty;

            //  Found our node and only our node
            if (1 == nodes.Count)
            {
                //  Make sure we have a source to scrape
                if (nodes[0].Attributes.Contains("href"))
                {
                    nextURL = nodes[0].Attributes["href"].Value;
                }
            }
            else if (1 < nodes.Count)
            {
                //  Hmmm we found multiple nodes. Try and work our way down
                //  TODO - Need to fix this
                var links = (from node in nodes
                            where node.Attributes.Contains("href")
                            select node).FirstOrDefault();

                if (null != links)
                {
                    nextURL = links.Attributes["href"].Value;
                }
            }

            return nextURL;
        }
    }
}
