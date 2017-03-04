#include <iostream>

using namespace std;

void revers(int mas[], int start, int end)
{
	for (int i = start; i <= start + (end - start) / 2; i++)
	{
		swap(mas[i], mas[end + start - i]);
	}
}

int main()
{
	setlocale(LC_ALL, "Russian");
	int m;
	int n;
	cout << "введите m и n: ";
	cin >> m >> n;
	int *mas = new int[m + n];
	cout << "введите элементы массива: " << endl;
	for (int i = 0; i < m + n; i++)
	{
		cin >> mas[i];
	}
	if (m != 0)
	{
		revers(mas, 0, m - 1);
	}
	if (n != 0)
	{
		revers(mas, m, m + n - 1);
	}
	revers(mas, 0, m + n - 1);
	cout << "переставленный массив: " << endl;
	for (int i = 0; i < m + n; i++)
	{
		cout << mas[i] << " ";
	}
	cout << endl;
	delete[] mas;
	return 0;
}