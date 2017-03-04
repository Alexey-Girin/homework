#include "sort.h"
#include <vector>

using namespace std;

int partition(vector<int> &mas, int low, int high)
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

void insertionSort(vector<int> &mas, int low, int high)
{
	for (int i = low + 1; i <= high; i++)
	{
		int currentElement = 0;
		if (mas[i] < mas[i - 1] && i >= 1)
		{
			int t = i - 1;
			currentElement = mas[i];
			while (t >= 0 && currentElement < mas[t])
			{
				mas[t + 1] = mas[t];
				t--;
			}
			mas[t + 1] = currentElement;
		}
	}
}

void quicksort(vector<int> &mas, int low, int high)
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