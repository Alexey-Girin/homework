#include "stack.h"
#include <iostream>

using std::string;

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
	return stack->head == nullptr;
}

FlagType peek(Stack *&stack)
{
	return stack->head->value;
}

void pop(Stack *&stack)
{
	Element *position = stack->head;
	stack->head = position->next;
	delete position;
}

void deleteStack(Stack *&stack)
{
	while (!isEmpty(stack))
	{
		pop(stack);
	}
	delete stack;
	stack = nullptr;
}

string stackRemainder(Stack *&stack)
{
	string result = "";
	Element *position = stack->head;
	while (position)
	{
		result += position->value;
		position = position->next;
	}
	return result;
}