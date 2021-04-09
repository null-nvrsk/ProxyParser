using System;
using System.Windows;
using System.Windows.Input;

namespace ProxyParser.ViewModels
{
    internal partial class MainWindowViewModel
    {

        #region StartParsingCommand

        public ICommand StartParsingCommand { get; }

        private void OnStartParsingCommandExecuted(object p)
        {
            _parsingStarted = true;
            _parsingPaused = false;

            _ = ParseSiteHideMyNameAsync( /*ProxyList*/);
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
    }
}