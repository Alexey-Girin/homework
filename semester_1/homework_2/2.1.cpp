#include <iostream> 
#include <cmath>

using namespace std;

int recursionFibonacci(int num)
{
	if (num == 1 || num == 2)
	{
		return 1;
	}
	return recursionFibonacci(num - 1) + recursionFibonacci(num - 2);
}

int iterationFibonachi(int num)
{
	if (num == 1 || num == 2)
	{
		return 1;
	}
	int masFib[3] = {};
	masFib[0] = 1;
	masFib[1] = 1;
	for (int i = 2; i < num; i++)
	{
		if (i % 3 == 0)
		{
			masFib[0] = masFib[1] + masFib[2];
		}
		if (i % 3 == 1)
		{
			masFib[1] = masFib[0] + masFib[2];
		}
		if (i % 3 == 2)
		{
			masFib[2] = masFib[0] + masFib[1];
		}
	}
	return masFib[(num - 1) % 3];
}

int main()
{
	setlocale(LC_ALL, "Russian");
	bool start = false;
	cout << "введите 1 для рекурсивного вычисления или 0 для итеративного вычисления: ";
	cin >> start;
	cout << "введите номер: ";
	int num = 0;
	cin >> num;
	cout << "результат: ";
	if (start)
	{
		cout << recursionFibonacci(num) << endl;
	}
	else
	{
		cout << iterationFibonachi(num) << endl;
	}
	return 0;
}