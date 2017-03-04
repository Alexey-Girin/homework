#pragma once

//тип данных список
struct List;

//хранимые в списке данные
struct Data;

//создание списка
List *createList();

//добавление новой записи
void addToEnd(List *&list, const Data &newEntry);

//размерность списка
int sizeOfList(List *&list);

//чтение головного элемента списка
Data read(List *&list);

//изымание головного элемента списка
Data pop(List *&list);

//печать списка 
void printList(List *&list);

//удаление списка
void deleteList(List *&list);
