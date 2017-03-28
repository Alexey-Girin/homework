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

            public double Calculate()
            {
                if (nodeValue is Operand)
                {
                    return 1.0 * nodeValue.GetValue();
                }

                double firstNum = leftSon.Calculate();
                double secondNum = rightSon.Calculate();
                return BinaryOperation((char)nodeValue.GetValue(), firstNum, secondNum);
            }

            private double BinaryOperation(char sign, double firstNum, double secondNum)
            {
                switch (sign)
                {
                    case ('+'):
                        return firstNum + secondNum;
                    case ('-'):
                        return firstNum - secondNum;
                    case ('*'):
                        return firstNum * secondNum;
                    case ('/'):
                        return firstNum / secondNum;
                    default:
                        throw new Exception();
                }
            }
        }

        private Node root;

        public ParseTree() => this.root = null;

        public double Calculate(string expression)
        {
            root = BuildTree(expression);

            Console.Write((char)root.nodeValue.GetValue());

            return root.Calculate();
        }

        private Node BuildTree(string expression)
        {
            Node localRoot = new Node(expression[1], null, null);

            int closeBracket;
            int openBracket = 3;

            if (expression[openBracket] == '(')
            {
                closeBracket = FindCloseBracket(expression, openBracket + 1);
                localRoot.leftSon = BuildTree(expression.Substring(openBracket, closeBracket - openBracket + 1));
                openBracket = closeBracket + 2;
            }
            else
            {
                int firstOperand = openBracket;
                localRoot.leftSon = new Node(expression[firstOperand], null, null);

                int secondOpenBracket = 5;
                openBracket = secondOpenBracket;
            }

            if (expression[openBracket] == '(')
            {
                closeBracket = FindCloseBracket(expression, openBracket + 1);
                localRoot.rightSon = BuildTree(expression.Substring(openBracket, closeBracket - openBracket + 1));
            }
            else
            {
                int secondOperand = openBracket;
                localRoot.rightSon = new Node(expression[secondOperand], null, null);
            }

            return localRoot;
        }

        private int FindCloseBracket(string expression, int startPosition)
        {
            int balance = 1;
            int expressionSize = expression.Length;

            for (int i = startPosition; i < expressionSize; i++)
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
                        return i;
                    }
                }
            }
   
            throw new Exception();
        }
    }
}
