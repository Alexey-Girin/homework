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
            DataContext = clientViewModel;
        }

        private ClientViewModel clientViewModel = new ClientViewModel();

        private string defaultPath = @"C:\";

        private async void ListOfFilesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!((sender as ListBox).SelectedItem is FileInfo fileInfo))
            {
                return;
            }

            if (fileInfo.IsDirectory)
            {
                await clientViewModel.GetDirectory(fileInfo.Name);
                return;
            }

            clientViewModel.DownloadFiles(fileInfo.Name);
        }

        private void DownloadButtonClick(object sender, RoutedEventArgs e) => clientViewModel.DownloadFiles(null);      

        private async void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            clientViewModel.Reset();
            await clientViewModel.GetDirectory(defaultPath);
        }
    }
}
