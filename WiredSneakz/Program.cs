using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using CsvHelper;
using WiredSneakz.Models;

namespace WiredSneakz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            var sneakerLinks = GetSneakerLinks("https://stockx.com/new-releases/sneakers");
            Console.WriteLine("Found {0} links", sneakerLinks.Count);
            var sneakers = GetSneakerDetails(sneakerLinks);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        // Parses the URL and returns HtmlDocument object
        // static HtmlDocument GetDocument(string url)
        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<string> GetSneakerLinks(string url)
        {
            var sneakerLinks = new List<string>();
            HtmlDocument doc = GetDocument(url);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//h3/a");
            var baseUri = new Uri(url);
            foreach (var link in linkNodes)
            {
                string href = link.Attributes["href"].Value;
                sneakerLinks.Add(new Uri(baseUri, href).AbsoluteUri);
            }
            return sneakerLinks;
        }

        static List<Sneaker> GetSneakerDetails(List<string> urls)
        {
            var sneakers = new List<Sneaker>();
            foreach (var url in urls)
            {
                HtmlDocument document = GetDocument(url);
                var nameXPath = "/html/body/div[1]/div[1]/main/div/div[2]/div/div[2]/div/div/div/div[2]/div[1]/div/a/div/div[3]/p";
                var imageXPath = "/html/body/div[1]/div[1]/main/div/div[2]/div/div[2]/div/div/div/div[2]/div[1]/div/a/div/div[1]/img/@src";
                var sneaker = new Sneaker();
                sneaker.Name = document.DocumentNode.SelectSingleNode(nameXPath).InnerText;
                sneaker.Image = Convert.ToString(document.DocumentNode.SelectSingleNode(imageXPath));
                sneakers.Add(sneaker);
            }
            return sneakers;
        }
    }
}