using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool[,] field;

        public MainWindow()
        {
            InitializeComponent();

            Initialization();
        }

        private void MakeMove(object sender, RoutedEventArgs e)
        {
            if (isFinished)
            {
                return;
            }

            var button = (Button)sender;

            if (button.Content != null)
            {
                MessageBox.Show("Ячейка уже занята");
                return;
            }

            int x = button.Name[6] - '0';
            int y = button.Name[7] - '0';

            var players = (Players)Resources["CurrentPlayer"];
            button.Content = players.CurrentPlayer == 0 ? "O" : "X";
            field[x, y] = players.CurrentPlayer == 0;

            if (CheckWin())
            {
                MessageBox.Show($"Игрок {players.CurrentPlayer} победил. Начните новую игру");
                isFinished = true;
            }
        }

        private void Initialization()
        {
            field = new bool[2, 2];
            isFinished = false;
        }

        private bool CheckWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if (field[i, 0] == field[i, 1] == field[i, 2] == true)
                {
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (field[0, i] == field[1, i] == field[2, i] == true)
                {
                    return true;
                }
            }
            return field[0, 0] == field[1, 1] == field[2, 2] == true ||
                    field[0, 2] == field[1, 1] == field[2, 0] == true;
        }

        private bool isFinished = false;

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            isFinished = false;

            Button00.Content = null;
            Button01.Content = null;
            Button02.Content = null;
            Button10.Content = null;
            Button11.Content = null;
            Button12.Content = null;
            Button20.Content = null;
            Button21.Content = null;
            Button22.Content = null;

            Initialization();
        }
    }
}
