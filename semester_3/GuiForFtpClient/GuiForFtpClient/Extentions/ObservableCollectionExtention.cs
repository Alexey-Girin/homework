using SimpleFTP_Client;
using System.Collections.ObjectModel;

namespace GuiForFtpClient.Extentions
{
    public static class ObservableCollectionExtention
    {
        public static void Update(this ObservableCollection<FileInfo> files)
        {
            files.Clear();
            files.Add(new FileInfo("...", true));
        }
    }
}
