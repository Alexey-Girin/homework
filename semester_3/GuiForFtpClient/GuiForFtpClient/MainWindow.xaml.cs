using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private async void ListOfFilesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fileInfo = (sender as ListBox).SelectedItem as FileInfo;

            if (fileInfo == null)
            {
                return;
            }

            if (fileInfo.IsDirectory)
            {
                await GetNewDirectory(fileInfo.Name);
                return;
            }

            await DownloadFiles(fileInfo.Name);
        }

        private async Task GetNewDirectory(string path)
        {
            if (path == "...")
            {
                await Back();
                return;
            }

            try
            {
                await client.List(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            pathHistory.Push(path);
        }

        private async Task Back()
        {
            if (pathHistory.Count == 1)
            {
                return;
            }

            try
            {
                pathHistory.Pop();
                await client.List(pathHistory.Peek());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task DownloadFiles(string path)
        {
           /* var errorReport = */await client.DownloadFiles(path);

            //if (errorReport == null)
            //{
            //    return;
            //}

            //MessageBox.Show(errorReport);
        }

        private async void DownloadButtonClick(object sender, RoutedEventArgs e) => await DownloadFiles(null);

        private async void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            client.Reset();
            await GetNewDirectory(defaultPath);
        }
    }
}
