using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    internal partial class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ProxyInfo> _proxyList = new ObservableCollection<ProxyInfo>();
        /// <summary>Список прокси</summary>
        public ObservableCollection<ProxyInfo> ProxyList
        {
            get => _proxyList;
            set => Set(ref _proxyList, value);
        }

        private Queue<string> _urlQueue = new Queue<string>();
        /// <summary>Список прокси</summary>
        public Queue<string> UrlQueue
        {
            get => _urlQueue;
            set => Set(ref _urlQueue, value);
        }



        #region Services

        IDialogService _dialogService;
        IFileService _fileService;
        IParseService _parseSiteHideMyNameService;

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

        #region Status - ParsingStarted

        private bool _parsingStarted = false;

        public bool ParsingStarted
        {
            get => _parsingStarted;
            set => Set(ref _parsingStarted, value);
        }

        #endregion

        #region Status - ParsingPaused

        private bool _parsingPaused = false;

        public bool ParsingPaused
        {
            get => _parsingPaused;
            set => Set(ref _parsingPaused, value);
        }

        #endregion



        public MainWindowViewModel()
        {
            _dialogService = new DefaultDialogService();
            _fileService = new TxtFileService();
            _parseSiteHideMyNameService = new ParseSiteHideMyNameService(ref _proxyList);

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

        //private const string DataUrl = @"https://hidemy.name/en/proxy-list/?maxtime=1500&type=s45&anon=234#list"; // HTTPS, SOCKS4, SOCKS5


    }
}
