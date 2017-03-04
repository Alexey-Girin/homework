#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include "list.h"
#include "hashTable.h"

using namespace std;

struct LineDate
{
	string word;
	int frequency;
};

void hashFile(vector<List*> &array, const string &fileName)
{
	ifstream input(fileName);
	if (!input.is_open())
	{
		cout << "файл не найден" << endl;
	}

	string inputName = "";
	double fillFactor = 0;
	while (input >> inputName)
	{
		LineDate inputLine = { inputName , 1 };
		hashing(array, inputLine, fillFactor);
		if (fillFactor > 1)
		{
			rehashing(array, fillFactor);
		}
	}

	input.close();
}

int main()
{
	setlocale(LC_ALL, "Russian");

	double fillFactor = 0;
	int sizeOfArray = 2;
	vector<List*>array(sizeOfArray, nullptr);

	const string fileName = "file.txt";
	hashFile(array, fileName);

	printArray(array);
	deleteArray(array);
	return 0;
}