#include <iostream>
#include <vector>
#include <fstream>

using namespace std;

const int stateTableLength = 3;
const int stateTableHeight = 7;

enum class State
{
	initialPosition,
	openSlash,
	openAsterisk,
	closeAsterisk,
	internalSymbol,
	externalSymbol,
	closeSlash
};

bool isSlash(const char &symbol)
{
	return symbol == '/';
}

bool isAsterisk(const char &symbol)
{
	return symbol == '*';
}

void print(State &state, const char symbol)
{
	switch (state)
	{
	case State::openAsterisk:
		cout << "/*";
		return;
	case State::closeAsterisk:
		cout << '*';
		return;
	case State::internalSymbol:
		cout << symbol;
		return;
	case State::closeSlash:
		cout << '/' << endl << "-------" << endl;
		return;
	default:
		return;
	}
}

void condition(State &state, const char symbol, vector<vector<State>> &stateTable)
{
	if (isAsterisk(symbol))
	{
		state = stateTable[static_cast<int>(state)][0];
	}
	else if (isSlash(symbol))
	{
		state = stateTable[static_cast<int>(state)][1];
	}
	else
	{
		state = stateTable[static_cast<int>(state)][2];
	}

	print(state, symbol);
}

void createStateTable(vector<vector<State>> &stateTable)
{
	ifstream fin("stateTable.txt");
	for (int i = 0; i < stateTableHeight; i++)
	{
		for (int t = 0;t < stateTableLength; t++)
		{
			int state = 0;
			fin >> state;
			stateTable[i][t] = static_cast<State> (state);
		}
	}
}

void findComments(FILE *ptrfile)
{
	State state = State::initialPosition;
	vector<vector<State>> stateTable(stateTableHeight, vector<State>(stateTableLength, state));
	createStateTable(stateTable);
	int getSymbol = 0;
	while (true)
	{
		getSymbol = getc(ptrfile);
		if (getSymbol == EOF)
		{
			fclose(ptrfile);
			cout << endl;
			return;
		}
		condition(state, static_cast<char>(getSymbol), stateTable);
	}
}