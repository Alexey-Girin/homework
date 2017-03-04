#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

void revers(vector<int> &mas)
{
	for (int i = 0; i < mas.size() / 2; i++)
	{
		swap(mas[i], mas[mas.size() - i - 1]);
	}
}

void binary(vector<int> &mas, int num)
{
	int bit = 0b1;
	for (int i = 0; i < sizeof(int) * 8; i++)
	{
		mas.push_back((bit & num) ? 1 : 0);
		bit = bit << 1;
	}
	revers(mas);
	cout << endl;
}

void print(const vector<int> &mas)
{
	for (int i = 0; i < mas.size(); i++)
	{
		cout << mas[i];
	}
	cout << endl;
}

void sum(vector<int> &sumBin, const vector<int> &masFirstNum, const vector<int> &masSecondNum)
{
	int digit = 0;
	for (int i = masFirstNum.size() - 1; i >= 0; i--)
	{
		int sum = masFirstNum[i] + masSecondNum[i] + digit;
		sumBin.push_back((sum % 2 == 0) ? 0 : 1);
		digit = ((sum >= 2) ? 1 : 0);
	}
	for (int i = 0; i < sumBin.size() / 2; i++)
	{
		swap(sumBin[i], sumBin[sumBin.size() - i - 1]);
	}
}

int decimal(vector<int> mas)
{
	int powOfTwo = 1;
	revers(mas);
	int res = 0;
	int bit = 0b1;
	for (int i = 0; i < mas.size(); i++)
	{
		res += ((bit & mas[i]) ? powOfTwo : 0);
		powOfTwo *= 2;
	}
	return res;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "введите два числа [-128;127]: ";
	int firstNum = 0;
	cin >> firstNum;
	int secondNum = 0;
	cin >> secondNum;

	vector<int> masFirstNum = {};
	cout << "двоичное представление первого числа: ";
	binary(masFirstNum, firstNum);
	print(masFirstNum);

	vector<int> masSecondNum = {};
	cout << "двоичное представление второго числа: ";
	binary(masSecondNum, secondNum);
	print(masSecondNum);

	vector<int> binSum = {};
	cout << "двоичное представление суммы чисел: ";
	sum(binSum, masFirstNum, masSecondNum);
	print(binSum);
	masFirstNum.clear();
	masSecondNum.clear();

	cout << "сумма чисел в 10-ой записи: " << decimal(binSum) << endl;
	binSum.clear();
	return 0;
}