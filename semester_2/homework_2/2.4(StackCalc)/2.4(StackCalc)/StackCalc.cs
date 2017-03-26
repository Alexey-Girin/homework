namespace StackCalculator
{
    using System;

    /// <summary>
    /// Стековый калькулятор.
    /// </summary>
    public class StackCalc
    {
        private IStack Stack;
        
        public StackCalc(IStack stack)
        {
            this.Stack = stack;
        }

        /// <summary>
        /// Вычисление значения выражения (пример ввода "12 3 +").
        /// </summary>
        /// <param name="expression">Вычисляемое выражение.</param>
        /// <returns>Вычисленное значение.</returns>
        public double Calculation(string expression)
        {
            int number = 0;
            int expressionSize = expression.Length;
            for (int i = 0; i < expressionSize; i++)
            {
                if (expression[i] >= '0' && expression[i] <= '9')
                {
                    number = number * 10 + Convert.ToInt32(expression[i] - '0');
                    continue;
                }

                if (number != 0)
                {
                    Stack.Push(number);
                    number = 0;
                    continue;
                }

                if (expression[i] == ' ')
                {
                    continue;
                }

                PerformOperation(expression[i]);
            }

            if (Stack.Size() != 1)
            {
                throw new Exception("ошибка ввода");
            }
           
            return Stack.Pop();
        }

        /// <summary>
        /// Бинарная операция.
        /// </summary>
        /// <param name="operator">Возможный оператор.</param>
        private void PerformOperation(char @operator)
        {
            double firstOperand;
            double secondOperand;

            if (Stack.Size() < 2)
            {
                throw new Exception("ошибка ввода");
            }

            switch (@operator)
            {
                case '+':
                    secondOperand = Stack.Pop();
                    firstOperand = Stack.Pop();
                    Stack.Push(firstOperand + secondOperand);
                    return;
                case '-':
                    secondOperand = Stack.Pop();
                    firstOperand = Stack.Pop();
                    Stack.Push(firstOperand - secondOperand);
                    return;
                case '*':
                    secondOperand = Stack.Pop();
                    firstOperand = Stack.Pop();
                    Stack.Push(firstOperand * secondOperand);
                    return;
                case '/':
                    secondOperand = Stack.Pop();
                    firstOperand = Stack.Pop();
                    Stack.Push(firstOperand / secondOperand);
                    return;
                default:
                    throw new Exception("ошибка ввода");
            }
        }        
    }
}
