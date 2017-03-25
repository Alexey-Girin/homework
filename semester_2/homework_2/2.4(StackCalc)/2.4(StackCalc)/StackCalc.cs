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
        /// Вычисление значения выражения.
        /// </summary>
        /// <param name="expression">Вычисляемое выражение.</param>
        /// <returns>Вычисленное значение.</returns>
        public double Calculation(string expression)
        {
            int expressionSize = expression.Length;
            for (int i = 0; i < expressionSize; i++)
            {
                if (expression[i] >= '0' && expression[i] <= '9')
                {
                    Stack.Push(Convert.ToInt32(expression[i] - '0'));
                    continue;
                }

                PerformOperation(expression[i]);
            }

            return Stack.Pop();
        }

        /// <summary>
        /// Вычисление бинарной операции.
        /// </summary>
        /// <param name="operator">Возможный оператор.</param>
        private void PerformOperation(char @operator)
        {
            double firstOperand;
            double secondOperand;

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
            }
        }        
    }
}
