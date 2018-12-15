

#include "pch.h"

#include <stdio.h>
#include <iostream>
#include <vector>
#include <algorithm>
#include <map>
#include <iterator>

using namespace std;


int Matrix[500][500];
int N;
map<long long, int> cash;

long long GetHash(vector<int> path)
{
	long long res = 1;
	for (int i = 0; i < path.size(); i++)
	{
		//res *= i;
		res *= 500;
		res += path[i];
	}
	return res;
}

bool Contains(vector<int> v, int x) {
	return find(v.begin(), v.end(), x) != v.end();
}

int req(vector<int> path) {

	if (path.size() <= 3)
	{
		return 0;
	}

	long long pathhash = GetHash(path);
	if (cash.find(pathhash) != cash.end())
	{
		return cash[pathhash];
	}

	int res = INT_MAX;
	for (int i = 0; i < path.size(); i++)
	{

		int index = -1;
		for (int j1 = i + 1; j1 < path.size() + 2; j1++)
		{
			int j = j1 % path.size();
			if (index == -1)
			{
				index = j;
			}
			else
			{
				vector<int> newpath(path);
				newpath.erase(newpath.begin() + index);

				int r = req(newpath);
				if (r < INT_MAX) {
					r += Matrix[path[i]][path[j]];
				}
				if (res > r) {
					res = r;
				}
				break;
			}
		}
	}

	cash[pathhash] = res;
	return res;
}

int main()
{
	scanf("%d", &N);

	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
		{
			int v;
			scanf("%d", &v);
			Matrix[i][j] = v;
		}
	}

	//req(new List<int>());
	vector<int> v;
	for (int i = 0; i < N; i++) {
		v.push_back(i);
	}
	int res = req(v);
	printf("%d", res);

	return 0;
}