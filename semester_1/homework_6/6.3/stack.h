#pragma once
#include <iostream>

typedef char FlagType;

//тип данных стек
struct Stack;

//создание стека
Stack *createStack();

//добавление элемента в стек
void push(Stack *&stack, FlagType value);

//проверка стека на пустоту
bool isEmpty(Stack *&stack);

//чтение головного элемента стека
FlagType peek(Stack *&stack);

//удаление элемента из стека
void pop(Stack *&stack);

//удаление стека
void deleteStack(Stack *&stack);

//содержимое стека
std::string stackRemainder(Stack *&stack);