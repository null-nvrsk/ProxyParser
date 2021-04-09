using System.Collections.ObjectModel;
using ProxyParser.Models;

namespace ProxyParser.Infrastructure.Interfaces
{
    public interface IFileService
    {
        //ObservableCollection<ProxyInfo> Open(string filename);
        void Save(string filename, ObservableCollection<ProxyInfo> proxyList, FileExportType format = FileExportType.PlainText);
    }

}
