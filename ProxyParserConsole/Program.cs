using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ProxyParserConsole
{
    class Program
    {
        // определяет как бота
        // private const string dataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=5&anon=234#list";
        // private const string dataUrl = @"https://spys.one/proxys/US/";
        // private const string dataUrl = @"http://free-proxy.cz/ru/proxylist/country/all/socks5/ping/all";

        // требуется cookies и JavaScript
        // private const string dataUrl = @"https://advanced.name/ru/freeproxy?type=socks5";


        // Через HttpClient работаеет нормально
        private const string dataUrl = @"https://www.luxuryhomemarketing.com/real-estate-agents/advanced_search.html";


        static async Task Main(string[] args)
        {

            //HttpClient client = new HttpClient();
            //var response = await client.GetAsync(dataUrl);
            //var content = await response.Content.ReadAsStringAsync();

            //Console.WriteLine(content);

            var web = new HtmlWeb();
            var doc = web.Load(dataUrl);

            var h1 = doc.DocumentNode
                .SelectNodes("//body//h1")
                .First().InnerHtml;

            Console.WriteLine(h1);

            Console.ReadLine();
        }

    }
}
