namespace ProxyParser.Infrastructure.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        public FileExportType ExportType { get; set; }
        bool SaveFileDialog();
    }
}
