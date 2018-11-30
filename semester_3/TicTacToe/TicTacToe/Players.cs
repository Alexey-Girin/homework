using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToe
{
    public class Players : DependencyObject
    {
        public static readonly DependencyProperty CurrentPlayerProperty;

        static Players()
        {
            CurrentPlayerProperty = DependencyProperty.Register("CurrentPlayer", typeof(int), typeof(Players));
        }

        public int CurrentPlayer
        {
            get
            {
                return (int)GetValue(CurrentPlayerProperty);
            }
            set
            {
                SetValue(CurrentPlayerProperty, value);
            }
        }
    }
}
