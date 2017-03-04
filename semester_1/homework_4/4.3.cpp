#include <iostream>
#include <fstream>
#include <string>
#include <vector>

using namespace std;

struct Record
{
	string name;
	string phone;
};

void addEntry(vector<Record> &vecRecord, int &index)
{
	const int maxRecord = 100;
	if (index >= maxRecord)
	{
		cout << "недостаточно места\n";
		return;
	}
	Record newRecord;
	vecRecord.push_back(newRecord);
	string newName = "";
	cout << "введите имя: ";
	cin >> newName;
	int i = 0;
	for (i = 0; i < index && newName != vecRecord[i].name; i++)
	{

	}
	if (i == index)
	{
		string newPhone = "";
		cout << "введите телефон: ";
		cin >> newPhone;
		int t = 0;
		for (t = 0; t < index && newPhone != vecRecord[t].phone; t++)
		{

		}
		if (t == index)
		{
			vecRecord[index].name = newName;
			vecRecord[index].phone = newPhone;
			cout << "данные будут удалены при выходе из программы\nдля сохранения введите: 5\n";
			index++;
		}
		else
		{
			cout << "ошибка. введенный телефон уже существует\n";
		}
	}
	else
	{
		cout << "ошибка. введенное имя уже занято\n";
	}
}

void printAll(vector<Record> &vecRecord, int index)
{
	if (index == 0)
	{
		cout << "записи отсутствуют\n";
		return;
	}
	cout << "имя\t\tтелефон\n";
	for (int i = 0; i < index; i++)
	{
		cout << vecRecord[i].name << "\t\t" << vecRecord[i].phone << endl;
	}
}

void findPhone(vector<Record> &vecRecord, int index)
{
	cout << "введите имя: ";
	string nameInput = "";
	cin >> nameInput;
	ifstream fileIn("directory.txt");
	int i = 0;
	while (i < index && nameInput != vecRecord[i].name)
	{
		i++;
	}
	if (i == index)
	{
		cout << "телефон не найден\n";
		return;
	}
	cout << "телефон " << vecRecord[i].name << ": " << vecRecord[i].phone << endl;
	fileIn.close();
}

void findName(vector<Record> &vecRecord, int index)
{
	cout << "введите номер телефона: ";
	string phoneInput = "";
	cin >> phoneInput;
	ifstream fileIn("directory.txt");
	int i = 0;
	while (i < index && phoneInput != vecRecord[i].phone)
	{
		i++;
	}
	if (i == index)
	{
		cout << "имя не найдено\n";
		return;
	}
	cout << "владелец " << vecRecord[i].phone << ": " << vecRecord[i].name << endl;
	fileIn.close();
}

void save(vector<Record> &vecRecord, int index)
{
	remove("directory.txt");
	ofstream output;
	output.open("directory.txt");
	for (int i = 0; i < index; i++)
	{
		output << vecRecord[i].name << " " << vecRecord[i].phone << endl;
	}
	output.close();
	cout << "данные сохранены в справочник\n";
}

void insertionSortName(vector<Record> &vecRecord, int low, int index)
{
	for (int i = low; i < index; i++)
	{
		int j = i - 1;
		string elementName = vecRecord[i].name;
		string elementPhone = vecRecord[i].phone;
		while (j >= 0 && elementName < vecRecord[j].name)
		{
			vecRecord[j + 1] = vecRecord[j];
			j--;
		}
		vecRecord[j + 1].name = elementName;
		vecRecord[j + 1].phone = elementPhone;
	}
}

bool instruction(vector<Record> &vecRecord, int &index)
{
	cout << "\nвведите команду: ";
	int input = 0;
	cin >> input;
	switch (input)
	{
	case 0:
	{
		return false;
	}
	case 1:
	{
		addEntry(vecRecord, index);
		insertionSortName(vecRecord, index - 1, index);
		break;
	}
	case 2:
	{
		printAll(vecRecord, index);
		break;
	}
	case 3:
	{
		findPhone(vecRecord, index);
		break;
	}
	case 4:
	{
		findName(vecRecord, index);
		break;
	}
	case 5:
	{
		save(vecRecord, index);
		break;
	}
	default:
	{
		cout << "ошибка ввода/n";
		break;
	}
	}
	return true;
}

void reference()
{
	cout << "Телефонный справочник" << endl;
	cout << "Записи вида (name\tnumber)\n";
	cout << "0 - выйти" << endl;
	cout << "1 - добавить запись (имя и телефон)" << endl;
	cout << "2 - распечатать все имеющиеся записи" << endl;
	cout << "3 - найти телефон по имени" << endl;
	cout << "4 - найти имя по телефону" << endl;
	cout << "5 - сохранить текущие данные в файл " << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	reference();
	ifstream fileIn("directory.txt");
	if (!fileIn.is_open())
	{
		ofstream fileOut("directory.txt");
		fileOut.close();
	}
	const int maxRecord = 100;
	vector<Record> vecRecord;
	int index = 0;
	Record newRecord;
	while (index < maxRecord && fileIn >> newRecord.name && fileIn >> newRecord.phone)
	{
		vecRecord.push_back(newRecord);
		index++;
	}
	fileIn.close();
	insertionSortName(vecRecord, 0, index);
	while (instruction(vecRecord, index))
	{

	}
	return 0;
}