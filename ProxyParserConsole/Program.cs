using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ProxyParserConsole
{
    class Program
    {
        // определяет как бота
        // Через Selenium работает
        private const string dataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=5&anon=234#list";
        // private const string dataUrl = @"https://spys.one/proxys/US/";
        // private const string dataUrl = @"http://free-proxy.cz/ru/proxylist/country/all/socks5/ping/all";

        // требуется cookies и JavaScript
        // Через Selenium работает
        // private const string dataUrl = @"https://advanced.name/ru/freeproxy?type=socks5";


        // Через HttpClient работаеет нормально
        // private const string dataUrl = @"https://www.luxuryhomemarketing.com/real-estate-agents/advanced_search.html";


        static void Main(string[] args)
        {

            // Чистый HTML
            //HttpClient client = new HttpClient();
            //var response = await client.GetAsync(dataUrl);
            //var content = await response.Content.ReadAsStringAsync();

            //Console.WriteLine(content);

            // Парсинг через HtmlAgilityPack
            //var web = new HtmlWeb();
            //var doc = web.Load(dataUrl);

            //var h1 = doc.DocumentNode
            //    .SelectNodes("//body//h1")
            //    .First().InnerHtml;

            //Console.WriteLine(h1);

            IWebDriver browser = new ChromeDriver();
            //Browser.Manage().Window.Maximize();
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Start page
            browser.Navigate().GoToUrl(dataUrl);


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
                    try
                    {
                        string country = cols[2].FindElement(By.CssSelector("span.country")).Text;
                        Console.WriteLine($"Country = {country}");
                    }
                    catch (Exception) { throw; }

                    try
                    {
                        string city = cols[2].FindElement(By.CssSelector("span.city")).Text;
                        Console.WriteLine($"City = {city}");
                    }
                    catch (Exception) { throw; }

                    //Console.WriteLine($"Country, City = {cols[2].Text}");
                    Console.WriteLine($"Speed = {cols[3].Text}");
                    Console.WriteLine($"Type = {cols[4].Text}");
                }

                //foreach (var col in cols)
                //    Console.Write($"[{col.Text}]");

                Console.WriteLine();
            }

            browser.Quit();
            //Console.ReadLine();
        }

    }
}
