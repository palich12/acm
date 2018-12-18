

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

	for (int &item : path) {
		res *= 500;
		res += item;
	}

	return res;
}

int req(vector<int> path) {

	int path_size = path.size();
	
	if (path_size <= 3)
	{
		return 0;
	}

	long long path_hash = GetHash(path);
	auto search = cash.find(path_hash);
	if (search != cash.end())
	{
		return search->second;
	}

	int res = INT_MAX;
	
	for (int i = 0; i < path_size; i++)
	{

		int index = -1;
		for (int j1 = i + 1; j1 < path_size + 2; j1++)
		{
			int j = j1;
			if (j >= path_size) {
				j -= path_size;
			}
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

	cash[path_hash] = res;
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