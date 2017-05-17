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
            Numeral,
            OperationSign,
            Negation,
            Delimiter,
            Equality,
            Message
        };

        /// <summary>
        /// Текущее событие.
        /// </summary>
        private State currentState = State.Numeral;

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
            display.Text = currentNumber;
        }

        /// <summary>
        /// Обработка события нажатия на цифровую кнопку.
        /// </summary>
        private void OnButtonNumClick(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (currentNumber.Length > 8)
            {
                return;
            }

            if (currentState == State.Numeral
                || currentState == State.Negation
                || currentState == State.Message)
            {
                if (currentNumber == "0")
                {
                    currentNumber = button.Text;
                    display.Text = button.Text;
                    return;
                }

                currentNumber += button.Text;
                display.Text += button.Text;
                currentState = State.Numeral;
                return;
            }  
            
            if (currentState == State.OperationSign
                || currentState == State.Delimiter)
            {
                currentNumber += button.Text;
                display.Text += button.Text;
                currentState = State.Numeral;
                return;
            }

            if (currentState == State.Equality)
            {
                currentNumber = button.Text;
                display.Text = button.Text;
                currentState = State.Numeral;
            }
        }

        /// <summary>
        /// Обработка события смены знака числа.
        /// </summary>
        private void OnButtonNegationClick(object sender, EventArgs e)
        {
            if (currentState == State.OperationSign)
            {
                return;
            }

            if (currentState == State.Message)
            {
                Remove();
                return;
            }

            var startPosition = 0;

            if (currentOperator != ' ')
            {
                var operatorIndex = display.Text.Substring(1).IndexOf(currentOperator) + 1;

                if (currentOperator == '+'
                    || currentOperator == '-')
                {
                    var sign = (currentOperator == '+') ? '-' : '+';
                    display.Text = display.Text.Substring(0, operatorIndex) + sign +
                        display.Text.Substring(operatorIndex + 1);
                    currentState = State.Negation;
                    currentOperator = sign;
                    return;
                }

                startPosition = operatorIndex + 2;
            }

            if (currentNumber[0] == '-')
            { 
                currentNumber = currentNumber.Substring(1);
                display.Text = display.Text.Remove(startPosition, 1);
            }
            else if (currentNumber != "0")
            {
                currentNumber = "-" + currentNumber;
                display.Text = display.Text.Insert(startPosition, "-");
            }

            currentState = State.Negation;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку разделителя.
        /// </summary>
        private void OnButtonDelimiterClick(object sender, EventArgs e)
        {
            if (currentNumber.Contains(",")
                || currentState == State.OperationSign)
            {
                return;
            }
            
            if (currentState == State.Message)
            {
                Remove();
            }

            currentNumber += ",";
            display.Text += ",";

            currentState = State.Delimiter;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку полной отмены операции.
        /// </summary>
        private void OnButtonCClick(object sender, EventArgs e) => Remove();

        /// <summary>
        /// Обработка события нажатия на кнопку операции.
        /// </summary>
        private void OnButtonOperationClick(object sender, EventArgs e)
        {
            if (currentState == State.Message)
            {
                Remove();
                return;
            }

            var button = sender as Button;
            var @operator = button.Text[0];

            if (display.Text.Contains(currentOperator)
                && display.Text[display.Text.Length - 2] != currentOperator)
            {
                var secondOperand = Convert.ToDouble(currentNumber);
                result = Calculation(secondOperand);
                
                if (currentState == State.Message)
                {
                    return;
                }

                currentNumber = result.ToString();
                display.Text = currentNumber;
                currentState = State.Equality;
            }

            currentOperator = @operator;

            switch (currentState)
            {
                case State.OperationSign:
                    display.Text = display.Text.Remove(display.Text.Length - 2) + currentOperator + " ";
                    break;
                case State.Equality:
                    currentNumber = "0";
                    display.Text += " " + currentOperator + " ";
                    break;
                default:
                    result = Convert.ToDouble(currentNumber);
                    currentNumber = "0";
                    display.Text = result.ToString() + " " + currentOperator + " ";
                    break;
            }

            currentState = State.OperationSign;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку равенства.
        /// </summary>
        private void OnButtonEqualityClick(object sender, EventArgs e)
        {
            if (currentState == State.Message)
            {
                Remove();
                return;
            }

            if (currentState == State.OperationSign 
                || currentState == State.Equality)
            {
                return;
            }

            if (currentOperator == ' ')
            {
                display.Text = Convert.ToDouble(currentNumber).ToString();
                currentNumber = display.Text;
                currentState = State.Numeral;
                return;
            }

            var secondOperand = Convert.ToDouble(currentNumber);
            result = Calculation(secondOperand);

            if (currentState == State.Message)
            {
                return;
            }

            currentNumber = result.ToString();
            display.Text = currentNumber;
            currentOperator = ' ';

            currentState = State.Equality;
        }

        /// <summary>
        /// Удаление введенного числа.
        /// </summary>
        private void OnButtonCEClick(object sender, EventArgs e)
        {
            if (currentState == State.OperationSign)
            {
                return;
            }

            currentNumber = "0";

            if (currentOperator != ' ')
            {
                var operatorIndex = display.Text.Substring(1).IndexOf(currentOperator) + 1;
                display.Text = display.Text.Remove(operatorIndex + 2);
                currentState = State.OperationSign;
                return;
            }

            display.Text = currentNumber;
            result = 0;
            currentState = State.Numeral;
        }

        /// <summary>
        /// Переход калькулятора в стартовое положение.
        /// </summary>
        private void Remove()
        {
            currentNumber = "0";
            currentOperator = ' ';
            display.Text = currentNumber;
            result = 0;
            currentState = State.Numeral;
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
                        Remove();
                        currentState = State.Message;
                        display.Text = "деление на ноль";
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
