#include <iostream>
#include "commentsSearch.h"

using namespace std;

int main()
{
	FILE *ptrfile = fopen("input.txt", "r");
	findComments(ptrfile);

	fclose(ptrfile);
	return 0;
}