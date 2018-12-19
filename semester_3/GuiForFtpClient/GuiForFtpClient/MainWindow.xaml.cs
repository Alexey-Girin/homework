using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleFtpClient;

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

        private const string defaultPath = @"...\";

        private async void ListOfFilesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!((sender as ListBox).SelectedItem is FileInfo fileInfo))
            {
                return;
            }

            if (fileInfo.IsDirectory)
            {
                await clientViewModel.GetDirectory(fileInfo.Path, Dispatcher);
                return;
            }

            clientViewModel.DownloadFiles(fileInfo, Dispatcher);
        }

        private void DownloadButtonClick(object sender, RoutedEventArgs e)
            => clientViewModel.DownloadFiles(null, Dispatcher);

        private async void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            clientViewModel.Reset();
            await clientViewModel.GetDirectory(defaultPath, Dispatcher);
        }
    }
}
