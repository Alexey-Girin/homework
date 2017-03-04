#include <iostream>
#include <string>
#include "stack.h"

using namespace std;

bool isEquivalentSymbols(char &firstSymbol, char &secondSymbol)
{
	if (firstSymbol == '(' && secondSymbol == ')')
	{
		return true;
	}
	if (firstSymbol == '[' && secondSymbol == ']')
	{
		return true;
	}
	if (firstSymbol == '{' && secondSymbol == '}')
	{
		return true;
	}
	return false;
}

bool isBalance(string &inputString)
{
	Stack *stack = createStack();
	int sizeInputString = inputString.size();
	for (int i = 0; i < sizeInputString; i++)
	{
		char symbol = inputString[i];
		if (symbol == '(' || symbol == '[' || symbol == '{')
		{
			push(stack, symbol);
			continue;
		}
		if (symbol == ')' || symbol == ']' || symbol == '}')
		{
			bool isEmptyStack = false;
			char popSymbol = pop(stack, isEmptyStack);
			if (isEmptyStack || !isEquivalentSymbols(popSymbol, symbol))
			{
				deleteStack(stack);
				return false;
			}
			continue;
		}

		deleteStack(stack);
		return false;
	}

	if (!isEmpty(stack))
	{
		deleteStack(stack);
		return false;
	}

	deleteStack(stack);
	return true;
}

bool test1()
{
	string testingString = "{{(()({({}){[({})]}})())[]()}}";
	return isBalance(testingString);
}

bool test2()
{
	string testingString = "({)}";
	return !isBalance(testingString);;
}

bool test3()
{
	string testingString = "((((((({{{}}}[]))))))";
	return !isBalance(testingString);
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
	string inputString = "";
	cout << "введите последовательность:" << endl;
	getline(cin, inputString);

	if (isBalance(inputString))
	{
		cout << "есть баланс" << endl;
		return 0;
	}
	cout << "нет баланса" << endl;
	return 0;
}