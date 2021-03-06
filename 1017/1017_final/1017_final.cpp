// 1017.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include "pch.h"
#include <iostream>

long long arr[500][500];

long long req(int ls, int k) {
	if (arr[ls][k] != -1) {
		return arr[ls][k];
	}

	if (k == 0) {
		arr[ls][k] = 1;
		return 1;
	}

	if (k <= ls) {
		arr[ls][k] = 0;
		return 0;
	}

	int i = ls + 1;
	long long  res = 0;
	while (i < k || i == k && ls > 0)
	{
		res += req(i, k - i);
		i++;
	}

	arr[ls][k] = res;
	return res;
}



void zero() {
	int i = 0;
	int j = 0;
	while (i < 500)
	{
		j = 0;
		while (j < 500)
		{
			arr[i][j] = -1;
			j++;
		}
		i++;
	}
}

int main()
{

	zero();
	int K;
	std::cin >> K;
	std::cout << req(0, K);

}



// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
