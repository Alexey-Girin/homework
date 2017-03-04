#include <iostream>
#include <string>

using namespace std;

int main()
{
	setlocale(LC_ALL, "Russian");
	string S1, S;
	cout << "строка S:" << endl; getline(cin, S);
	cout << "строка S1:" << endl; getline(cin, S1);
	const int SizeS1 = S1.size();
	int Res = 0;
	for (int i = 0; i <= S.size() - SizeS1;i++)
	{
		if (S1 == S.substr(i, SizeS1)) Res++;
	}
	cout << "кол-во вхождений: " << Res << endl;
	return 0;
}