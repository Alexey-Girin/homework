namespace ParseTreeNamespace
{
    using System;

    /// <summary>
    /// Дерево разбора арифметического выражения.
    /// </summary>
    public class ParseTree
    {
        /// <summary>
        /// Узел дерева.
        /// </summary>
        private abstract class Node
        {
            public Node LeftSon { get; set; }
            public Node RightSon { get; set; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="Node"/>.
            /// </summary>
            public Node()
            {
            }

            /// <summary>
            /// Вычисление значения в данном узле дерева.
            /// </summary>
            /// <returns>Результат вычисления.</returns>
            public abstract double CalculateNode();

            /// <summary>
            /// Печать узла и его предков.
            /// </summary>
            public abstract void Print();
        }

        /// <summary>
        /// Узел - операнд.
        /// </summary>
        private class Operand : Node
        {
            private double operand;

            /// <summary>
            /// Конструктор экземпляра класса <see cref="Operand"/>.
            /// </summary>
            /// <param name="value">Значение операнда.</param>
            public Operand(int value) => operand = 1.0 * value;

            /// <summary>
            /// Возврат значения операнда.
            /// </summary>
            /// <returns>Значение операнда.</returns>
            public override double CalculateNode() => operand;

            /// <summary>
            /// Печать значения операнда.
            /// </summary>
            public override void Print() => Console.Write(operand + " ");
        }

        /// <summary>
        /// Узел дерева - оператор.
        /// </summary>
        private class Operator : Node
        {
            private char @operator;

            /// <summary>
            /// Конструктор экземпляра класса <see cref="Operator"/>.
            /// </summary>
            /// <param name="value">Знак операции.</param>
            public Operator(char value) => @operator = value;

            /// <summary>
            /// Вычисление значения узла.
            /// </summary>
            /// <returns>Результат операции.</returns>
            public override double CalculateNode()
            {
                double firstNum = LeftSon.CalculateNode();
                double secondNum = RightSon.CalculateNode();
                return BinaryOperation(@operator, firstNum, secondNum);
            }

            /// <summary>
            /// Выполнение операции над сыновьями узла.
            /// </summary>
            /// <param name="sign">Арифметический знак операции.</param>
            /// <param name="firstNum">Значение операнда из левого сына узла.</param>
            /// <param name="secondNum">Значение операнда из правого сына узла.</param>
            /// <returns>Результат операции.</returns>
            private double BinaryOperation(char sign, double firstNum, double secondNum)
            {
                switch (sign)
                {
                    case '+':
                        return firstNum + secondNum;
                    case '-':
                        return firstNum - secondNum;
                    case '*':
                        return firstNum * secondNum;
                    case '/':
                        if (secondNum == 0)
                        {
                            throw new DivideByZeroException("деление на ноль");
                        }
                        return firstNum / secondNum;
                    default:
                        throw new InputException("некорректное выражение");
                }
            }

            /// <summary>
            /// Печать знака операции и предков узла.
            /// </summary>
            public override void Print()
            {
                Console.Write("( " + @operator + " ");

                LeftSon.Print();
                RightSon.Print();

                Console.Write(") ");
            }
        }

        private Operator root;

        /// <summary>
        /// Конструктор экзмепляра класса <see cref="ParseTree"/>.
        /// </summary>
        public ParseTree()
        {
        }

        /// <summary>
        /// Вычисление значения выражения через дерево разбора (построение, печать, вычисление).
        /// </summary>
        /// <param name="expression">Вычисляемое выражение.</param>
        /// <returns>Значение выражения.</returns>
        public double Calculate(string expression)
        {
            root = BuildTree(expression);

            root.Print();
            Console.WriteLine();

            return root.CalculateNode();
        }

        /// <summary>
        /// Построение дерева разбора.
        /// </summary>
        /// <param name="expression">Выражение, по которому строится дерево разбора.</param>
        /// <returns>Корень построенного дерева разбора.</returns>
        private Operator BuildTree(string expression)
        {
            IsOperator(expression[1]);
            Operator localRoot = new Operator(expression[1]);

            int closeBracket;
            int openBracket = 3;

            if (expression[openBracket] == '(')
            {
                closeBracket = FindCloseBracket(expression, openBracket);
                localRoot.LeftSon = BuildTree(
                    expression.Substring(openBracket, closeBracket - openBracket + 1));
                openBracket = closeBracket + 2;
            }
            else
            {
                int expressionSize = expression.Length;
                string operand = expression.Substring(
                    openBracket, expressionSize - openBracket);

                int endPosition = operand.IndexOf(" ");
                operand = operand.Substring(0, endPosition);

                openBracket += endPosition + 1;

                IsOperand(operand);
                localRoot.LeftSon = new Operand(Convert.ToInt32(operand, 10));
            }

            if (expression[openBracket] == '(')
            {
                closeBracket = FindCloseBracket(expression, openBracket);
                localRoot.RightSon = BuildTree(
                    expression.Substring(openBracket, closeBracket - openBracket + 1));
            }
            else
            {
                int expressionSize = expression.Length;
                string operand = expression.Substring(openBracket, expressionSize - openBracket - 1);

                IsOperand(operand);
                localRoot.RightSon = new Operand(Convert.ToInt32(operand, 10));
            }

            return localRoot;
        }

        /// <summary>
        /// Вспомогательный метод для построения дерева - нахождение балансовой закрывающейся скобки для данной открывающейся.
        /// </summary>
        /// <param name="expression">Выражение, в котором ведется поиск.</param>
        /// <param name="startPosition">Начальная позиця поиска.</param>
        /// <returns>Позиция балансовой закрывающейся скобки в выражении.</returns>
        private int FindCloseBracket(string expression, int startPosition)
        {
            int balance = 1;
            int expressionSize = expression.Length;

            for (int i = startPosition + 1; i < expressionSize; i++)
            {
                if (expression[i] == '(')
                {
                    balance++;
                    continue;
                }

                if (expression[i] == ')')
                {
                    balance--;

                    if (balance == 0)
                    {
                        return i;
                    }
                }
            }

            throw new InputException("некорректное выражение");
        }

        /// <summary>
        /// Проверка символа на знак операции.
        /// </summary>
        /// <param name="symbol">Проверяемый символ.</param>
        /// <returns></returns>
        private void IsOperator(char symbol)
        {
            if (symbol == '+' || symbol == '-' || symbol == '*' || symbol == '/')
            {
                return;
            }

            throw new InputException("некорректное выражение");
        }

        /// <summary>
        /// Проверка выражения на число.
        /// </summary>
        /// <param name="expression">Проверяемое выражение.</param>
        private void IsOperand(string expression)
        {
            foreach(char symbol in expression)
            {
                if (symbol < '0' || symbol > '9')
                {
                    throw new InputException("некорректное выражение");
                }
            }
        }
    }
}
