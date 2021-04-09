using System;
using System.Collections.ObjectModel;
using System.IO;
using ProxyParser.Infrastructure;
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
        /// <param name="format">Формат экспорта</param>
        public void Save(string filename, ObservableCollection<ProxyInfo> proxyList, FileExportType format = FileExportType.PlainText)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                {
                    // Выводим шапку
                    if (format == FileExportType.CsvWithSemecolon)
                        sw.WriteLine($"IP; Port; Country; State; City");


                    foreach (var proxy in proxyList)
                    {
                        switch (format)
                        {
                            case FileExportType.PlainText: 
                                sw.WriteLine($"{proxy.Ip}:{proxy.Port}"); 
                                break;
                            case FileExportType.CsvWithSemecolon:
                                sw.WriteLine($"{proxy.Ip}; " +
                                             $"{proxy.Port}; " +
                                             $"{proxy.Country}; " +
                                             $"{proxy.State}; " +
                                             $"{proxy.City}");
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
