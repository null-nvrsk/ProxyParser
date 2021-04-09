using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ProxyParserConsole
{
    class Program
    {
        private const string DataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=s45&anon=234#list";
        // private const string dataUrl = @"https://spys.one/proxys/US/";
        // private const string dataUrl = @"http://free-proxy.cz/ru/proxylist/country/all/socks5/ping/all";

        // требуется cookies и JavaScript
        // Через Selenium работает
        // private const string dataUrl = @"https://advanced.name/ru/freeproxy?type=socks5";


        // Через HttpClient работаеет нормально
        // private const string dataUrl = @"https://www.luxuryhomemarketing.com/real-estate-agents/advanced_search.html";


        static void Main()
        {
            
            ParseWithHttpClient();
            // ParseWithSelenium();

            Console.ReadLine();
        }

        public static async void ParseWithHttpClient()
        {
            // Чистый HTML
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");

            // TODO: добавить еще куки

            var response = await client.GetAsync(DataUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Console.WriteLine(content);

            // Парсинг через HtmlAgilityPack 
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            // //*[@class="table_block"]/table/tbody/tr[1]/td[1]
            var rows = doc.DocumentNode
                .SelectNodes("//*[@class='table_block']/table/tbody/tr");


            Console.WriteLine($"Items: {rows.Count}");
            int rowCount = 0;

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols is null || cols.Count != 7) continue;

                Console.WriteLine($"({++rowCount}) IP = {cols[0].InnerText}");
                Console.WriteLine($"Port = {cols[1].InnerText}");
                string country = cols[2].SelectSingleNode("//span[@class='country']").InnerText;
                Console.WriteLine($"Country = {country}");

                string city = cols[2].SelectSingleNode("//span[@class='city']").InnerText;
                Console.WriteLine($"City = {city}");

                Console.WriteLine($"Speed = {cols[3].InnerText}");
                Console.WriteLine($"Type = {cols[4].InnerText}");
            }
        }

        public static void ParseWithSelenium()
        {
            IWebDriver browser = new ChromeDriver();
            //Browser.Manage().Window.Maximize();
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Start page
            browser.Navigate().GoToUrl(DataUrl);


            // парсим страницу
            List<IWebElement> items = browser.FindElements(By.CssSelector("div.table_block tbody tr")).ToList();
            Console.WriteLine($"Items: {items.Count}");

            int rowCount = 0;
            foreach (IWebElement row in items)
            {

                //Console.WriteLine($"row = {row}");

                var cols = row.FindElements(By.CssSelector("td"));
                //Console.Write($"({cols.Count}) ");

                if (cols.Count == 7)
                {
                    Console.WriteLine($"({++rowCount}) IP = {cols[0].Text}");
                    Console.WriteLine($"Port = {cols[1].Text}");
                    string country = cols[2].FindElement(By.CssSelector("span.country")).Text;
                    Console.WriteLine($"Country = {country}");

                    string city = cols[2].FindElement(By.CssSelector("span.city")).Text;
                    Console.WriteLine($"City = {city}");

                    //Console.WriteLine($"Country, City = {cols[2].Text}");
                    Console.WriteLine($"Speed = {cols[3].Text}");
                    Console.WriteLine($"Type = {cols[4].Text}");
                }

                //foreach (var col in cols)
                //    Console.Write($"[{col.Text}]");

                Console.WriteLine();
            }

            browser.Quit();
        }

    }
}
