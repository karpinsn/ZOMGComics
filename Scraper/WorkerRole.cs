using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Scraper
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("Scraper entry point called", "Information");

            while (true)
            {
                XPathScraper scraper = new XPathScraper(@"http://xkcd.com", @"/1/", @"//div[@id=""comic""]//img", @"//div[@id=""comic""]//img", @"//a[@accesskey=""n""]");

                do
                {
                    Trace.WriteLine(scraper.ScrapeComic(), "Information");
                    Trace.WriteLine(scraper.ScrapeExtra(), "Information");

                } while (scraper.AdvanceNext());

                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
