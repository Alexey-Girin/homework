#pragma once

typedef double FlagType;

//структура данных стек
struct Stack;

//создание стека
Stack *createStack();

//добавление элемента в стек
void push(Stack *&stack, FlagType value);

//проверка стека на пустоту
bool isEmpty(Stack *&stack);

//удаление элемента из стека
FlagType pop(Stack *&stack, bool &isEmptyStack);

//удаление стека
void deleteStack(Stack *&stack);

//печать стека
void printStack(Stack *&stack);