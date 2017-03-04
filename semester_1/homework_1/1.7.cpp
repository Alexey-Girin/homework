#include <iostream>
#include <cmath>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	int Chis, i, t, Del;
	cout << "введите число: ";	cin >> Chis;
	cout << "простые числа до заданного: " << endl;
	for (i = 2; i <= Chis; i++)
	{
		Del = 0;
		for (t = 2; t <= sqrt(i) && Del == 0; t++)
		{
			if (i%t == 0) Del++;
		}
		if (Del == 0) cout << i << "\t";
	}
	cout << endl;
	return 0;
}