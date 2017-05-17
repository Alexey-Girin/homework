namespace ClockNamespace
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Отображение текущего времени.
    /// </summary>
    public partial class ClockForm : Form
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="ClockForm"/>.
        /// </summary>
        public ClockForm() => InitializeComponent();

        /// <summary>
        /// Обработчик события для таймера.
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
            => labelTime.Text = DateTime.Now.ToShortTimeString();
    }
}
