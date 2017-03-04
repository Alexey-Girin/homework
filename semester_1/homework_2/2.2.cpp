#include <iostream> 
#include <cmath>

using namespace std;

int iterativeInvolution(int num, int degree)
{
	int result = 1;
	for (int i = 0; i < degree; i++)
	{
		result *= num;
	}
	return result;
}

int exponentiationBySquaring(int num, int degree)
{
	int result = 1;
	while (degree != 0)
	{
		if (degree % 2)
		{
			result *= num;
		}
		num *= num;
		degree /= 2;
	}
	return result;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "введите число : ";
	int num = 0;
	cin >> num;
	cout << "введите положительную степень : ";
	int degree = 0;
	cin >> degree;
	int resIterativeInvolution = iterativeInvolution(num, degree);
	cout << "возведение 1: " << resIterativeInvolution << endl;
	long int resExponentiationBySquaring = exponentiationBySquaring(num, degree);
	cout << "возведение 2: " << resExponentiationBySquaring << endl;
	return 0;
}