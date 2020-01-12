#include <iostream>
#include <vector>
#include <bitset>
#include <map>

#define MAX_K 81

struct action {
	int t;
	int g;
};

int sort[4][16] = {
	{0b0001, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0011, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0100, 0b0011, 0b0110, 0b0101, 0b0111, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0100, 0b1000, 0b0011, 0b0110, 0b1010, 0b0101, 0b1100, 0b1001, 0b1110, 0b1101, 0b1011, 0b0111, 0b1111, 0b0000}
};

int N = 0;
int K = 0;
int reqCount = 0;
int glasses[4] = { 0,0,0,0 };
std::bitset<MAX_K * 21 * 21 * 21 * 21> arr = { false };
std::bitset<1450> fastArr = { false };
std::bitset<1450> fastArr2 = { false };

void fastReq(int k) {
	if (fastArr[k]) {
		return;
	}
	
	fastArr[k] = true;

	for (int i = 0; i < N; i++) {
		int nextT = k + glasses[i];
		if (nextT <= K) {
			fastReq(nextT);
		}
	}
}

std::vector<action> getFastSteps(int k, int target) {
	std::vector<action> empty;
	if (fastArr2[k]) {
		return empty;
	}
	fastArr2[k] = true;
	
	for (int i = 0; i < N; i++) {

		action nextAction;
		nextAction.g = i;
		nextAction.t = k;
		int nextT = k + glasses[i];

		if (nextT == target) {
			std::vector<action> result;
			result.push_back(nextAction);
			return result;
		}


		if (nextT < target) {
			std::vector<action> result = getFastSteps(nextT, target);
			if (result.size() > 0) {
				action nextAction;
				nextAction.g = i;
				nextAction.t = k;
				result.push_back(nextAction);
				return result;
			}
		}
	}

	return empty;
}

std::vector<action> req(int k, std::vector<int> state) {

	std::vector<action> result;
	if (k < MAX_K) {
		int index = k;
		int m = MAX_K;
		for (int i = 0; i < N; i++) {
			index += state[i] * m;
			m *= 21;
		}
		
		if (arr[index]) {
			return result;
		}
		else {
			arr[index] = true;
		}
	}

	reqCount++;
	if (reqCount > 800000) {
		return result;
	}

	for (int i = 1; i < 1 << N; i++) {
		std::vector<int> newState;
		int tStep = 0;
		for (int j = 0; j < N; j++) {
			int newStateValue;
			if ((sort[N-1][i-1] & (1 << j)) > 0) {
				newStateValue = glasses[j] - state[j];
			}
			else {
				newStateValue = state[j];
			}
			newState.push_back(newStateValue);

			if (newStateValue > 0 && newStateValue < tStep || tStep == 0) {
				tStep = newStateValue;
			}
		}

		if (tStep == 0 || tStep > k) {
			continue;
		}

		action nextAction;
		nextAction.g = i;
		nextAction.t = k;
		if (tStep == k) {

			result.push_back(nextAction);
			return result;
		}

		for (int j = 0; j < N; j++) {
			if (newState[j] > 0) {
				newState[j] = newState[j] - tStep;
			}
		}

		std::vector<action> rr = req(k - tStep, newState);
		if (rr.size() > 0) {
			rr.push_back(nextAction);
			return rr;
		}
	}

	return result;
}

int main()
{
	std::vector<int> startState;

	scanf_s("%d", &N);
	for (int i = 0; i < N; i++) {
		int g;
		scanf_s("%d", &g);
		glasses[i] = g;
		startState.push_back(0);
	}
	scanf_s("%d", &K);
	
	fastReq(0);

	int maxDeep = 15;
	for (int i = 0; i < N; i++) {
		maxDeep += glasses[i];
	}
	

	for (int fastT = 0; fastT < maxDeep && fastT <= K; fastT++) {
		std::vector<action> result;
		std::vector<action> fast;
		if ( fastT == K || fastArr[K-fastT] ) {

			
			
			if (fastT > 0) {
				result = req(fastT, startState);
				if (result.size() == 0) {
					continue;
				}
			}

			fast = getFastSteps(0, K - fastT);

			printf_s("%d\n", fast.size() + result.size() + 1);
			
			for (int i = fast.size()-1; i >= 0; i--) {
				printf_s("%d 1 %d\n", fast[i].t, fast[i].g+1);
			}

			for (int i = result.size() - 1; i >= 0; i--) {
				int g = result[i].g;
				int j = 1;
				std::vector<int> answer;
				answer.clear();
				while (g > 0) {
					if ((g & 1) > 0) {
						answer.push_back(j);
					}
					g = g >> 1;
					j++;
				}
				
				printf_s("%d %d", K - result[i].t, answer.size());
				for (int j = 0; j < answer.size(); j++) {
					int n = answer[j];
					printf_s(" %d", n);
				}
				printf_s("\n");
			}
			printf_s("%d 0\n", K);
			return 0;
		}
	}


	printf_s("-1\n");
}