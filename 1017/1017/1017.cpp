// 1017.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <ctime>

long long arr[500][500];
int K;

long long req( int ls, int k) {
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
		res += req( i, k - i);
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

void print() {
	std::cout << "\n--------------------------\n";
	int i, j;
	i = 0;
	while (i < 500)
	{
		j = 0;
		if (arr[i][0] == 0) {
			i++;
			continue;
		}
		std::cout << "\n";
		while (j < 500)
		{
			if (arr[i][j] == 0 && j > 0) {
				break;
			}
			std::cout << arr[i][j];
			std::cout << "\t";
			j++;
		}
		i++;
	}
}

int main()
{
	
	zero();
	
	const clock_t begin_time = clock();


    std::cout << "1017\n--------------------------\n\n\n"; 

	K = 5;
	long long q = 0;
	while (K <= 500)
	{
		const clock_t begin_k_time = clock();
		q = req( 0, K);

		std::cout << K;
		std::cout << " >> ";
		std::cout << q;
		std::cout << " time: ";
		std::cout << float(clock() - begin_k_time) / CLOCKS_PER_SEC;
		std::cout << "\n";
		K++;
	}

	std::cout << "\n--------------------------\nfull time: ";
	std::cout << float(clock() - begin_time) / CLOCKS_PER_SEC;
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
