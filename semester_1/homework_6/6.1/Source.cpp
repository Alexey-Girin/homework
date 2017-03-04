#include <iostream>
#include <string>
#include "stack.h"

using namespace std;

string input()
{
	string inputString = "";
	cout << "введите арифметического выражения в постфиксной форме:" << endl;
	getline(cin, inputString);
	return inputString;
}

FlagType solutionOfPostfixExpression(Stack *&stack, bool &error, string inputString)
{
	int inputStringSize = inputString.size();
	if (inputStringSize < 3)
	{
		error = true;
		return -1;
	}

	for (int i = 0; i < inputStringSize; i++)
	{
		char symbol = inputString[i];
		if (symbol >= '0' && symbol <= '9')
		{
			push(stack, symbol - '0');
			continue;
		}
		bool isEmptyStack = false;
		FlagType firstNum = pop(stack, isEmptyStack);
		FlagType secondNum = pop(stack, isEmptyStack);
		if (isEmptyStack)
		{
			error = true;
			return -1;
		}
		switch (symbol)
		{
		case '+':
		{
			push(stack, firstNum + secondNum);
			break;
		}
		case '-':
		{
			push(stack, secondNum - firstNum);
			break;
		}
		case '*':
		{
			push(stack, firstNum * secondNum);
			break;
		}
		case '/':
		{
			if (firstNum == 0)
			{
				error = true;
				return -1;
			}
			push(stack, secondNum / firstNum);
			break;
		}
		default:
		{
			error = true;
			return -1;
		}

		}
	}
	bool isEmptyStack = false;
	FlagType result = pop(stack, isEmptyStack);
	if (!isEmpty(stack))
	{
		error = true;
		return -1;
	}
	return result;
}

bool test1()
{
	string testString = "261111+1111+1+++*---/";
	Stack *stack = createStack();
	bool errorTest = false;
	double resultTest = solutionOfPostfixExpression(stack, errorTest, testString);
	const double resultTrue = -0.5;
	deleteStack(stack);
	return (resultTest == resultTrue && !errorTest);
}

bool test2()
{
	string testString = "1111++0/";
	Stack *stack = createStack();
	bool errorTest = false;
	double resultTest = solutionOfPostfixExpression(stack, errorTest, testString);
	deleteStack(stack);
	return errorTest;
}

bool test3()
{
	string testString = "11+22+33+44+-*/";
	Stack *stack = createStack();
	bool errorTest = false;
	double resultTest = solutionOfPostfixExpression(stack, errorTest, testString);
	const double resultTrue = -0.25;
	deleteStack(stack);
	return (resultTest == resultTrue && !errorTest);
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
	testing();
	setlocale(LC_ALL, "Russian");
	Stack *stack = createStack();
	bool error = false;
	FlagType result = solutionOfPostfixExpression(stack, error, input());
	if (error)
	{
		cout << "ошибка ввода" << endl;
		return 0;
	}
	cout << "результат: " << result << endl;
	deleteStack(stack);
	return 0;
}