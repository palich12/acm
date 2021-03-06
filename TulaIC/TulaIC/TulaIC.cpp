

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


class Vertex
{
public:
	Vertex* Previos;
	Vertex* Next;
	bool OldFirst;
	int Number;
	int Value;
	Vertex(int number) {
		this->OldFirst = false;
		this->Number = number;
	}

private:

};

struct VertexValue
{
	Vertex* CurientVertex;
	int Value;
};

class CycleList
{
public:
	int Count;
	Vertex* FirstVertex;
	CycleList(Vertex* fv) {
		this->Count = 1;		
		fv->Next = fv->Previos = fv;
		this->FirstVertex = fv;
	}

	void Add(Vertex* curV) {
		this->Count++;
		this->FirstVertex->Previos->Next = curV;
		curV->Previos = this->FirstVertex->Previos;
		curV->Next = this->FirstVertex;
		this->FirstVertex->Previos = curV;
	}

	void Remove(Vertex* curV) {
		if (curV == this->FirstVertex) {
			this->FirstVertex = curV->Next;
			curV->OldFirst = true;
		}

		this->Count--;
		curV->Next->Previos = curV->Previos;
		curV->Previos->Next = curV->Next;
	}

	void Recover(Vertex* curV) {
		if (curV->OldFirst) {
			this->FirstVertex = curV;
			curV->OldFirst = false;
		}

		this->Count++;
		curV->Next->Previos = curV;
		curV->Previos->Next = curV;
	}

	long long GetHash()
	{
		long long res = 1;

		Vertex* curV = this->FirstVertex;
		do {
			res *= 500;
			res += curV->Number;
			curV = curV->Next;
		} while (curV != this->FirstVertex);
		

		return res;
	}

	vector<VertexValue> toValueVector() {
		vector<VertexValue> res;

		Vertex* curV = this->FirstVertex;
		do {
			VertexValue item;
			item.CurientVertex = curV;
			item.Value = Matrix[curV->Previos->Number][curV->Next->Number];
			res.push_back(item);
			curV = curV->Next;
		} while (curV != this->FirstVertex);
		return res;
	}

private:

};

bool VertexValueCompare(VertexValue i, VertexValue j) {
	return (i.Value < j.Value); 
}

int req(CycleList* path) {

	int path_size = path->Count;
	
	if (path_size <= 3)
	{
		return 0;
	}

	long long path_hash = path->GetHash();
	auto search = cash.find(path_hash);
	if (search != cash.end())
	{
		return search->second;
	}

	auto vert_vals = path->toValueVector();
	sort(vert_vals.begin(), vert_vals.end(), VertexValueCompare);
	
	int res = INT_MAX;
	for(auto item: vert_vals)
	{
		if (res <= item.Value) {
			break;
		}
		path->Remove(item.CurientVertex);
		int r = req(path);

		if (r < INT_MAX) {
			r += item.Value;
		}
		if (res > r) {
			res = r;
		}
		path->Recover(item.CurientVertex);
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

	CycleList* list = new CycleList(new Vertex(0));
	while (list->Count < N)
	{
		list->Add(new Vertex(list->Count));
	}

	int res = req(list);
	printf("%d", res);

	return 0;
}