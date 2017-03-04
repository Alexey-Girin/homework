#include <iostream>
#include <string>

using namespace std;

enum class State
{
	initialState,
	integerPart,
	point,
	fractionalPart,
	mantissa,
	sign,
	order,
	finiteState,
	error
};

bool isNumber(const char symbol)
{
	return symbol >= '0' && symbol <= '9';
}

bool isPoint(const char symbol)
{
	return symbol == '.';
}

bool isEOF(const char symbol)
{
	return symbol == '\0';
}

bool isMantissa(const char symbol)
{
	return symbol == 'E';
}

bool isSign(const char symbol)
{
	return symbol == '+' || symbol == '-';
}

bool isRealNumber(const string &sequence)
{
	State state = State::initialState;
	const int sequenceSize = sequence.size();

	for (int i = 0; i <= sequenceSize + 1; i++)
	{
		switch (state)
		{
		case State::initialState:
			if (!isNumber(sequence[i]))
			{
				state = State::error;
				break;
			}
			state = State::integerPart;
			break;
		case State::integerPart:
			if (isNumber(sequence[i]))
			{

			}
			else if (isPoint(sequence[i]))
			{
				state = State::point;
			}
			else if (isEOF(sequence[i]))
			{
				state = State::finiteState;
			}
			else if (isMantissa(sequence[i]))
			{
				state = State::mantissa;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::point:
			if (isNumber(sequence[i]))
			{
				state = State::fractionalPart;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::fractionalPart:
			if (isNumber(sequence[i]))
			{

			}
			else if (isEOF(sequence[i]))
			{
				state = State::finiteState;
			}
			else if (isMantissa(sequence[i]))
			{
				state = State::mantissa;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::mantissa:
			if (isSign(sequence[i]))
			{
				state = State::sign;
			}
			else if (isNumber(sequence[i]))
			{
				state = State::order;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::sign:
			if (isNumber(sequence[i]))
			{
				state = State::order;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::order:
			if (isNumber(sequence[i]))
			{

			}
			else if (isEOF(sequence[i]))
			{
				state = State::finiteState;
			}
			else
			{
				state = State::error;
			}
			break;
		case State::finiteState:
			return true;
		case State::error:
			return false;
		}
	}
}

void test()
{
	string testSequence1 = "";
	bool test1 = !isRealNumber(testSequence1);

	string testSequence2 = "12.302E-20";
	bool test2 = isRealNumber(testSequence2);

	string testSequence3 = "12E109";
	bool test3 = isRealNumber(testSequence3);

	string testSequence4 = "12.";
	bool test4 = !isRealNumber(testSequence4);

	string testSequence5 = "3453o";
	bool test5 = !isRealNumber(testSequence5);

	if (test1 && test2 && test3 && test4 && test5)
	{
		cout << "testing true" << endl;
		return;
	}
	cout << "testing false" << endl;
}

int main()
{
	test();
	setlocale(LC_ALL, "Russian");
	cout << "введите последовательность символов:" << endl;
	string sequence = "";
	getline(cin, sequence);

	if (isRealNumber(sequence))
	{
		cout << "вещественное число" << endl;
	}
	else
	{
		cout << "не вещественное число" << endl;
	}

	return 0;
}