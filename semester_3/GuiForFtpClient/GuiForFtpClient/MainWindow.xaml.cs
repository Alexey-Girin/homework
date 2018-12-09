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
        }

        private FtpClient client;

        private Stack<string> pathHistory = new Stack<string>();

        private string defaultPath = @"C:\";

        private void ListOfFilesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = ((sender as ListBox).SelectedItem as FileInfo).Name;

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

        private void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            client = new FtpClient(hostNameTextBlock.Text, int.Parse(hostPortTextBlock.Text));
            listOfFiles.ItemsSource = client.Files;

            client.List(defaultPath);
            pathHistory.Push(defaultPath);
        }
    }
}
