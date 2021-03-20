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
    }

}
