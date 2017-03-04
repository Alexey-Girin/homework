#include <iostream>
#include <string>

using namespace std;

struct Element
{
	int k;
	Element *next;
};

void createCycle(Element *&start, int n)
{
	Element *newElement = new Element{ 0, nullptr };
	start = newElement;
	Element *position = start;
	for (int i = 1; i < n; i++)
	{
		newElement = new Element{ i, nullptr };
		position->next = newElement;
		position = newElement;
	}
	position->next = start;
}

int searchSurvivor(Element *position, int m, int n)
{
	Element *previousPosition = position;
	while (position->next != position)
	{
		for (int t = 0; t < n + m - 1; t++)
		{
			previousPosition = position;
			position = position->next;
		}
		previousPosition->next = position->next;
		delete position;
		position = previousPosition->next;
		n--;
	}
	int res = position->k;
	delete position;
	return res;
}

int resultTest(int n, int m)
{
	Element *start = nullptr;
	createCycle(start, n);
	int result = searchSurvivor(start, m, n) + 1;
	start = nullptr;
	return result;
}

bool test1()
{
	int n = 2;
	int m = 1;
	int resultTrue = 2;
	return resultTest(n, m) == resultTrue;
}

bool test2()
{
	int n = 10;
	int m = 5;
	int resultTrue = 3;
	return resultTest(n, m) == resultTrue;
}

bool test3()
{
	int n = 7;
	int m = 10;
	int resultTrue = 5;
	return resultTest(n, m) == resultTrue;
}

void testing()
{
	if (test1() && test2() && test3())
	{
		cout << "tesing true" << endl;
		return;
	}
	cout << "testing false" << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	testing();

	cout << "введите значение n: ";
	int n = 0;
	cin >> n;
	cout << "введите значение m: ";
	int m = 0;
	cin >> m;

	Element *start = nullptr;
	createCycle(start, n);
	cout << "значение k: " << searchSurvivor(start, m, n) + 1 << endl;
	start = nullptr;
	return 0;
}