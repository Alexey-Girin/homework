#include "list.h"
#include <iostream>
#include <vector>

using namespace std;

struct Vertex
{
	int vertex;
	int edge;
	bool isDelete;
};

struct Element
{
	Vertex vertexData;
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

void add(List *&list, const Vertex &newVertexData)
{
	if (list->head == nullptr)
	{
		list->head = new Element{ newVertexData, nullptr };
		return;
	}

	Element *position = list->head;
	Element *savePosition = position;
	while (newVertexData.edge > position->vertexData.edge)
	{
		if (position->next == nullptr)
		{
			position->next = new Element{ newVertexData, nullptr };
			return;
		}
		savePosition = position;
		position = position->next;
	}

	Element *newElement = new Element{ newVertexData, position };
	if (position == list->head)
	{
		list->head = newElement;
		return;
	}
	savePosition->next = newElement;
}

Vertex read(List *&list)
{
	Element *position = list->head;
	while (position != nullptr)
	{
		if (!position->vertexData.isDelete)
		{
			return position->vertexData;
		}
		position = position->next;
	}

	return{ -1, -1 };
}

void block(List *&list, const int blockVertex)
{
	Element *position = list->head;
	while (position != nullptr && blockVertex != position->vertexData.vertex)
	{
		position = position->next;
	}
	if (position == nullptr)
	{
		return;
	}
	position->vertexData.isDelete = true;
}

void blockRoadToCity(vector<List*> &adjacencyList, const int city)
{
	Element *position = adjacencyList[city]->head;
	while (position != nullptr)
	{
		int adjacenceVertex = position->vertexData.vertex;
		block(adjacencyList[adjacenceVertex], city);
		position = position->next;
	}
}

void removeList(List *&list)
{
	if (list == nullptr)
	{
		return;
	}

	while (list->head != nullptr)
	{
		Element *position = list->head;
		list->head = list->head->next;
		delete position;
	}
	list = nullptr;
}
