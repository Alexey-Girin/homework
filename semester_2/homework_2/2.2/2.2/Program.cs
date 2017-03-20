using System;

namespace SecondHomework.Task2
{
    class List
    {
        class ListElement
        {
            public int Value;
            public ListElement Next;

            public ListElement (int value, ListElement next)
            {
                this.Value = value;
                this.Next = next;
            }
        }

        private ListElement head;
        public int ListLenght;

        public List()
        {
            this.head = null;
        }

        public void Add(int value)
        {
            ListElement newElement = new ListElement(value, head);
            head = newElement;
            ListLenght++;
        }

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

            Console.WriteLine("value not found");
        }

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

        public void Print()
        {
            Console.WriteLine("list:");

            ListElement position = head;
            while (position != null)
            {
                Console.Write(position.Value + " ");
                position = position.Next;
            }
        }

        public void DeleteList()
        {
            head = null;
        }
    }
}