#include <iostream> 

using namespace std;

int partition(int mas[], int low, int high)
{
	int pivot = mas[low];
	int rise = low - 1;
	int down = high + 1;
	while (true)
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

int mostÑommonElement(int mas[], int num)
{
	int max = 1;
	int currentNum = 1;
	int res = mas[0];
	for (int i = 1; i < num; i++)
	{
		if (mas[i] == mas[i - 1])
		{
			currentNum++;
		}
		else
		{
			if (currentNum > max)
			{
				max = currentNum;
				res = mas[i - 1];
			}
			currentNum = 1;
		}
	}
	if (currentNum > max)
	{
		max = currentNum;
		res = mas[num - 1];
	}
	return res;
}

bool test1()
{
	const int num = 5;
	int masTest[num] = { 1, 2, 3, 4, 4 };
	const int low = 0;
	quicksort(masTest, low, num - 1);
	const int resTest = 4;
	if (resTest == mostÑommonElement(masTest, num))
	{
		return true;
	}
	return false;
}

bool test2()
{
	const int num = 8;
	int masTest[num] = { 0, 0, 1, 1, 2, 2, 3, 3 };
	const int low = 0;
	quicksort(masTest, low, num - 1);
	const int resTest = 0;
	if (resTest == mostÑommonElement(masTest, num))
	{
		return true;
	}
	return false;
}

bool test3()
{
	const int num = 12;
	int masTest[num] = { 1, 9, 9, 2, 1, 2, 1, 9, 9, 2, 1, 9 };
	const int low = 0;
	quicksort(masTest, low, num - 1);
	const int resTest = 9;
	if (resTest == mostÑommonElement(masTest, num))
	{
		return true;
	}
	return false;
}

void testing()
{
	cout << "test1: " << bool(test1()) << endl;
	cout << "test2: " << bool(test2()) << endl;
	cout << "test3: " << bool(test3()) << endl;
}

int main()
{
	testing();
	setlocale(LC_ALL, "Russian");
	cout << "åñëè â ìàññèâå íåñêîëüêî ýëåìåíòîâ, âñòðå÷àþùèõñÿ íàèáîëåå ÷àñòî, âûâîäèòñÿ ìèíèìàëüíûé èç íèõ\n";
	cout << "ââåäèòå êîë-âî ýëåìåíòîâ ìàññèâà: ";
	int num = 0;
	cin >> num;
	int *mas = new int[num];
	cout << "ââåäèòå ýëåìåíòû ìàññèâà: \n";
	for (int i = 0; i < num; i++)
	{
		cin >> mas[i];
	}
	const int low = 0;
	quicksort(mas, low, num - 1);
	int res = mostÑommonElement(mas, num);
	cout << "íàèáîëåå ÷àñòî âñòðå÷àþùèéñÿ ýëåìåíò â ìàññèâå: " << res << endl;
	delete[]  mas;
	return 0;
}