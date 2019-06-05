using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExhibitorModule.Models;

namespace ExhibitorModule.Services.Helpers
{
    public static class FeedParser
    {
        public static async Task<List<T>> ParseFeed<T>(string rss) where T : FeedItem, new()
        {
            return await Task.Run(() =>
            {
                var xdoc = XDocument.Parse(rss);

                return (from item in xdoc.Descendants("item")
                        select new T
                        {
                            Id = item.Element("id")?.Value,
                            Title = item.Element("title")?.Value,
                            Description = item.Element("description")?.Value,
                            Link = item.Element("link")?.Value,
                            PublicationDate = ParseDate(item.Element("pubDate")?.Value),
                            Category = item.Element("category")?.Value
                        }).ToList();
            });
        }

        static DateTime ParseDate(string date) 
        {
            return DateTime.Parse(date, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}
