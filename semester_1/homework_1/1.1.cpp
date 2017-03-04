#include <iostream>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	int x;
	cout << "введите X: ";
	cin >> x;
	int a = x*x;
	a = a*(a + x) + a + x + 1;
	cout << "результат: " << a << endl;
	return 0;
}