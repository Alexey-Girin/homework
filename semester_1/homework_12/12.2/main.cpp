#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include "list.h"

using namespace std;

struct Vertex
{
	int vertex;
	int distanceToTree;
};

bool isNumber(const string &line)
{
	const int size = line.size();
	for (int i = 0; i < size; i++)
	{
		if (line[i] < '0' || line[i] > '9')
		{
			return false;
		}
	}
	return true;
}

bool fillMas(vector<int> &mas, string &line)
{
	if (line.size() == 0)
	{
		return false;
	}

	const char tab = '	';
	while (true)
	{
		int tabPosition = line.find(tab);
		if (tabPosition == -1)
		{
			if (mas.size() == 0 && !isNumber(line))
			{
				return false;
			}

			int vertex = stoi(line);
			mas.push_back(vertex);
			return true;
		}

		int vertex = stoi(line.substr(0, tabPosition));
		mas.push_back(vertex);
		line.erase(0, tabPosition + 1);
	}
}

bool inputList(vector<vector<int>> &adjacencyList, const string &inputFileName)
{
	ifstream fin(inputFileName);
	string line = "";
	getline(fin, line);

	vector<int> mas(0);
	if (!fillMas(mas, line))
	{
		cout << "ошибка ввода" << endl;
		fin.close();
		return false;
	}
	adjacencyList.push_back(mas);

	const int numOfVertex = mas.size();
	for (int i = 1; i < numOfVertex; i++)
	{
		adjacencyList.push_back(vector<int>(numOfVertex, 0));
		for (int j = 0; j < numOfVertex; j++)
		{
			fin >> adjacencyList[i][j];
		}
	}
	fin.close();
	return true;
}

void changeList(const vector<vector<int>> &adjacencyList, const int mainVertex, List *&list, vector<bool> &isUsed)
{
	const int numOfVertex = adjacencyList.size();
	for (int i = 1; i < numOfVertex; i++)
	{
		int vertex = adjacencyList[mainVertex][i];
		if (i == mainVertex || isUsed[i] || vertex == 0)
		{
			continue;
		}
		if (isBelongWithLongDistance(list, { i, vertex }))
		{
			continue;
		}
		add(list, { i, vertex });
	}
}

void buildTree(const vector<vector<int>> &adjacencyList, vector<int> &tree)
{
	const int numOfVertex = adjacencyList.size();
	vector<bool> isUsed(numOfVertex, false);
	isUsed[0] = true;

	List *list = createList();
	changeList(adjacencyList, 0, list, isUsed);

	for (int i = 1; i < numOfVertex; i++)
	{
		int addedVertex = pop(list);
		isUsed[addedVertex] = true;
		tree.push_back(addedVertex);
		changeList(adjacencyList, addedVertex, list, isUsed);
	}
	removeList(list);
}

void print(vector<int> &tree)
{
	cout << "вершины минимального остовного дерева в порядке добавления:" << endl;
	const int size = tree.size();
	for (int i = 0; i < size; i++)
	{
		cout << tree[i] << " ";
	}
	cout << endl;
}

void test()
{
	vector<vector<int>> testAdjacencyList(0);
	const string inputFileName = "test.txt";
	inputList(testAdjacencyList, inputFileName);

	vector<int> tree(1, 0);
	buildTree(testAdjacencyList, tree);
	
	vector<int> trueTree = { 0, 3, 4, 5, 6, 2, 1 };
	const int size = tree.size();
	if (size != trueTree.size())
	{
		cout << "testing fasle" << endl;
		return;
	}
	for (int i = 0; i < size; i++)
	{
		if (tree[i] != trueTree[i])
		{
			cout << "testing false";
			return;
		}
	}
	cout << "testing true" << endl;
}

int main()
{
	test();
	setlocale(LC_ALL, "Russian");

	vector<vector<int>> adjacencyList(0);
	const string inputFileName = "input.txt";
	if (!inputList(adjacencyList, inputFileName))
	{
		return 0;
	}

	vector<int> tree(1, 0);
	buildTree(adjacencyList, tree);
	print(tree);
}