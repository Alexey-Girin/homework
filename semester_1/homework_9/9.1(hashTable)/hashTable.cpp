#include "hashTable.h"
#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include "list.h"

using namespace std;

struct LineDate
{
	string word;
	int frequency;
};

unsigned int calculateHash(const string &key)
{
	unsigned int hash = 0;
	const unsigned int a = 1664525;
	const unsigned int b = 1013904223;

	int keySize = key.size();
	for (int i = 0; i < keySize; i++)
	{
		hash = (hash * a) + static_cast<int>(key[i]) + b;
	}

	return hash;
}

void hashing(vector<List*> &array, const LineDate &sourceLine, double &fillFactor)
{
	const int sizeOfArray = array.size();
	int hash = calculateHash(sourceLine.word) % sizeOfArray;

	if (array[hash] == nullptr)
	{
		array[hash] = createList();
	}

	if (!findAndAdd(array[hash], sourceLine))
	{
		fillFactor += 1.0 / sizeOfArray;
	}
}

void printArray(vector<List*> &array)
{
	const int sizeOfArray = array.size();
	int maxLength = 0;
	int numOfLine = 0;
	int sumLength = 0;

	cout << "слово: частота" << endl;
	for (int i = 0; i < sizeOfArray; i++)
	{
		if (array[i] == nullptr)
		{
			continue;
		}

		int currentLenght = 0;
		printList(array[i], currentLenght);

		numOfLine++;
		sumLength += currentLenght;
		if (currentLenght > maxLength)
		{
			maxLength = currentLenght;
		}
	}

	cout << "______" << endl;
	cout << "коэффициент заполнени€: " << static_cast<double>(sumLength) / sizeOfArray << endl;
	cout << "максимальна€ длина списка: " << maxLength << endl;
	cout << "средн€€ длина списка: ";
	if (numOfLine == 0)
	{
		cout << 0 << endl;
	}
	else
	{
		cout << static_cast<double>(sumLength) / numOfLine << endl;
	}
}

void deleteArray(vector<List*> &array)
{
	const int sizeOfArray = array.size();
	for (int i = 0; i < sizeOfArray; i++)
	{
		if (array[i] == nullptr)
		{
			continue;
		}
		deleteList(array[i]);
	}
}

void rehashing(vector<List*> &array, double &fillFactor)
{
	const int sizeOfArray = array.size();
	vector<List*> newArray(2 * sizeOfArray, nullptr);
	fillFactor = 0;

	for (int i = 0; i < sizeOfArray; i++)
	{
		while (true)
		{
			LineDate popLine = pop(array[i]);
			if (popLine.frequency == -1)
			{
				break;
			}
			hashing(newArray, popLine, fillFactor);
		}
	}

	deleteArray(array);
	array = newArray;
}