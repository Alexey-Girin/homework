namespace ListNamespace
{
    using System;

    /// <summary>
    /// List based on references. 
    /// </summary>
    public class List
    {
        private class ListElement
        {
            public int Value;
            public ListElement Next;

            public ListElement(int value, ListElement next)
            {
                this.Value = value;
                this.Next = next;
            }
        }

        private ListElement head;

        public List()
        {
            this.head = null;
        }

        /// <summary>
        /// Add value to list.
        /// </summary>
        /// <param name="value">Value to be added</param>
        public void Add(int value)
        {
            ListElement newElement = new ListElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Check list for emptiness.
        /// </summary>
        /// <returns>If list is empty then true.</returns>
        public bool IsEmpty()
        {
            if(head == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete value from list.
        /// </summary>
        /// <param name="value">Value to be deleted.</param>
        public void Delete(int value)
        {
            if (head == null)
            {
                return;
            }

            if (head.Value == value)
            {
                head = head.Next;
                return;
            }

            ListElement position = head;
            while (position.Next != null)
            {
                if (position.Next.Value == value)
                {
                    position.Next = position.Next.Next;
                    return;
                }

                position = position.Next;
            }

            Console.WriteLine("Value not found");
        }

        /// <summary>
        /// Сheck for the presence of value in list.
        /// </summary>
        /// <param name="value">Value belongs to list.</param>
        /// <returns></returns>
        public bool Find(int value)
        {
            ListElement position = head;
            while (position != null)
            {
                if (position.Value == value)
                {
                    return true;
                }

                position = position.Next;
            }

            return false;
        }
    }
}
