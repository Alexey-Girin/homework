#include <iostream>
#include <string>
#include "stack.h"

using namespace std;

bool isOperand(char &sign)
{
	return sign >= '0' && sign <= '9';
}

bool isOperator(char &sign)
{
	return sign == '+' || sign == '-' || sign == '*' || sign == '/';
}

bool isCorrectInfix(string infix)
{
	infix = "+" + infix + "+";
	int bracketBalance = 0;
	int infixSize = infix.size();
	for (int i = 1; i < infixSize - 1; i++)
	{
		if (isOperator(infix[i]) && 
			!isOperator(infix[i - 1]) && 
			!isOperator(infix[i +	1]))
		{
			continue;
		}
		if (infix[i] == '(' && 
			(isOperand(infix[i + 1]) || infix[i + 1] == '(') && 
			(isOperator(infix[i - 1]) || infix[i - 1] == '('))
		{
			bracketBalance++;
			continue;
		}
		if (infix[i] == ')' &&
			(isOperand(infix[i - 1]) || infix[i - 1] == ')') &&
			(isOperator(infix[i + 1]) || infix[i + 1] == ')'))
		{
			if (bracketBalance == 0)
			{
				return false;
			}
			bracketBalance--;
			continue;
		}
		if (!isOperand(infix[i]))
		{
			return false;
		}
	}
	return bracketBalance == 0;
}

bool isPrecedenceOverTopStack(char &firstSign, char &secondSign)
{
	if (secondSign == '(')
	{
		return true;
	}
	if (firstSign == '+' || firstSign == '-')
	{
		return false;
	}
	return (firstSign != '*' && firstSign != '/' || secondSign != '*' && secondSign != '/');
}

void pushOperator(Stack *&stack, string &postfix, char &sign)
{
	if (isEmpty(stack))
	{
		push(stack, sign);
		return;
	}
	char topStackSign = peek(stack);
	while (!isPrecedenceOverTopStack(sign, topStackSign))
	{
		postfix += topStackSign;
		pop(stack);
		if (isEmpty(stack))
		{
			break;
		}
		topStackSign = peek(stack);
	}
	push(stack, sign);
}

void popBeforeBracket(Stack *&stack, string &postfix)
{
	char topStackSign = peek(stack);
	while (topStackSign != '(')
	{
		postfix += topStackSign;
		pop(stack);
		topStackSign = peek(stack);
	}
	pop(stack);
}

string shuntingYard(Stack *&stack, string infix)
{
	string postfix = "";
	int infixSize = infix.size();
	for (int i = 0; i < infixSize; i++)
	{
		char sign = infix[i];
		if (isOperand(sign))
		{
			postfix += sign;
			continue;
		}
		if (isOperator(sign))
		{
			pushOperator(stack, postfix, sign);
			continue;
		} 
		if (sign == '(')
		{ 
			push(stack, sign);
			continue;
		}
		if (sign == ')')
		{
			popBeforeBracket(stack, postfix);
			continue;
		}
	}
	string remainder = stackRemainder(stack);
	return postfix + remainder;
}

bool test(string &infix, string &resultTrue)
{
	if (!isCorrectInfix(infix) && resultTrue == "ошибка ввода")
	{
		return true;
	}
	Stack *stack = createStack();
	string resultTest = shuntingYard(stack, infix);
	deleteStack(stack);
	return resultTest == resultTrue;
}

bool test1()
{
	string infix = "(1+2)*(3+4)";
	string resultTrue = "12+34+*";
	return test(infix, resultTrue);
}

bool test2()
{
	string infix = "1+2*(3+4/(0-5)/(2+3*(7-9))-11)";
	string resultTrue = "123405-/2379-*+/+11-*+";
	return test(infix, resultTrue);
}

bool test3()
{
	string infix = "(2+2)*(3+3))";
	string resultTrue = "ошибка ввода";
	return test(infix, resultTrue);
}

void testing()
{
	if (test1() && test2() && test3())
	{
		cout << "testing true" << endl;
		return;
	}
	cout << "testing false" << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	testing();

	Stack *stack = createStack();
	string infix = "";
	cout << "инфиксная форма:" << endl;
	getline(cin, infix);

	if (!isCorrectInfix(infix))
	{
		cout << "ошибка ввода" << endl;
		return 0;
	}

	string postfix = shuntingYard(stack, infix);
	cout << "постфиксная форма:" << endl << postfix << endl;
	deleteStack(stack);
	return 0;
}