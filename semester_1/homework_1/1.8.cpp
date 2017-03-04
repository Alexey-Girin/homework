#include <iostream>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "введите кол-во элементов массива: ";
	int num;
	cin >> num;
	int *mas = new int[num];
	cout << "введите элементы массива: " << endl;
	int nullelement = 0;
	for (int i = 0; i < num; i++)
	{
		cin >> mas[i];
		if (mas[i] == 0)
		{
			nullelement++;
		}
	}
	cout << "кол-во нулевых элементов массива: " << nullelement << endl;
	delete[] mas;
	return 0;
}