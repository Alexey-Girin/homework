using System.Collections.Generic;
using System.Linq;
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
            client = new FtpClient(hostNameTextBlock.Text, int.Parse(hostPortTextBlock.Text));
        }

        private FtpClient client;

        private Stack<string> pathHistory = new Stack<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateListBox(client.List(@"C:\"));
            pathHistory.Push(@"C:\");
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path = (sender as ListBoxItem).Content.ToString();
            pathHistory.Push(path);
            UpdateListBox(client.List(path));
        }

        private void ListBoxItem_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            if (pathHistory.Count() == 1)
            {
                return;
            }

            pathHistory.Pop();
            UpdateListBox(client.List(pathHistory.Peek()));
        }

        private void UpdateListBox(List<FileInfo> filesInfo)
        {
            listBox.Items.Clear();

            var item1 = new ListBoxItem
            {
                Content = "..."
            };

            item1.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick1);
            listBox.Items.Add(item1);

            foreach (var res in filesInfo)
            {
                var item = new ListBoxItem
                {
                    Content = res.Name
                };
                listBox.Items.Add(item);

                item.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            client = new FtpClient(hostNameTextBlock.Text, int.Parse(hostPortTextBlock.Text));
            listBox.Items.Clear();
        }
    }
}
