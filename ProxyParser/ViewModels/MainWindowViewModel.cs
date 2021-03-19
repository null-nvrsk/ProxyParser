using System;
using System.Collections.Generic;
using System.Text;
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

    }
}
