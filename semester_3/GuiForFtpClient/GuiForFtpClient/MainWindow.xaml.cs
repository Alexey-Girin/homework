using System;
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
            var fileInfo = (sender as ListBox).SelectedItem as FileInfo;

            if (fileInfo == null)
            {
                return;
            }

            if (fileInfo.IsDirectory)
            {
                GetNewDirectory(fileInfo.Name);
                return;
            }

            DownloadFile(fileInfo.Name);
        }

        private void GetNewDirectory(string path)
        {
            if (path == "...")
            {
                Back();
                return;
            }

            try
            {
                client.List(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            pathHistory.Push(path);
        }

        private void Back()
        {
            if (pathHistory.Count == 1)
            {
                return;
            }

            try
            {
                pathHistory.Pop();
                client.List(pathHistory.Peek());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void DownloadFile(string path)
        {
            try
            {
                client.DownloadFile(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            var errorReport = client.DownloadAllFilesInDirectory();

            if (errorReport == null)
            {
                return;
            }

            MessageBox.Show(errorReport);
        }

        private void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            client.Reset();
            GetNewDirectory(defaultPath);
        }
    }
}
