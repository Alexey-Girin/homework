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

//возвращение данных ближайшей незаблокированной вершины
Vertex read(List *&list);

//блокирует доступ к вершине из других вершин
void blockRoadToCity(std::vector<List*> &adjacencyList, const int city);

//удаление списка
void removeList(List *&list);