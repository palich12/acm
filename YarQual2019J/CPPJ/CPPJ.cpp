
#include "pch.h"
#include <iostream>
#include <stdio.h> 

int main()
{

	long long N = 0;
	long long M = 0;
	long long k = 0;

	scanf_s("%lld %lld %lld", &N, &M, &k);

	long long p = 0;
	long long i = 0;

	while (k > 0)
	{
		long v = i % 2 == 0 ? 1 : -1;
		long d = M - i - (i == 0 ? 1 : 0);
		if (d <= 0)
		{
			break;
		}

		if (d <= k)
		{
			p += d * v;
			k -= d;
		}
		else
		{
			p += k * v;
			break;
		}

		d = N - i - 1;
		if (d <= 0)
		{
			break;
		}

		if (d <= k)
		{
			p += d * v;
			k -= d;
		}
		else
		{
			p += k * v;
			break;
		}

		i++;
	}

	printf_s( "%lld", p % 7 + 1);
	scanf("%d");


}
