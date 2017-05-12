namespace CalcNamespace
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Приложение калькулятор.
    /// </summary>
    public partial class Calculator : Form
    {
        /// <summary>
        /// Возможные события.
        /// </summary>
        private enum State
        {
            numeral,
            operationSign,
            negation,
            delimiter,
            deleting,
            deletingAfterDelimiter,
            equality
        };

        /// <summary>
        /// Текущее событие.
        /// </summary>
        private State currentState = State.numeral;

        /// <summary>
        /// Текущий результат вычисления.
        /// </summary>
        private double result;

        /// <summary>
        /// Текущий оператор.
        /// </summary>
        private char currentOperator = ' ';

        /// <summary>
        /// Текущее значение вводимого числа.
        /// </summary>
        private string currentNumber = "0";

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Calculator"/>.
        /// </summary>
        public Calculator()
        {
            InitializeComponent();
            textBox.Text = currentNumber;
        }

        /// <summary>
        /// Обработка события нажатия на цифровую кнопку.
        /// </summary>
        private void OnButtonNumClick(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (currentState == State.numeral ||
                currentState == State.negation)
            {
                if (currentNumber == "0")
                {
                    currentNumber = button.Text;
                    textBox.Text = button.Text;
                    currentState = State.numeral;
                    return;
                }

                currentNumber += button.Text;
                textBox.Text += button.Text;
                currentState = State.numeral;
                return;
            }  
            
            if (currentState == State.operationSign ||
                currentState == State.delimiter)
            {
                currentNumber += button.Text;
                textBox.Text += button.Text;
                currentState = State.numeral;
                return;
            }

            if (currentState == State.equality)
            {
                currentNumber = button.Text;
                textBox.Text = button.Text;
                currentState = State.numeral;
            }
        }

        /// <summary>
        /// Обработка события смены знака числа.
        /// </summary>
        private void OnButtonNegationClick(object sender, EventArgs e)
        {
            if (currentState == State.operationSign)
            {
                return;
            }

            var startPosition = 0;

            if (currentOperator != ' ')
            {
                var index = textBox.Text.IndexOf(currentOperator);

                if (currentOperator == '+')
                {
                    textBox.Text = textBox.Text.Substring(0, index) + "-" +
                        textBox.Text.Substring(index + 1);
                    currentState = State.negation;
                    currentOperator = '-';
                    return;
                }

                if (currentOperator == '-')
                {
                    textBox.Text = textBox.Text.Substring(0, index) + "+" +
                        textBox.Text.Substring(index + 1);
                    currentState = State.negation;
                    currentOperator = '+';
                    return;
                }

                startPosition = index + 2;
            }

            if (currentNumber[0] == '-')
            {
                
                currentNumber = currentNumber.Substring(1);
                textBox.Text = textBox.Text.Remove(startPosition, 1);
            }
            else
            {
                if (currentNumber != "0")
                {
                    currentNumber = "-" + currentNumber;
                    textBox.Text = textBox.Text.Insert(startPosition, "-");
                }
            }

            currentState = State.negation;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку разделителя.
        /// </summary>
        private void OnButtonDelimiterClick(object sender, EventArgs e)
        {
            if (currentNumber.Contains(",") || 
                currentState == State.operationSign)
            {
                return;
            }

            currentNumber += ",";
            textBox.Text += ",";

            currentState = State.delimiter;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку полной отмены операции.
        /// </summary>
        private void OnButtonCClick(object sender, EventArgs e)
        {
            currentNumber = "0";
            textBox.Text = currentNumber;
            result = 0;
            currentState = State.numeral;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку операции.
        /// </summary>
        private void OnButtonOperationClick(object sender, EventArgs e)
        {
            var button = sender as Button;
            var @operator = button.Text[0];

            if (textBox.Text.Contains(currentOperator) &&
                textBox.Text[textBox.Text.Length - 2] != currentOperator)
            {
                var secondOperand = ConvertToDouble(currentNumber);
                result = Calculation(secondOperand);
                currentNumber = result.ToString();
                textBox.Text = currentNumber;
                currentState = State.equality;
            }

            currentOperator = @operator;

            switch (currentState)
            {
                case State.operationSign:
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 2) + currentOperator + " ";
                    break;
                case State.equality:
                    currentNumber = "0";
                    textBox.Text += " " + currentOperator + " ";
                    break;
                default:
                    result = ConvertToDouble(currentNumber);
                    currentNumber = "0";
                    textBox.Text = result.ToString() + " " + currentOperator + " ";
                    break;
            }

            currentState = State.operationSign;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку равенства.
        /// </summary>
        private void OnButtonEqualityClick(object sender, EventArgs e)
        {
            if(currentState == State.numeral ||
                currentState == State.negation ||
                currentState == State.delimiter)
            {
                if (currentOperator == ' ')
                {
                    textBox.Text = ConvertToDouble(currentNumber).ToString();
                    currentState = State.numeral;
                    return;
                }

                var secondOperand = ConvertToDouble(currentNumber);
                result = Calculation(secondOperand);
                currentNumber = result.ToString();
                textBox.Text = currentNumber;
                currentOperator = ' ';

                currentState = State.equality;
            }
        }

        /// <summary>
        /// Метод для конвертирования числа из string в double/
        /// </summary>
        /// <param name="numString">Строка для конвертирования.</param>
        /// <returns>Результат конвертирования.</returns>
        public double ConvertToDouble(string numString)
        {
            int delimiterPosition = numString.IndexOf(",");

            if (delimiterPosition == -1)
            {
                return Convert.ToInt32(numString);
            }

            string wholePart = numString.Substring(0, delimiterPosition);
            string fraction = numString.Substring(delimiterPosition + 1);

            if (fraction.Length == 0)
            {
                return Convert.ToInt32(wholePart);
            }

            int sign = 1;

            if (wholePart[0] == '-')
            {
                sign = -1;
            }

            return Convert.ToInt32(wholePart) +
                Convert.ToInt32(fraction) * Math.Pow(10, -fraction.Length) * sign;
        }

        /// <summary>
        /// Метод выполнения бинарной операции.
        /// </summary>
        /// <param name="secondOperand">Второй операнд.</param>
        /// <returns>Результат выполнения операции.</returns>
        private double Calculation(double secondOperand)
        {
            switch (currentOperator)
            {
                case '/':
                    if (secondOperand == 0)
                    {
                        currentNumber = "0";
                        textBox.Text = currentNumber;
                        result = 0;
                        currentState = State.numeral;
                        return 0;
                    }
                    return result / secondOperand;
                case '*':
                    return result * secondOperand;
                case '-':
                    return result - secondOperand;
                case '+':
                    return result + secondOperand;
                default:
                    throw new InputExeption();
            }
        }
    }
}
