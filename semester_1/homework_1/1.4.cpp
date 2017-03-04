#include <iostream>
#include <cmath>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	int Mas[28] = { 0 };
	for (int i = 0; i <= 9; i++)
	{
		for (int t = 0; t <= 9; t++)
		{
			for (int k = 0; k <= 9; k++)
			{
				Mas[i + t + k]++;
			}
		}
	}
	int Res = 0;
	for (int i = 0; i < 28; i++)
	{
		Res += pow(Mas[i], 2);
	}
	cout << Res << endl;
	return 0;
}