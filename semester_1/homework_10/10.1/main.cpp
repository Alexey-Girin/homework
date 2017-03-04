#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include "list.h"

using namespace std;

struct Vertex
{
	int vertex;
	int edge;
	bool isDelete;
};

bool readInputFile(vector<List*> &adjacencyList, vector<int> &capital, const string &fileName)
{
	ifstream input(fileName);
	if (!input.is_open())
	{
		cout << "Ù‡ÈÎ ÌÂ Ì‡È‰ÂÌ" << endl;
		return false;
	}

	int numOfCities = 0;
	input >> numOfCities;
	int numOfRoads = 0;
	input >> numOfRoads;

	for (int i = 0; i < numOfCities; i++)
	{
		adjacencyList.push_back(createList());
	}

	for (int i = 0; i < numOfRoads; i++)
	{
		int firstCity = 0;
		input >> firstCity;
		int secondCity = 0;
		input >> secondCity;
		int road = 0;
		input >> road;

		add(adjacencyList[firstCity], { secondCity, road, false });
		add(adjacencyList[secondCity], { firstCity, road, false });
	}

	int numOfCapitals = 0;
	input >> numOfCapitals;

	for (int i = 0; i < numOfCapitals; i++)
	{
		int capitalNum = 0;
		input >> capitalNum;
		capital.push_back(capitalNum);
	}

	input.close();
	return true;
}

int addNearestCityToCountry(vector<int> &country, vector<List*> &adjacencyList)
{
	Vertex nearestCityToCountry{ -1, -1 };
	
	const int countrySize = country.size();
	for (int i = 0; i < countrySize; i++)
	{
		Vertex nearestCityToCity = read(adjacencyList[country[i]]);
		if (nearestCityToCity.edge != -1 && 
			(nearestCityToCountry.edge == -1 || nearestCityToCity.edge < nearestCityToCountry.edge))

		{
			nearestCityToCountry = nearestCityToCity;
		}
	}
	return nearestCityToCountry.vertex;
}

void distributeCities(vector<List*> &adjacencyList, vector<vector<int>*> &countries, vector<int> &capital)
{
	const int numOfCapitals = capital.size();
	for (int i = 0; i < numOfCapitals; i++)
	{
		countries[i] = new vector<int>(1, capital[i]);
		blockRoadToCity(adjacencyList, capital[i]);
	}

	int unusedCities = adjacencyList.size() - numOfCapitals;
	while (unusedCities != 0)
	{
		for (int i = 0; unusedCities != 0 && i < numOfCapitals; i++)
		{
			int addedCity = addNearestCityToCountry(*countries[i], adjacencyList);
			if (addedCity != -1)
			{
				(*countries[i]).push_back(addedCity);
				blockRoadToCity(adjacencyList, addedCity);
				unusedCities--;
			}
		}
	}
}

void print(vector<vector<int>*> &countries)
{	
	const int countriesSize = countries.size();
	for (int i = 0; i < countriesSize; i++)
	{
		cout << "√ÓÒÛ‰‡ÒÚ‚Ó: " << i << endl;
		cout << "\t„ÓÓ‰‡: ";

		vector<int> country = *countries[i];
		const int countrySize = country.size();

		for (int t = 0; t < countrySize; t++)
		{
			cout << country[t] << " ";
		}
		cout << endl;
	}
}

void removeCountries(vector<vector<int>*> &countries)
{
	const int vectorSize = countries.size();
	for (int i = 0; i < vectorSize; i++)
	{
		(*countries[i]).clear();
		countries[i] = nullptr;
	}
}

void removeAdjacencyLists(vector<List*> &adjacencyList)
{
	const int listSize = adjacencyList.size();
	for (int i = 0; i < listSize; i++)
	{
		removeList(adjacencyList[i]);
	}
}

void testPrint(vector<vector<int>*> &countries)
{
	ofstream output("result.txt");
	const int countriesSize = countries.size();
	for (int i = 0; i < countriesSize; i++)
	{
		vector<int> country = *countries[i];
		const int countrySize = country.size();

		for (int t = 0; t < countrySize; t++)
		{
			output << country[t] << " ";
		}
		output << endl;
	}
	output.close();
}

bool file—omparison()
{
	ifstream inputResult("result.txt");
	ifstream inputTrueResult("trueResultTest.txt");
	string testResult = "";
	string trueResult = "";

	while (inputResult.good() && inputTrueResult.good())
	{
		getline(inputResult, testResult);
		getline(inputTrueResult, trueResult);
		if (testResult != trueResult)
		{
			return false;
		}
	}
	if (inputResult.good() || inputTrueResult.good())
	{
		return false;
	}

	inputResult.close();
	inputTrueResult.close();
	return true;
}

void test()
{
	vector<List*> adjacencyList(0);
	vector<int> capital(0);

	const string fileName = "test.txt";
	if (!readInputFile(adjacencyList, capital, fileName))
	{
		cout << "testing false" << endl;
		return;
	}

	vector<vector<int>*> countries(capital.size(), nullptr);
	distributeCities(adjacencyList, countries, capital);

	testPrint(countries);
	removeCountries(countries);
	removeAdjacencyLists(adjacencyList);

	if (file—omparison())
	{
		cout << "tetsing true" << endl;
		return;
	}
	cout << "testing false" << endl;

}

int main()
{
	test();
	setlocale(LC_ALL, "Russian");

	vector<List*> adjacencyList(0);
	vector<int> capital(0);

	const string fileName = "input.txt";
	if (!readInputFile( adjacencyList, capital, fileName))
	{
		return 0;
	}

	vector<vector<int>*> countries(capital.size(), nullptr);
	distributeCities(adjacencyList, countries, capital);

	print(countries);
	removeCountries(countries);
	removeAdjacencyLists(adjacencyList);
	return 0;
}