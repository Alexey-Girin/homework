using System;

namespace SecondHomework.Task1
{
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
        public int StackLenght;

        public Stack()
        {
            this.head = null;
            this.StackLenght = 0;
        }

        public void Push(int value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
            StackLenght++;
        }

        public int Pop()
        {
            if (isEmpty())
            {
                return -1;
            }

            int popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        public int ReadHeadElement()
        {
            if (isEmpty())
            {
                return -1;
            }

            return head.Value;
        }

        public bool isEmpty()
        {
            return head == null;
        }

        public void PrintStack()
        {
            Console.Write("stack:\n");

            StackElement positionElement = head;
            while (positionElement != null)
            {
                Console.Write(positionElement.Value + " ");
                positionElement = positionElement.Next;
            }

            Console.Write("\n");
        }

        public void DeleteStack()
        {
            while (!isEmpty())
            {
                Pop();
            }
        }
    }
}