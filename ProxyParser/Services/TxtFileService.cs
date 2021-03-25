using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using ProxyParser.Infrastructure.Interfaces;
using ProxyParser.Models;

namespace ProxyParser.Services
{
    public class TxtFileService : IFileService
    {
        /// <summary>
        /// Сохраняем прокси в формате ip:port
        /// </summary>
        /// <param name="filename">Путь к файлу</param>
        /// <param name="proxyList">Массив прокси</param>
        public void Save(string filename, ObservableCollection<ProxyInfo> proxyList)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                {
                    foreach (var proxy in proxyList)
                        sw.WriteLine($"{proxy.Ip}:{proxy.Port}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
