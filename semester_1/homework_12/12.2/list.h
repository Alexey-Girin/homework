#pragma once
#include <vector>

//данные вершины
struct Vertex;

//отсортированный список вершин
struct List;

//создание списка вершин
List *createList();

//добавление новой вершины в отсортированный список
void add(List *&list, const Vertex &newVertexData);

//изъятие головной вершины списка
int pop(List *&list);

//обновляет позицию вершины в списке, если вершина в нем уже содержится и distanceToTree уменьшилось (true), иначе (false)
bool isBelongWithLongDistance(List *&list, const Vertex sourceVertex);

//удаление списка
void removeList(List *&list);