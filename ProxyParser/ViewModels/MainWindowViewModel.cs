using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ProxyParser.Infrastructure.Commands;
using ProxyParser.ViewModels.Base;

namespace ProxyParser.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
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

            // Заглушка!!!
            MessageBox.Show("Parsing started");
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
            #region Commands

            StartParsingCommand = new RelayCommand(OnStartParsingCommandExecuted, CanStartParsingCommandExecute);
            PauseParsingCommand = new RelayCommand(OnPauseParsingCommandExecuted, CanPauseParsingCommandExecute);
            StopParsingCommand = new RelayCommand(OnStopParsingCommandExecuted, CanStopParsingCommandExecute);
            ClearParsingResultCommand = new RelayCommand(OnClearParsingResultCommandExecuted, CanClearParsingResultCommandExecute);
            ExportParsingResultCommand = new RelayCommand(OnExportParsingResultCommandExecuted, CanExportParsingResultCommandExecute);

            #endregion

        }
    }

}
