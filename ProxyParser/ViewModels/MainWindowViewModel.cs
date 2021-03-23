﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProxyParser.Infrastructure.Commands;
using ProxyParser.Models;
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



        #region Window title

        private string _Title = "Default title";
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion


        #region Status panel

        #region Status - total proxy

        private int _ProxyTotal = 0;
        /// <summary>Общее кол-во прокси</summary>
        public int ProxyTotal
        {
            get => _ProxyTotal;
            set => Set(ref _ProxyTotal, value);
        }

        #endregion

        #region Status - good proxy

        private int _ProxyGood = 0;
        /// <summary>Кол-во рабочих прокси</summary>
        public int ProxyGood
        {
            get => _ProxyGood;
            set => Set(ref _ProxyGood, value);
        }

        #endregion

        #region Status - Bad proxy

        private int _ProxyBad = 0;
        /// <summary>Кол-во нерабочих прокси</summary>
        public int ProxyBad
        {
            get => _ProxyBad;
            set => Set(ref _ProxyBad, value);
        }

        #endregion

        #region Status - Elapsed time

        private TimeSpan _TimeElapsed = TimeSpan.Zero;
        /// <summary>Прошедшее время</summary>
        public TimeSpan TimeElapsed
        {
            get => _TimeElapsed;
            set => Set(ref _TimeElapsed, value);
        }

        #endregion

        #region Status - Estimated time

        private TimeSpan _TimeEstimated = TimeSpan.Zero;
        /// <summary>Примерное время завершения</summary>
        public TimeSpan TimeEstimated
        {
            get => _TimeEstimated;
            set => Set(ref _TimeEstimated, value);
        }

        #endregion

        #endregion

        #region Commands

        private bool _parsingStarted = false;
        private bool _parsingPaused = false;

        #region StartParsingCommand

        public ICommand StartParsingCommand { get; }

        private void OnStartParsingCommandExecuted(object p)
        {
            _parsingStarted = true;
            _parsingPaused = false;

            ParseSiteHideMyName(ProxyList);
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
            // Заглушка!!!
            MessageBox.Show("Parsing result clear");
        }

        private bool CanClearParsingResultCommandExecute(object p) => !_parsingStarted;

        #endregion

        #region ExportParsingResultCommand

        public ICommand ExportParsingResultCommand { get; }

        private void OnExportParsingResultCommandExecuted(object p)
        {
            // Заглушка!!!
            MessageBox.Show("Export parsing result");
        }

        private bool CanExportParsingResultCommandExecute(object p) => !_parsingStarted;

        #endregion


        #endregion


        public MainWindowViewModel()
        {
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

        private const string dataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=5&anon=234#list";

        private void ParseSiteHideMyName(ObservableCollection<ProxyInfo> _proxyList)
        {
            // TOOD: Переделать в сервис

            IWebDriver browser = new ChromeDriver();
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            // Start page
            browser.Navigate().GoToUrl(dataUrl);

            // парсим страницу
            List<IWebElement> items = browser.FindElements(By.CssSelector("div.table_block tbody tr")).ToList();
            Console.WriteLine($"Items: {items.Count}");

            int rowCount = 0;
            foreach (IWebElement row in items)
            {
                var cols = row.FindElements(By.CssSelector("td"));

                if (cols.Count != 7) continue;

                ProxyInfo proxy = new ProxyInfo();

                proxy.Ip = cols[0].Text;
                Console.WriteLine($"({++rowCount}) IP = {proxy.Ip}");

                proxy.Port = Int32.Parse(cols[1].Text);  
                Console.WriteLine($"Port = {proxy.Port}");

                try
                {
                    proxy.Country = cols[2].FindElement(By.CssSelector("span.country")).Text;
                    Console.WriteLine($"Country = {proxy.Country}");
                }
                catch (Exception) { throw; }

                try
                {
                    proxy.City = cols[2].FindElement(By.CssSelector("span.city")).Text;
                    Console.WriteLine($"City = {proxy.City}");
                }
                catch (Exception) { throw; }

                // Extract integer from string like "380 ms"
                char[] trimChars = {' ', 'm', 's'};
                proxy.LastPing = Int32.Parse(cols[3].Text.TrimEnd(trimChars));
                Console.WriteLine($"Ping = {proxy.LastPing}");
                
                // TODO: Extract proxy type (HTTP, SOCKS4, SOCK5)
                Console.WriteLine($"Type = {cols[4].Text}");

                _proxyList.Add(proxy);
            }

            browser.Quit();
        }
    }
}
