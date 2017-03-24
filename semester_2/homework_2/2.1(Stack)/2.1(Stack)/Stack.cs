namespace StackNamespace
{
    using System;

    /// <summary>
    /// Stack based on references.
    /// </summary>
    public class Stack
    {
        private class StackElement
        {
            public int Value;
            public StackElement Next;

            public StackElement(int value, StackElement next)
            {
                this.Next = next;
                this.Value = value;
            }
        }

        private StackElement head;

        public Stack()
        {
            this.head = null;
        }

        /// <summary>
        /// Push value to stack
        /// </summary>
        /// <param name="value">Value to be pushed</param>
        public void Push(int value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Pop value from stack
        /// </summary>
        /// <returns>Popped value</returns>
        public int Pop()
        {
            if (IsEmpty())
            {
                return -1;
            }

            int popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Peek value from stack
        /// </summary>
        /// <returns>Peeked value</returns>
        public int Peek()
        {
            if (IsEmpty())
            {
                return -1;
            }

            return head.Value;
        }

        /// <summary>
        /// Check stack for emptiness
        /// </summary>
        /// <returns>True is empty</returns>
        public bool IsEmpty()
        {
            return head == null;
        }
    }
}
