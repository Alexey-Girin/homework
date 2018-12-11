using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleFTP_Client;

namespace GuiForFtpClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = client;
        }

        private FtpClient client = new FtpClient();

        private Stack<string> pathHistory = new Stack<string>();

        private string defaultPath = @"C:\";

        private void ListOfFilesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var info = (sender as ListBox).SelectedItem as FileInfo;

            if (info.IsDirectory)
            {
                GetNewDirectory(info.Name);
                return;
            }

            DownloadFile(info.Name);
        }

        private void GetNewDirectory(string path)
        {
            if (path == "...")
            {
                Back();
                return;
            }

            client.List(path);
            pathHistory.Push(path);
        }

        private void Back()
        {
            if (pathHistory.Count == 1)
            {
                return;
            }

            pathHistory.Pop();
            client.List(pathHistory.Peek());
        }

        private void DownloadFile(string path) => client.Get(path);

        private void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            client.List(defaultPath);
            pathHistory.Push(defaultPath);
        }
    }
}
