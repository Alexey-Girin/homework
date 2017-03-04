#include <cstdlib> 
#include <iostream>
#include <ctime>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "введите кол-во элементов массива: ";
	int num = 0;
	cin >> num;
	int *masRandom = new int[num];

	cout << "рандомный массив: " << endl;
	srand(time(0));
	for (int i = 0; i < num; i++)
	{
		masRandom[i] = rand() % 100;
		cout << masRandom[i] << " ";
	}
	cout << endl;

	int position = 1;
	bool coincidenceWithFirst = false;
	for (int i = 1; i < num; i++)
	{
		if (coincidenceWithFirst == false && masRandom[i] == masRandom[0])
		{
			coincidenceWithFirst = true;
		}
		if (masRandom[i] < masRandom[0])
		{
			if (i != position)
			{
				swap(masRandom[i], masRandom[position]);
			}
			position++;
		}
	}
	if (coincidenceWithFirst == true)
		for (int i = position; i < num; i++)
		{
			if (masRandom[i] == masRandom[0])
			{
				if (i != position)
				{
					swap(masRandom[i], masRandom[position]);
				}
				position++;
			}
		}

	cout << "полученный массив: " << endl;
	for (int i = 0; i < num; i++)
	{
		cout << masRandom[i] << " ";
	}
	cout << endl;
	delete[] masRandom;
	return 0;
}