#include "stack.h"
#include <iostream>

struct Element
{
	FlagType value;
	Element *next;
};

struct Stack
{
	Element *head;
};

Stack *createStack()
{
	return new Stack{ nullptr };
}

void push(Stack *&stack, FlagType value)
{
	Element *newElement = new Element{ value, stack->head };
	stack->head = newElement;
}

bool isEmpty(Stack *&stack)
{
	if (!stack->head)
	{
		return true;
	}
	return false;
}

FlagType pop(Stack *&stack, bool &isEmptyStack)
{
	if (isEmpty(stack))
	{
		isEmptyStack = true;
		return -1;
	}
	Element *position = stack->head;
	FlagType popElement = position->value;
	stack->head = position->next;
	delete position;
	return popElement;
}

void deleteStack(Stack *&stack)
{
	bool isEmptyStack = false;
	while (!isEmptyStack)
	{
		pop(stack, isEmptyStack);
	}
	delete stack;
	stack = nullptr;
}

void printStack(Stack *&stack)
{
	Element *position = stack->head;
	while (position)
	{
		std::cout << position->value << std::endl;
		position = position->next;
	}
}