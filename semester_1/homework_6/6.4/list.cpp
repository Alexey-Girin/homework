#include "list.h"
#include <iostream>
#include <string>

using namespace std;

struct Data
{
	string name;
	string phone;
};

struct Element
{
	Data entry;
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
	return list->head == nullptr;
}

void addToEnd(List *&list, const Data &newEntry)
{
	if (isEmpty(list))
	{
		list->head = new Element{ newEntry, nullptr };
		return;
	}

	Element *position = list->head;
	while (position->next != nullptr)
	{
		position = position->next;
	}
	position->next = new Element{ newEntry, nullptr };
}

Data read(List *&list)
{
	Data readEntry = { "", "" };
	if (isEmpty(list))
	{
		return readEntry;
	}

	readEntry = list->head->entry;
	return readEntry;
}

Data pop(List *&list)
{
	Data popEntry = { "", "" };
	if (isEmpty(list))
	{
		return popEntry;
	}

	Element *popElement = list->head;
	popEntry = popElement->entry;
	list->head = list->head->next;
	delete popElement;
	return popEntry;
}

void printList(List *&list)
{
	Element *position = list->head;
	if (position == nullptr)
	{
		cout << "список пуст" << endl;
		return;
	}

	while (position)
	{
		cout << position->entry.name << " " << position->entry.phone << endl;
		position = position->next;
	}
}

void deleteList(List *&list)
{
	while (list->head != nullptr)
	{
		Element *deleteElement = list->head;
		list->head = list->head->next;
		delete deleteElement;
	}

	delete list;
	list = nullptr;
}

int sizeOfList(List *&list)
{
	Element *position = list->head;
	int result = 0;
	while (position)
	{
		position = position->next;
		result++;
	}
	return result;
}
