using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ProxyParser.Models;

namespace ProxyParser.Infrastructure.Interfaces
{
    public interface IParseService
    {
        public Queue<string> UrlQueue { get; set; }
        public string StartPage { get; set; }
        public Task StartAsync();
        public int FindProxyOnCurrentPage(HtmlDocument _htmlDoc);
        public int AddUrlsToQueue(HtmlDocument _htmlDoc);
    }
}
