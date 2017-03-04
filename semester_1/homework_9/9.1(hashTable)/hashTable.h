#pragma once
#include <iostream>
#include <vector>
#include "list.h"

//хэширование и добаление в массив новой строки
void hashing(std::vector<List*> &array, const LineDate &sourceLine, double &fillFactor);

//перехеширование строк
void rehashing(std::vector<List*> &array, double &fillFactor);

//печать строк (с частотой) и сведений таблицы
void printArray(std::vector<List*> &array);

//удаление массива
void deleteArray(std::vector<List*> &array);