#pragma once
#include <iostream>

//данные строки (строка и ее частота)
struct LineDate;

//структура данных список
struct List;

//создание нового списка
List *createList();

//инкрементирование частоты встречаемости строки (true) или добаление строки к списку (с частотой = 1), если строка в нем отсутствует (false) 
bool findAndAdd(List *&list, const LineDate newLine);

//изымает данные головной строки из списка
const LineDate pop(List *&list);

//печать списка
void printList(List *&list, int &currentLenght);

//удаление списка
void deleteList(List *&list);
