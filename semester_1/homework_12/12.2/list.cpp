#include "list.h"
#include <iostream>
#include <vector>

using namespace std;

struct Vertex
{
	int vertex;
	int distanceToTree;
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
	while (newVertexData.distanceToTree > position->vertexData.distanceToTree)
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

int pop(List *&list)
{
	Element *position = list->head;
	if (position == nullptr)
	{
		return -1;
	}

	int popVertex = position->vertexData.vertex;
	list->head = position->next;
	delete position;
	return popVertex;
}

void removeElement(List *&list, int deleteVertex)
{
	Element *position = list->head;
	Element *savePosition = position;
	while (position->vertexData.vertex != deleteVertex)
	{
		savePosition = position;
		position = position->next;
	}

	if (position == list->head)
	{
		list->head = position->next;
		delete position;
		return;
	}
	
	savePosition->next = position->next;
	delete position;
}

bool isBelongWithLongDistance(List *&list, const Vertex sourceVertex)
{
	Element *position = list->head;
	while (position != nullptr)
	{
		if (position->vertexData.vertex == sourceVertex.vertex &&
			position->vertexData.distanceToTree > sourceVertex.distanceToTree)
		{
			removeElement(list, position->vertexData.vertex);
			add(list, sourceVertex);
			return true;
		}
		position = position->next;
	}
	return false;
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