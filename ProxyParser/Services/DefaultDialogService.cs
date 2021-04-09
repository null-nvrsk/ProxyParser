using System.Windows;
using Microsoft.Win32;
using ProxyParser.Infrastructure;
using ProxyParser.Infrastructure.Interfaces;

namespace ProxyParser.Services
{
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }
        public FileExportType ExportType { get; set;  }


        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "proxy";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Text documents|*.txt" +
                                    "|CSV (delimiter \";\")|*.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                switch (saveFileDialog.FilterIndex)
                {
                    case 1: ExportType = FileExportType.PlainText; break;
                    case 2: ExportType = FileExportType.CsvWithSemecolon; break;
                    default: ExportType = FileExportType.PlainText; break;
                }
               
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
