#include <iostream> 
#include <fstream>
#include <vector>
#include <string>
#include "sort.h"

using namespace std;

int mostСommonElement(const vector<int> &mas)
{
	int max = 1;
	int currentNum = 1;
	int res = mas[0];
	int num = mas.size();
	for (int i = 1; i < num; i++)
	{
		if (mas[i] == mas[i - 1])
		{
			currentNum++;
		}
		else
		{
			if (currentNum > max)
			{
				max = currentNum;
				res = mas[i - 1];
			}
			currentNum = 1;
		}
	}
	if (currentNum > max)
	{
		max = currentNum;
		res = mas[num - 1];
	}
	return res;
}

void input(vector<int> &mas, string &inputResult)
{
	ifstream fileIn("file.txt");
	if (!fileIn.is_open())
	{
		inputResult = "ошибка. нет файла file.txt";
		return;
	}
	int element = 0;
	while (fileIn >> element)
	{
		mas.push_back(element);
	}
	fileIn.close();
	if (mas.size() == 0)
	{
		inputResult = "пустой файл";
		return;
	}
}

bool test1()
{
	vector<int> masTest = { 1 };
	quicksort(masTest, 0, masTest.size() - 1);
	const int resultTest = 1;
	return mostСommonElement(masTest) == resultTest;
}

bool test2()
{
	vector<int> masTest = { 3, 2, 2, 1, 3, 3, 1, 1, 2, 3 };
	quicksort(masTest, 0, masTest.size() - 1);
	const int resultTest = 3;
	return mostСommonElement(masTest) == resultTest;
}

bool test3()
{
	vector<int> masTest = { 2, 2, 2, 3, 3, 3, 1, 1, 1 };
	quicksort(masTest, 0, masTest.size() - 1);
	const int resultTest = 1;
	return mostСommonElement(masTest) == resultTest;
}

void testing()
{
	if (test1() && test2() && test3())
	{
		cout << "testing true" << endl;
		return;
	}
	cout << "testing false" << endl;
}

void reference()
{
	cout << "если в массиве несколько элементов, встречающихся наиболее часто, выводится минимальный из них" << endl;
	cout << "значение массива берется из файла file.txt\n___________" << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	testing();
	reference();
	vector<int> mas = {};
	string inputResult = "";
	input(mas, inputResult);
	if (inputResult != "")
	{
		cout << inputResult << endl;
		return 0;
	}
	quicksort(mas, 0, mas.size() - 1);
	int res = mostСommonElement(mas);
	cout << "наиболее часто встречающийся элемент в массиве: " << res << endl;
	return 0;
}