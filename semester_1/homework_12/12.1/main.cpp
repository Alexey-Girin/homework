#include <iostream>
#include <vector>
#include <string>
#include <fstream>

using namespace std;

void addShiftForSymbol(vector<int> &offsetTable, const char symbol, const string &sample)
{
	int symbolCode = static_cast<int>(symbol);
	if (offsetTable[symbolCode] != -1)
	{
		return;
	}

	const int sizeSample = sample.size();
	int num = 0;
	while (num < sizeSample - 1 && symbol != sample[num])
	{
		num++;
	}
	
	offsetTable[symbolCode] = ((num == sizeSample - 1) ? sizeSample : sizeSample - num - 1);
}

bool findMatch(const string &line, const string &sample, vector<int> &offsetTable, int &shift)
{
	int num = line.size() - 1;
	if (line[num] != sample[num])
	{
		int symbolCode = static_cast<int>(line[num]);
		shift = offsetTable[symbolCode];
		return false;
	}

	do
	{
		num--;
	} while (num != -1 && line[num] == sample[num]);

	shift = 1;
	return ((num == -1) ? true : false);
}

int passAlongText(vector<int> &offsetTable, const string &sample, const string &fileName)
{
	ifstream fin(fileName);
	const int sizeSample = sample.size();
	string line = "";
	for (int i = 0; i < sizeSample && fin.good(); i++)
	{
		char symbol = 0;
		fin.get(symbol);
		addShiftForSymbol(offsetTable, symbol, sample);
		line += symbol;
	}
	if (line.size() != sizeSample)
	{
		fin.close();
		return -1;
	}

	int num = 0;
	while (true)
	{
		int shift = 0;
		if (findMatch(line, sample, offsetTable, shift))
		{
			fin.close();
			return num;
		}

		for (int i = 0; i < shift && fin.good(); i++)
		{
			char symbol = 0;
			fin.get(symbol);
			addShiftForSymbol(offsetTable, symbol, sample);
			line += symbol;
		}
		line.erase(0, shift);
		if (line.size() != sizeSample)
		{
			fin.close();
			return -1;
		}
		num += shift;
	}
}

void test()
{
	string sample = "the end of";
	vector<int> offsetTable(256, -1);
	const string fileName = "test.txt";
	const int truePosition = 52;

	if (passAlongText(offsetTable, sample, fileName) == truePosition)
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

	string sample = "";
	cout << "введите образец:" << endl;
	getline(cin, sample);

	vector<int> offsetTable(256, -1);
	const string fileName = "input.txt";
	int positionOfMatch = passAlongText(offsetTable, sample, fileName);

	if (positionOfMatch == -1)
	{
		cout << "вхождений нет" << endl;
		return 0;
	}
	cout << "позиция первого вхождения: " << positionOfMatch << endl;
}