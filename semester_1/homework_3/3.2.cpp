#include <iostream> 
#include <ctime>
#include <cstdlib>
#include <cmath>

using namespace std;

bool binarySearch(int element, int mas[], int num)
{
	int low = 0;
	int high = num - 1;
	int mid = 0;
	while (low <= high)
	{
		mid = low + (high - low) / 2;
		if (element == mas[mid])
		{
			return true;
		}
		else
		{
			if (element > mas[mid])
			{
				low = mid + 1;
			}
			else
			{
				high = mid - 1;
			}
		}
	}
	return (element == mas[low]);
}

void merge(int mas[], int low, int mid, int high)
{
	int iLow = 0;
	int iHigh = 0;
	const int num = high - low;
	int *result = new int[num] {};
	while (low + iLow < mid && mid + iHigh < high)
	{
		if (mas[low + iLow] < mas[mid + iHigh])
		{
			result[iLow + iHigh] = mas[low + iLow];
			iLow++;
		}
		else
		{
			result[iLow + iHigh] = mas[mid + iHigh];
			iHigh++;
		}
	}
	while (low + iLow < mid)
	{
		result[iLow + iHigh] = mas[low + iLow];
		iLow++;
	}
	while (mid + iHigh < high)
	{
		result[iLow + iHigh] = mas[mid + iHigh];
		iHigh++;
	}
	for (int i = 0; i < num; i++)
	{
		mas[low + i] = result[i];
	}
	delete[] result;
}

void mergeSort(int mas[], int low, int high)
{
	if (low + 1 >= high)
	{
		return;
	}
	int mid = (low + high) / 2;
	mergeSort(mas, low, mid);
	mergeSort(mas, mid, high);
	merge(mas, low, mid, high);
}

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "введите значение n: ";
	int n = 0;
	cin >> n;
	cout << "введите значения k: ";
	int k = 0;
	cin >> k;
	const int max = pow(10, 9);
	srand(time(0));

	cout << "сгенерированный массив: \n";
	int *masN = new int[n];
	for (int i = 0; i < n; i++)
	{
		masN[i] = rand() % max;
		cout << masN[i] << " ";
	}
	cout << endl;

	int *masK = new int[k];
	for (int i = 0; i < k; i++)
	{
		masK[i] = rand() % max;
	}

	mergeSort(masN, 0, n);

	cout << "сгенерированные k чисел: \n";
	for (int i = 0; i < k; i++)
	{
		cout << masK[i];
		if (binarySearch(masK[i], masN, n))
		{
			cout << "\t- содержится в массиве\n";
		}
		else
		{
			cout << "\t- не содержится в массиве\n";
		}
	}
	delete[] masN;
	delete[] masK;
	return 0;
}