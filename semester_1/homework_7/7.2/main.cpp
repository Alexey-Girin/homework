#include <iostream>

using namespace std;

struct Node
{
	char sign;
	int number;
	Node *leftChild;
	Node *rightChild;
};

void createTree(FILE *&ptrFile, Node *&currentPosition);

Node *createChild(FILE *&ptrFile)
{
	int getSymbol = getc(ptrFile);
	ungetc(getSymbol, ptrFile);

	if (getSymbol == '(')
	{
		Node *child = nullptr;
		createTree(ptrFile, child);
		return child;
	}

	int num = 0;
	fscanf(ptrFile, "%d", &num);
	return new Node{ 0, num, nullptr, nullptr };
}

void createTree(FILE *&ptrFile, Node *&currentPosition)
{
	fseek(ptrFile, 1, SEEK_CUR);

	char sign = 0;
	fscanf(ptrFile, "%c", &sign);
	fseek(ptrFile, 1, SEEK_CUR);
	
	Node *leftChild = createChild(ptrFile);
	fseek(ptrFile, 1, SEEK_CUR);

	Node *rightChild = createChild(ptrFile);
	fseek(ptrFile, 1, SEEK_CUR);

	currentPosition = new Node{ sign, 0, leftChild, rightChild };
}

double calculation(const char sign, const double firstNum, const double secondNum)
{
	switch (sign)
	{
		case('+'):
			return firstNum + secondNum;
		case('-'):
			return firstNum - secondNum;
		case('*'):
			return firstNum * secondNum;
		case('/'):
			return firstNum / secondNum;
	}
}

double countResult(Node *&root)
{
	if (root->sign != 0)
	{
		double firstNum = countResult(root->leftChild);
		double secondNum = countResult(root->rightChild);
		return calculation(root->sign, firstNum, secondNum);
	}
	return 1.0 * (root->number);
}

void printNode(Node *&root);

void printTree(Node *&root)
{
	if (root->sign != 0)
	{
		cout << "( ";
	}

	printNode(root);

	if (root->sign != 0)
	{
		cout << ") ";
	}
}

void printRoot(Node *&root)
{
	if (root->sign == 0)
	{
		cout << root->number << " ";
		return;
	}
	cout << root->sign << " ";
}

void printNode(Node *&root)
{
	printRoot(root);

	if (root->leftChild != nullptr)
	{
		printTree(root->leftChild);
	}
	if (root->rightChild != nullptr)
	{
		printTree(root->rightChild);
	}
}

void removeTree(Node *&root)
{
	if (root->leftChild != nullptr)
	{
		removeTree(root->leftChild);
	}
	if (root->rightChild != nullptr)
	{
		removeTree(root->rightChild);
	}

	delete root;
}

void test()
{
	FILE *testFile = fopen("test.txt", "r");
	Node *root = nullptr;
	createTree(testFile, root);
	fclose(testFile);

	const double trueResult = -4.5;
	if (countResult(root) == trueResult)
	{
		cout << "testing true" << endl;
		removeTree(root);
		return;
	}
	cout << "testing false" << endl;
	removeTree(root);
}

int main()
{
	test();
	setlocale(LC_ALL, "Russian");

	FILE *ptrFile = fopen("input.txt", "r");
	Node *root = nullptr;
	createTree(ptrFile, root);
	fclose(ptrFile);

	cout << "дерево: " << endl;
	printTree(root);
	cout << endl << "результат: " << countResult(root) << endl;

	removeTree(root);
	root = nullptr;
	return 0;
}