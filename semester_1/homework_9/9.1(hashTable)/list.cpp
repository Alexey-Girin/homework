#include "list.h"
#include <iostream>
#include <string>
#include <fstream>

using namespace std;

struct LineDate
{
	string word;
	int frequency;
};

struct Element
{
	LineDate line;
	Element *next;
};

struct List
{
	Element *head;
};

List *createList()
{
	return new List{ nullptr };
}

bool isEmpty(List *&list)
{
	if (!list->head)
	{
		return true;
	}
	return false;
}

bool findAndAdd(List *&list, const LineDate newLine)
{
	if (isEmpty(list))
	{
		list->head = new Element{ newLine, nullptr };
		return false;
	}

	Element *position = list->head;
	while (position->line.word != newLine.word)
	{
		if (position->next == nullptr)
		{
			position->next = new Element{ newLine, nullptr };
			return false;
		}
		position = position->next;
	}
	position->line.frequency += newLine.frequency;
	return true;
}

void printList(List *&list, int &currentLenght)
{
	if (list == nullptr)
	{
		return;
	}

	Element *position = list->head;
	while (position != nullptr)
	{
		currentLenght++;
		cout << position->line.word << ": " << position->line.frequency << endl;
		position = position->next;
	}
}

const LineDate pop(List *&list)
{
	if (list == nullptr || isEmpty(list))
	{
		LineDate popLine = { "", -1 };
		return popLine;
	}

	Element *popElement = list->head;
	LineDate popLine = popElement->line;
	list->head = list->head->next;
	delete popElement;
	return popLine;
}

void deleteList(List *&list)
{
	while (pop(list).frequency != -1)
	{

	}

	delete list;
	list = nullptr;
}