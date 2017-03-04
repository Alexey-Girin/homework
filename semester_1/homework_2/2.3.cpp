#include <cstdlib> 
#include <iostream>

using namespace std;

void count(int *mas, int numCount)
{
	const int maxNum = 101;
	int masCountingSort[maxNum] = {};
	for (int i = 0; i < numCount; i++)
	{
		masCountingSort[mas[i]]++;
	}
	int index = 0;
	for (int i = 0; i < maxNum; i++)
	{
		for (int k = 0; k < masCountingSort[i]; k++)
		{
			mas[index] = i;
			index++;
		}
	}
	cout << "疱珞朦蜞� 耦痱桊钼觇: \n";
	for (int i = 0; i < numCount; i++)
	{
		cout << mas[i] << " ";
	}
	cout << endl;
}

void bubble(int *masBubble, int numBubble)
{
	bool conditionBubble = true;
	for (int i = numBubble - 1; i > 1 && conditionBubble; i--)
	{
		conditionBubble = false;
		for (int t = 0; t < i; t++)
		{
			if (masBubble[t] > masBubble[t + 1])
			{
				swap(masBubble[t], masBubble[t + 1]);
				conditionBubble = true;
			}
		}
	}
	cout << "疱珞朦蜞� 耦痱桊钼觇: \n";
	for (int i = 0; i < numBubble; i++)
	{
		cout << masBubble[i] << " ";
	}
	cout << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	cout << "忖邃栩� 0 潆 耦痱桊钼觇 镱漶麇蝾� 桦� 1 潆 耦痱桊钼觇 矬琨瘘觐�: ";
	bool start = false;
	cin >> start;
	if (start)
	{
		cout << "C铕蜩痤怅� 矬琨瘘觐� \n忖邃栩� 觐�-忸 屐屙蝾� 爨耨桠� : ";
		int numBubble = 0;
		cin >> numBubble;
		int *masBubble = new int[numBubble];
		cout << "忖邃栩� 屐屙螓 爨耨桠�: \n";
		for (int i = 0; i < numBubble; i++)
		{
			cin >> masBubble[i];
		}
		bubble(masBubble, numBubble);
		delete[] masBubble;
	}
	else
	{
		cout << "C铕蜩痤怅� 镱漶麇蝾� \n忖邃栩� 觐�-忸 屐屙蝾� 爨耨桠� : ";
		int numCount = 0;
		cin >> numCount;
		int *mas = new int[numCount];
		cout << "忖邃栩� 屐屙螓 爨耨桠� 桤 [0;100]: \n";
		for (int i = 0; i < numCount; i++)
		{
			cin >> mas[i];
		}
		count(mas, numCount);
		delete[] mas;
	}
	return 0;
}