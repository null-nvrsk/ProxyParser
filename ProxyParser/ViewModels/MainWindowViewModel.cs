using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HtmlAgilityPack;
using ProxyParser.Infrastructure.Commands;
using ProxyParser.Infrastructure.Interfaces;
using ProxyParser.Models;
using ProxyParser.Services;
using ProxyParser.ViewModels.Base;

namespace ProxyParser.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ProxyInfo> _proxyList = new ObservableCollection<ProxyInfo>();
        /// <summary>Список прокси</summary>
        public ObservableCollection<ProxyInfo> ProxyList
        {
            get => _proxyList;
            set => Set(ref _proxyList, value);
        }


        #region Services

        IDialogService _dialogService;
        IFileService _fileService;

        #endregion

        #region Status - total proxy

        private int _proxyTotal;
        /// <summary>Общее кол-во прокси</summary>
        public int ProxyTotal
        {
            get => _proxyTotal;
            set => Set(ref _proxyTotal, value);
        }

        #endregion

        #region Status - good proxy

        private int _proxyGood;
        /// <summary>Кол-во рабочих прокси</summary>
        public int ProxyGood
        {
            get => _proxyGood;
            set => Set(ref _proxyGood, value);
        }

        #endregion

        #region Status - Bad proxy

        private int _proxyBad;
        /// <summary>Кол-во нерабочих прокси</summary>
        public int ProxyBad
        {
            get => _proxyBad;
            set => Set(ref _proxyBad, value);
        }

        #endregion

        #region Status - Elapsed time

        private TimeSpan _timeElapsed = TimeSpan.Zero;
        /// <summary>Прошедшее время</summary>
        public TimeSpan TimeElapsed
        {
            get => _timeElapsed;
            set => Set(ref _timeElapsed, value);
        }

        #endregion

        #region Status - Estimated time

        private TimeSpan _timeEstimated = TimeSpan.Zero;
        /// <summary>Примерное время завершения</summary>
        public TimeSpan TimeEstimated
        {
            get => _timeEstimated;
            set => Set(ref _timeEstimated, value);
        }

        #endregion

        #region Commands

        private bool _parsingStarted;
        private bool _parsingPaused;

        #region StartParsingCommand

        public ICommand StartParsingCommand { get; }

        private void OnStartParsingCommandExecuted(object p)
        {
            _parsingStarted = true;
            _parsingPaused = false;

            _ = ParseSiteHideMyNameAsync(/*ProxyList*/);
        }

        private bool CanStartParsingCommandExecute(object p) => !_parsingStarted;

        #endregion

        #region PauseParsingCommand

        public ICommand PauseParsingCommand { get; }

        private void OnPauseParsingCommandExecuted(object p)
        {
            // Заглушка!!!
            if (!_parsingPaused)
            { 
                _parsingPaused = true;
                MessageBox.Show("Parsing paused");
            }
            else
            {
                _parsingPaused = false;
                MessageBox.Show("Parsing resume");
            }
        }

        private bool CanPauseParsingCommandExecute(object p) => _parsingStarted;

        #endregion

        #region StopParsingCommand

        public ICommand StopParsingCommand { get; }

        private void OnStopParsingCommandExecuted(object p)
        {
            _parsingStarted = false;
            _parsingPaused = false;

            // Заглушка!!!
            MessageBox.Show("Parsing stopped");
        }

        private bool CanStopParsingCommandExecute(object p) => _parsingStarted;

        #endregion

        #region ClearParsingResultCommand

        public ICommand ClearParsingResultCommand { get; }

        private void OnClearParsingResultCommandExecuted(object p)
        {
            ProxyList.Clear();
            ProxyTotal = ProxyList.Count;
        }

        private bool CanClearParsingResultCommandExecute(object p) => (ProxyList.Count > 0);

        #endregion

        #region ExportParsingResultCommand

        public ICommand ExportParsingResultCommand { get; }

        private void OnExportParsingResultCommandExecuted(object p)
        {
            try
            {
                if (_dialogService.SaveFileDialog() != true) return;

                _fileService.Save(_dialogService.FilePath, ProxyList, _dialogService.ExportType);
                _dialogService.ShowMessage("Файл сохранен");
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        private bool CanExportParsingResultCommandExecute(object p) => (ProxyList.Count > 0);

        #endregion


        #endregion


        public MainWindowViewModel()
        {
            _dialogService = new DefaultDialogService();
            _fileService = new TxtFileService();

            //ProxyList.Add(new ProxyInfo { Ip = "8.8.8.8", Port = 80 });
            //ProxyList.Add(new ProxyInfo { Ip = "9.9.9.9", Port = 1080 });
            //ProxyList.Add(new ProxyInfo { Ip = "12.34.56.78", Port = 8080 });

            #region Commands

            StartParsingCommand = new RelayCommand(OnStartParsingCommandExecuted, CanStartParsingCommandExecute);
            PauseParsingCommand = new RelayCommand(OnPauseParsingCommandExecuted, CanPauseParsingCommandExecute);
            StopParsingCommand = new RelayCommand(OnStopParsingCommandExecuted, CanStopParsingCommandExecute);
            ClearParsingResultCommand = new RelayCommand(OnClearParsingResultCommandExecuted, CanClearParsingResultCommandExecute);
            ExportParsingResultCommand = new RelayCommand(OnExportParsingResultCommandExecuted, CanExportParsingResultCommandExecute);

            #endregion
        }

        // private const string dataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=5&anon=234#list"; // SOCKS5
        private const string DataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=s45&anon=234#list"; // HTTPS, SOCKS4, SOCKS5

        private async Task ParseSiteHideMyNameAsync(/*ObservableCollection<ProxyInfo> _proxyList*/)
        {
            // TOOD: Переделать в сервис

            // Чистый HTML
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");

            var response = await client.GetAsync(DataUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Парсинг через HtmlAgilityPack 
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            // //*[@class="table_block"]/table/tbody/tr[1]/td[1]
            var rows = doc.DocumentNode
                .SelectNodes("//*[@class='table_block']/table/tbody/tr");


            Console.WriteLine($"Items: {rows.Count}");

            foreach (var row in rows)
            {
                var cols = row.SelectNodes("td");
                if (cols is null || cols.Count != 7) continue;

                // проверка повтор
                string ip = cols[0].InnerText;
                bool proxyExist = ProxyList.Any(p => p.Ip == ip);

                // Extract integer from string like "380 ms"
                char[] trimChars = { ' ', 'm', 's' };
                int ping = Int32.Parse(cols[3].InnerText.TrimEnd(trimChars));


                if (!proxyExist)
                {
                    // Если новый прокси, то добавляем в список
                    ProxyInfo proxy = new ProxyInfo();

                    proxy.Id = ProxyList.Count + 1;
                    proxy.Ip = ip;
                    proxy.Port = Int32.Parse(cols[1].InnerText);
                    proxy.Country = cols[2].SelectSingleNode("span[@class='country']").InnerText;
                    proxy.City = cols[2].SelectSingleNode(@"span[@class='city']").InnerHtml;
                    proxy.LastPing = ping;

                    // TODO: Extract proxy type (HTTP, SOCKS4, SOCK5)
                    //Console.WriteLine($"Type = {cols[4].InnerText}");

                    ProxyTotal++;
                    ProxyList.Add(proxy);
                }
                else
                {
                    // Если старый прокси - обновляем пинг
                    ProxyList.First(p => p.Ip == ip).LastPing = ping;

                }
            }
        }
    }
}
