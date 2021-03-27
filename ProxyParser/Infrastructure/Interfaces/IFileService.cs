using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ProxyParser.Models;

namespace ProxyParser.Infrastructure.Interfaces
{
    public interface IFileService
    {
        //ObservableCollection<ProxyInfo> Open(string filename);
        void Save(string filename, ObservableCollection<ProxyInfo> proxyList, FileExportType format = FileExportType.plainText);
    }

}
