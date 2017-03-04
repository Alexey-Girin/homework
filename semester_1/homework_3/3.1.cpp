#include <iostream> 

using namespace std;

int partition(int mas[], int low, int high)
{
	int pivot = mas[low];
	int rise = low - 1;
	int down = high + 1;
	for (;;)
	{
		do
		{
			rise++;
		} while (mas[rise] < pivot);
		do
		{
			down--;
		} while (mas[down] > pivot);
		if (rise >= down)
		{
			return down;
		}
		swap(mas[rise], mas[down]);
	}
}

void insertionSort(int mas[], int low, int high)
{
	for (int i = low + 1; i <= high; i++)
	{
		int currentElement = 0;
		if (mas[i] < mas[i - 1])
		{
			int t = i - 1;
			currentElement = mas[i];
			while (currentElement < mas[t])
			{
				mas[t + 1] = mas[t];
				t--;
			}
			mas[t + 1] = currentElement;
		}
	}
}

void quicksort(int mas[], int low, int high)
{
	if (low < high)
	{
		int resultresultOfPartition = partition(mas, low, high);
		if (resultresultOfPartition > low)
		{
			if (resultresultOfPartition - low < 9)
			{
				insertionSort(mas, low, resultresultOfPartition);
			}
			else
			{
				quicksort(mas, low, resultresultOfPartition);
			}
		}
		if (resultresultOfPartition + 1 < high)
		{
			if (high - resultresultOfPartition < 10)
			{
				insertionSort(mas, resultresultOfPartition + 1, high);
			}
			else
			{
				quicksort(mas, resultresultOfPartition + 1, high);
			}
		}
	}
}

bool forTest(int masTest[], int masRes[], int num)
{
	for (int i = 0; i < num; i++)
	{
		if (masTest[i] != masRes[i])
		{
			return false;
		}
	}
	return true;
}

bool test1()
{
	const int num = 3;
	int masTest[num] = { 1, 1, 1 };
	int masRes[num] = { 1, 1, 1 };
	quicksort(masTest, 0, num - 1);
	return forTest(masTest, masRes, num);
}

bool test2()
{
	const int num = 9;
	int masTest[num] = { 8, 7, 6, 5, 4, 3, 2, 1, 0 };
	int masRes[num] = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
	quicksort(masTest, 0, num - 1);
	return forTest(masTest, masRes, num);
}

bool test3()
{
	const int num = 20;
	int masTest[num] = { -6, 2, 1, 4, 6, 5, 4, 2, -1, 1, 3, 8, -2, -4, 0, 0, 9, 8, 1, 3 };
	int masRes[num] = { -6, -4, -2, -1, 0, 0, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6, 8, 8, 9 };
	quicksort(masTest, 0, num - 1);
	return forTest(masTest, masRes, num);
}

void testing()
{
	cout << "testing: ";
	if (test1() && test2() && test3())
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}
}

int main()
{
	testing();
	setlocale(LC_ALL, "Russian");
	cout << "忖邃栩� 觐�-忸 屐屙蝾� 爨耨桠�: ";
	int num = 0;
	cin >> num;
	int *mas = new int[num];
	cout << "忖邃栩� 屐屙螓 爨耨桠�: ";
	for (int i = 0; i < num; i++)
	{
		cin >> mas[i];
	}
	const int low = 0;
	quicksort(mas, low, num - 1);
	cout << "疱珞朦蜞� 耦痱桊钼觇: \n";
	for (int i = 0; i < num; i++)
	{
		cout << mas[i] << " ";
	}
	cout << endl;
	delete[]  mas;
	return 0;
}