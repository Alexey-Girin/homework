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
        public void OnRight(object sender, EventArgs args)
        {
            if (Console.CursorLeft < Console.BufferWidth - 1)
            {
                ++Console.CursorLeft;
            }
        }

        /// <summary>
        /// Движение курсора вверх.
        /// </summary>
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
        public void OnDown(object sender, EventArgs args)
        {
            if (Console.CursorTop < Console.BufferHeight - 1)
            {
                ++Console.CursorTop;
            }
        }
    }
}
