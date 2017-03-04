#include <iostream>

using namespace std;

struct TreeElement
{
	int value;
	TreeElement *leftChild;
	TreeElement *rightChild;
};

bool add(TreeElement *&root, const int value)
{
	if (root == nullptr)
	{
		root = new TreeElement{ value, nullptr, nullptr};
		return true;
	}
	if (value == root->value)
	{
		return false;
	}
	if (value > root->value)
	{
		return add(root->leftChild, value);
	}
	if (value < root->value)
	{
		return add(root->rightChild, value);
	}
}

bool isBelong(TreeElement *&root, const int value)
{
	if (root == nullptr)
	{
		return false;
	}
	if (value == root->value)
	{
		return true;
	}
	if (value > root->value)
	{
		return isBelong(root->leftChild, value);
	}
	if (value < root->value)
	{
		return isBelong(root->rightChild, value);
	}
	return false;
}

void printAscending(TreeElement *&root)
{
	if (root == nullptr)
	{
		return;
	}
	if (root->leftChild != nullptr)
	{
		printAscending(root->leftChild);
	}
	cout << root->value << endl;
	if (root->rightChild != nullptr)
	{
		printAscending(root->rightChild);
	}
}

void printDescending(TreeElement *&root)
{
	if (root == nullptr)
	{
		return;
	}
	if (root->rightChild != nullptr)
	{
		printDescending(root->rightChild);
	}
	cout << root->value << endl;
	if (root->leftChild != nullptr)
	{
		printDescending(root->leftChild);
	}
}

TreeElement *findSuccessor(TreeElement *&root)
{
	TreeElement *element = root->leftChild;
	while (element->rightChild != nullptr)
	{
		element = element->rightChild;
	}
	return element;
}

bool removeElement(TreeElement *&root, const int &value);

void remove(TreeElement *&root)
{
	if (root->leftChild != nullptr && root->rightChild != nullptr)
	{
		TreeElement *successor = findSuccessor(root);
		root->value = successor->value;
		removeElement(root->leftChild, successor->value);
	}
	else if (root->leftChild != nullptr)
	{
		TreeElement *deleteElement = root;
		root = root->leftChild;
		delete deleteElement;
	}
	else if (root->rightChild != nullptr)
	{
		TreeElement *deleteElement = root;
		root = root->rightChild;
		delete deleteElement;
	}
	else
	{
		delete root;
		root = nullptr;
	}
}

bool removeElement(TreeElement *&root, const int &value)
{
	if (root == nullptr)
	{
		return false;
	}
	else if (value > root->value)
	{
		removeElement(root->leftChild, value);
	}
	else if (value < root->value)
	{
		removeElement(root->rightChild, value);
	}
	else
	{
		remove(root);
	}
	return true;
}

void removeTree(TreeElement *&root)
{
	if (root == nullptr)
	{
		return;
	}
	if (root->leftChild != nullptr)
	{
		removeTree(root->leftChild);
	}
	if (root->rightChild != nullptr)
	{
		removeTree(root->rightChild);
	}
	delete root;
	root = nullptr;
}

void reference()
{
	cout << "0 -выход" << endl;
	cout << "1 -добавить значение в множество" << endl;
	cout << "2 -проверить принадлежность значения множеству" << endl;
	cout << "3 -удалить значение из множества" << endl;
	cout << "4 -печать - возрастающий порядок" << endl;
	cout << "5 -печать - убывающий порядок" << endl;
	cout << "___________" << endl;
}

bool instruction(TreeElement *&root)
{
	cout << endl << "введите команду: ";
	int input = 0;
	cin >> input;
	switch (input)
	{
		case 0:
		{
			removeTree(root);
			return false;
		}
		case 1:
		{
			cout << "введите значение: ";
			int value = 0;
			cin >> value;
			if (add(root, value))
			{
				cout << "значение добавлено" << endl;
				return true;
			}
			cout << "значение уже принадлежит множеству" << endl;
			return true;
		}
		case 2:
		{
			cout << "введите значение: ";
			int value = 0;
			cin >> value;
			if (isBelong(root, value))
			{
				cout << "значение принадлежит множеству" << endl;
				return true;
			}
			cout << "значение не принадлежит множеству" << endl;
			return true;
		}
		case 3:
		{
			cout << "введите значение: ";
			int value = 0;
			cin >> value;
			if (removeElement(root, value))
			{
				cout << "значение удалено" << endl;
				return true;
			}
			cout << "ошибка. значение не принадлежит множеству" << endl;
			return true;
		}
		case 4:
		{
			printAscending(root);
			return true;
		}
		case 5:
		{
			printDescending(root);
			return true;
		}
		default:
		{
			cout << "ошибка ввода" << endl;
			return true;
		}
	}
}

int main()
{
	setlocale(LC_ALL, "Russian");
	reference();
	TreeElement *root = nullptr;
	while (instruction(root))
	{

	}
	return 0;
}