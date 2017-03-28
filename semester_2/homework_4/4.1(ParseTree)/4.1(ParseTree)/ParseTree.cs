namespace ParseTreeNamespace
{
    using System;

    class ParseTree
    {
        class Node
        {
            public NodeValue nodeValue;
            public Node leftSon;
            public Node rightSon;

            public Node(char value, Node newLeftSon, Node newRightChild)
            {
                this.leftSon = newLeftSon;
                this.rightSon = newRightChild;

                if (value > '0' && value < '9')
                {
                    this.nodeValue = new Operand(value - '0');
                    return;
                }

                this.nodeValue = new Operator(value);
            }

            public void Print()
            {

            }

            
        }

        private Node root;

        public ParseTree()
        {
            this.root = null;
        }

        public int Calculate(string expression)
        {
            BuildTree(expression, root);
            root.nodeValue.PrintValue();
            return 1;
            
        }

        private void BuildTree(string expression, Node localRoot)
        {
            localRoot = new Node(expression[2], null, null);

            int nextPosition;
            int closeBracket;

            if (expression[4] == '(')
            {
                closeBracket = FindCloseBracket(expression, 1);
                BuildTree(expression.Substring(4, closeBracket - 4 + 1), localRoot.leftSon);
                nextPosition = closeBracket + 2;
            }
            else
            {
                localRoot.leftSon = new Node(expression[4], null, null);
                localRoot.rightSon = new Node(expression[6], null, null);
                return;
            }

            closeBracket = FindCloseBracket(expression, 2);
            BuildTree(expression.Substring(nextPosition, closeBracket - nextPosition + 1), localRoot.rightSon);

        }

        private int FindCloseBracket(string expression, int numOfExpression)
        {
            int balance = 1;
            int expressionSize = expression.Length;
            for (int i = 12; i < expressionSize; i++)
            {
                if (expression[i] == '(')
                {
                    balance++;
                    continue;
                }

                if (expression[i] == ')')
                {
                    balance--;

                    if(balance == 0)
                    {
                        if (numOfExpression == 1)
                        {
                            return i;
                        }
                        else
                        {
                            balance = 1;
                            i += 3;
                        }
                    }
                }
            }
   
            throw new Exception();
        }
    }
}
