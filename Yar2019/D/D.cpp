// D.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <math.h>

int main()
{
	int a, b;
	scanf_s("%d %d", &a, &b);
	long double e = 0.00000000001, k;
	long double x = 1;
	k = (long double)b / log((long double)a);

	while ( x < 100000 ) {
		long double lnx = log(x)*k;
		if ( fabs(lnx - x ) < e ) {
			int X = x;
			printf_s("%d", X);
			return 0;
		}
		
		x++;
	}
	printf_s("0");
	return 0;
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
