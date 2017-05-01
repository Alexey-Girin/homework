namespace StackCalculator
{
    using System;

    /// <summary>
    /// Стековый калькулятор.
    /// </summary>
    public class StackCalc
    {
        private IStack stack;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="StackCalc"/>.
        /// </summary>
        /// <param name="stack">Стек.</param>
        public StackCalc(IStack stack)
        {
            this.stack = stack;
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
                    stack.Push(number);
                    number = 0;
                    continue;
                }

                if (expression[i] == ' ')
                {
                    continue;
                }

                PerformOperation(expression[i]);
            }

            if (stack.Size() != 1)
            {
                throw new InputErrorExeption("ошибка ввода");
            }
           
            return stack.Pop();
        }

        /// <summary>
        /// Бинарная операция.
        /// </summary>
        /// <param name="operator">Возможный оператор.</param>
        private void PerformOperation(char @operator)
        {
            if (stack.Size() < 2)
            {
                throw new InputErrorExeption("ошибка ввода");
            }

            switch (@operator)
            {
                case '+':
                    var secondOperand = stack.Pop();
                    var firstOperand = stack.Pop();
                    stack.Push(firstOperand + secondOperand);
                    return;
                case '-':
                    secondOperand = stack.Pop();
                    firstOperand = stack.Pop();
                    stack.Push(firstOperand - secondOperand);
                    return;
                case '*':
                    secondOperand = stack.Pop();
                    firstOperand = stack.Pop();
                    stack.Push(firstOperand * secondOperand);
                    return;
                case '/':
                    secondOperand = stack.Pop();
                    firstOperand = stack.Pop();
                    stack.Push(firstOperand / secondOperand);
                    return;
                default:
                    throw new InputErrorExeption("ошибка ввода");
            }
        }        
    }
}
