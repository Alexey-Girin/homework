namespace app
{
    using System;

    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
            => InitializeComponent();

        private int indicator;

        private bool isActiveIndicator = false;

        private void OnButtonClick(object sender, EventArgs e)
        {
            if (isActiveIndicator)
            {
                return;
            }

            isActiveIndicator = true;
            timer.Enabled = true;
            timer.Tick += new EventHandler(OnTimerTlick);
        }

        private void OnTimerTlick(object sender, EventArgs e)
        { 
            label.Text = indicator.ToString() + "%";
            indicator += 1;

            if (indicator == 101)
            {
                timer.Enabled = false;
                buttonExit.Visible = true;
            }
        }

        private void OnExitClick(object sender, EventArgs e)
            => Dispose();
    }
}
