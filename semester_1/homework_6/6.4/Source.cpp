#include <iostream>
#include <fstream>
#include <string>
#include "list.h"

using namespace std;

struct Data
{
	string name;
	string phone;
};

bool inputList(List *&list)
{
	ifstream input("input.txt");
	if (!input.is_open())
	{
		cout << "ошибка. файл не найден" << endl;
		return false;
	}

	string newRecord = "";
	getline(input, newRecord);
	if (newRecord == "")
	{
		cout << "пустой файл" << endl;
		return false;
	}

	do
	{
		int spacePosition = newRecord.find(' ');
		string newName = newRecord.substr(0, spacePosition);
		string newPhone = newRecord.erase(0, spacePosition + 1);

		Data newEntry = { newName, newPhone };
		addToEnd(list, newEntry);
	} while (getline(input, newRecord));

	input.close();
	return true;
}

List *merge(List *&firstHalf, List *&secondHalf, const bool sortPhone)
{
	List *list = createList();
	while (sizeOfList(firstHalf) != 0 && sizeOfList(secondHalf) != 0)
	{
		if (read(firstHalf).phone < read(secondHalf).phone && sortPhone ||
			read(firstHalf).name < read(secondHalf).name && !sortPhone)
		{
			addToEnd(list, pop(firstHalf));
		}
		else
		{
			addToEnd(list, pop(secondHalf));
		}
	}

	if (sizeOfList(firstHalf) != 0)
	{
		const int sizeOfFirstHalf = sizeOfList(firstHalf);
		for (int i = 0; i < sizeOfFirstHalf; i++)
		{
			addToEnd(list, pop(firstHalf));
		}
	}
	if (sizeOfList(secondHalf) != 0)
	{
		const int sizeOfSecondHalf = sizeOfList(secondHalf);
		for (int i = 0; i < sizeOfSecondHalf; i++)
		{
			addToEnd(list, pop(secondHalf));
		}
	}

	deleteList(firstHalf);
	deleteList(secondHalf);
	return list;
}

List *halfSeparatedList(List *&list)
{
	List *halfList = createList();
	const int sizeOfHalfList = sizeOfList(list) / 2;

	for (int i = 0; i < sizeOfHalfList; i++)
	{
		addToEnd(halfList, pop(list));
	}
	return halfList;
}

void mergeSort(List *&list, const bool sortPhone)
{
	if (sizeOfList(list) <= 1)
	{
		return;
	}

	List* firstHalf = halfSeparatedList(list);
	List* secondHalf = list;

	mergeSort(firstHalf, sortPhone);
	mergeSort(secondHalf, sortPhone);
	list = merge(firstHalf, secondHalf, sortPhone);
}

void reference()
{
	cout << "входной файл: input.txt" << endl;
	cout << "записи вида: \"имя <пробел> номер телефона\"" << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	reference();

	List *list = createList();
	if (!inputList(list))
	{
		return 0;
	}
	cout << "сортровка по имени: 0\nсортировка по номеру телефона: 1\nвведите команду: ";
	bool sortPhone = true;
	cin >> sortPhone;

	mergeSort(list, sortPhone);
	cout << "отсортированный список:" << endl;
	printList(list);

	deleteList(list);
	return 0;
}