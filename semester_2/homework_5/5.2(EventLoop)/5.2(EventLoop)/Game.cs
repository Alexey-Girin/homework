namespace EventLoopNamespace
{
    using System;

    /// <summary>
    /// Класс логики игры.
    /// </summary>
    class Game
    {
        /// <summary>
        /// Движение курсора влево.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnLeft(object sender, EventArgs args)
        {
            if (Console.CursorLeft != 0)
            {
                --Console.CursorLeft;
            }
        }

        /// <summary>
        /// Движение курсора вправо.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnRight(object sender, EventArgs args)
        {
            ++Console.CursorLeft;
        }

        /// <summary>
        /// Движение курсора вверх.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnUp(object sender, EventArgs args)
        {
            if (Console.CursorTop != 0)
            {
                --Console.CursorTop;
            }
        }

        /// <summary>
        /// Движение курсора вниз.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnDown(object sender, EventArgs args)
        {
            ++Console.CursorTop;
        }
    }
}
