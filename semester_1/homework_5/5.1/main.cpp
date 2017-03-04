#include <iostream>
#include <limits>
#include <string>

using namespace std;

struct Element
{
	string value;
	Element *previous;
	Element *next;
};

void addValue(Element *&head)
{
	cout << "введите запись: ";
	string value = "";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
	getline(cin, value);
	if (head == nullptr)
	{
		Element *newElement = new Element{ value, nullptr, nullptr };
		head = newElement;
		return;
	}
	if (value < head->value)
	{
		Element *newElement = new Element{ value, nullptr, head };
		head = newElement;
		newElement->next->previous = newElement;
		return;
	}
	Element *position = head;
	while (true)
	{
		if (value < position->value)
		{
			Element *newElement = new Element{ value, position->previous, position };
			position->previous->next = newElement;
			position->previous = newElement;
			return;
		}
		if (position->next == nullptr)
		{
			Element *newElement = new Element{ value, position, nullptr };
			position->next = newElement;
			return;
		}
		position = position->next;
	}
}

void print(Element *position)
{
	if (position == nullptr)
	{
		cout << "записи отсутствуют" << endl;
		return;
	}
	cout << "список записей:" << endl;
	while (position != nullptr)
	{
		cout << position->value << endl;
		position = position->next;
	}
}

bool deleteValue(Element *&head)
{
	Element *position = head;
	cout << "введите запись дл€ удалени€: ";
	string value = "";
	cin.ignore(numeric_limits<streamsize>::max(), '\n');
	getline(cin, value);
	while (position != nullptr)
	{
		if (position->value == value)
		{
			if (position->next == nullptr && position->previous == nullptr)
			{
				delete position;
				head = nullptr;
				return true;
			}
			if (position->next == nullptr)
			{
				position->previous->next = nullptr;
				delete position;
				return true;
			}
			if (position->previous == nullptr)
			{
				head = position->next;
				position->next->previous = nullptr;
				delete position;
				return true;
			}
			position->next->previous = position->previous;
			position->previous->next = position->next;
			delete position;
			return true;
		}
		position = position->next;
	}
	return false;
}

void deleteAll(Element *&head)
{
	Element *position = nullptr;
	while (head->next != nullptr)
	{
		head->next->previous = nullptr;
		position = head;
		head = head->next;
		delete position;
	}
	if (head->next == nullptr)
	{
		delete head;
	}
}

bool instruction(Element *&head)
{
	cout << "введите команду: ";
	int input = 0;
	cin >> input;
	switch (input)
	{
	case 0:
	{
		if (head != nullptr)
		{
			deleteAll(head);
		}
		return false;
	}
	case 1:
	{
		addValue(head);
		cout << "запись добавлена" << endl;
		break;
	}
	case 2:
	{
		if (deleteValue(head))
		{
			cout << "запись удалена" << endl;
			break;
		}
		cout << "ошибка! запись отсутствует" << endl;
		break;
	}
	case 3:
	{
		print(head);
		break;
	}
	default:
	{
		cout << "ошибка ввода" << endl;
		break;
	}
	}
	cout << "__________" << endl;
	return true;
}

void reference()
{
	cout << "значени€ в списке типа string" << endl;
	cout << "0 Ц выйти" << endl;
	cout << "1 Ц добавить значение в сортированный список" << endl;
	cout << "2 Ц удалить значение из списка" << endl;
	cout << "3 Ц распечатать список" << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	reference();
	Element *head = nullptr;
	while (instruction(head))
	{

	}
	head = nullptr;
	return 0;
}