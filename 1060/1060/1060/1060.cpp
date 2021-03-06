// 1060.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <queue>


struct Step
{
	int field;
	int deep;
};


int getVal(int input, int i, int j) {
	return (input >> i * 4 + j) & 1;
}

int setVal(int input, int val, int i, int j) {
	int mask = 1;
	mask = mask << (i * 4 + j);
	mask = ~mask;
	return (input & mask) + (val << (i * 4 + j));
}


int main()
{

	int field =0;
	char line[10];

	for (int i = 0; i < 4; i++) {
		std::cin >> line;
		for (int j = 0; j < 4; j++) {
			setVal(field, line[j] == 'w', i, j);

		}
	}
	
	std::queue<Step> q;
	Step s;
	s.deep = 0;
	s.field = field;
	q.push(s);

	while (q.size())
	{
		Step cs = q.front();
		q.pop();

		if (cs.deep >= 5) {
			std::cout << "Impossible";
			return 0;
		}

		int b = 1;
		int w = 1;
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (getVal(cs.field, i, j) == 0) {
					w = 0;
				}
				else {
					b = 0;
				}
			}
		}
		if (b || w) {
			std::cout << cs.deep;
			return 0;
		}

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				
				int newfield = cs.field;
				for (int k = 0; k <= 4; k++) {
					int di = 0;
					int dj = 0;
					switch (k) {
						case 0: di = -1; break;
						case 1: dj = 1; break;
						case 2: di = 1; break;
						case 3: dj = -1; break;
						default: break;
					}

					if (i + di >= 0 && i + di < 4 && j + dj >= 0 && j + dj < 4) {
						newfield = setVal(newfield, !getVal( field, i + di, j + dj), i + di, j + dj);
					}
				}

				Step ns;
				ns.deep = cs.deep + 1;
				ns.field = newfield;
				q.push(ns);

			}
		}

	}
	
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
