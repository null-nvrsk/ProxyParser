using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ProxyParser.Infrastructure.Interfaces;
using ProxyParser.Models;

namespace ProxyParser.Services
{
    class ParseSiteHideMyNameService : IParseService
    {
        private readonly ObservableCollection<ProxyInfo> proxyList;
        public ParseSiteHideMyNameService(ref ObservableCollection<ProxyInfo> _proxyList)
        {
            proxyList = _proxyList;
        }

        public Queue<string> UrlQueue { get; set; } = new Queue<string>();
        public string StartPage { get; set; } = "https://hidemy.name/en/proxy-list/?maxtime=1500&type=s45&anon=234#list";

        public async Task StartAsync()
        {
            // В очередь ставим 1ый запрос
            UrlQueue.Enqueue(StartPage);

            // Создаем HttpClient (Чистый HTML)
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");

            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en;q=0.9,en-US;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");


            // В цикле очереди берем запрос и парсим
            while (UrlQueue.Count > 0)
            {
                string currentUrl = UrlQueue.Peek();
                var response = await client.GetAsync(currentUrl);
                var content = await response.Content.ReadAsStringAsync();

                // Парсинг через HtmlAgilityPack 
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);

                // парсим прокси 
                FindProxyOnCurrentPage(htmlDoc);

                // парсим ссылки на другие страницы
                AddUrlsToQueue(htmlDoc);

                UrlQueue.Dequeue();

                // Пауза перед следующим запросом
                if (UrlQueue.Count > 0) await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        public int FindProxyOnCurrentPage(HtmlDocument _htmlDoc)
        {
            // //*[@class="table_block"]/table/tbody/tr[1]/td[1]
            var rows = _htmlDoc.DocumentNode
                .SelectNodes("//*[@class='table_block']/table/tbody/tr");
            if (rows is null) return 0;

            int proxyTotal = 0;
            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols is null || cols.Count != 7) continue;

                // проверка повтор
                string ip = cols[0].InnerText;
                bool proxyExist = proxyList.Any(p => p.Ip == ip);

                // Extract integer from string like "380 ms"
                char[] trimChars = { ' ', 'm', 's' };
                int ping = Int32.Parse(cols[3].InnerText.TrimEnd(trimChars));


                if (!proxyExist)
                {
                    // Если новый прокси, то добавляем в список
                    ProxyInfo proxy = new ProxyInfo();

                    proxy.Id = proxyList.Count + 1;
                    proxy.Ip = ip;
                    proxy.Port = Int32.Parse(cols[1].InnerText);
                    proxy.Country = cols[2].SelectSingleNode("span[@class='country']").InnerText;
                    proxy.City = cols[2].SelectSingleNode(@"span[@class='city']").InnerHtml;
                    proxy.LastPing = ping;

                    // TODO: Extract proxy type (HTTP, SOCKS4, SOCK5)
                    //Console.WriteLine($"Type = {cols[4].InnerText}");

                    proxyTotal++;
                    proxyList.Add(proxy);
                }
                else
                    // Если старый прокси - обновляем пинг
                    proxyList.First(p => p.Ip == ip).LastPing = ping;
            }

            return proxyTotal;
        }

        public int AddUrlsToQueue(HtmlDocument _htmlDoc)
        {
            // //li[@class='next_array']/a
            var nextUrl = _htmlDoc.DocumentNode
                .SelectSingleNode("//li[@class='next_array']/a")?.GetAttributeValue("href", "");
            if (nextUrl is null || nextUrl == "") return 0;

            UrlQueue.Enqueue(nextUrl);
            return 1;
        }
    }
}
