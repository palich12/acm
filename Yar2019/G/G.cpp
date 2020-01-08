#include <iostream>
#include <vector>
#include <bitset>

int N = 0;
int K = 0;
int glasses[4] = {0,0,0,0};
std::bitset<730 * 20 * 20 * 20 * 20> arr = {false};

struct action {
	int t;
	int g;
};

std::vector<action> req(int k, std::vector<int> state) {
	
	std::vector<action> result;
	if ((k & 1) == 1) {
		int index = 0;
		int m = 730;
		for (int i = 0; i < N; i++) {
			index += state[i] * m;
			m *= 20;
		}
		index += k >> 1;
		if (arr[index]) {
			return result;
		}
		else {
			arr[index] = true;
		}
	}

	for (int i = 1; i < 1 << N; i++ ) {
		std::vector<int> newState;
		int tStep = 0;
		for (int j = 0; j < N; j++) {
			int newStateValue;
			if ( (i & (1<<j)) > 0 ) {
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
		if ( rr.size() > 0) {
			rr.push_back(nextAction);
			return rr;
		}
	}

	return result;
}

int main()
{

	scanf_s("%d", &N);

	std::vector<int> startState;
	for (int i = 0; i < N; i++) {
		int g;
		scanf_s("%d", &g);
		glasses[i] = g;
		startState.push_back(0);
	}
	scanf_s("%d", &K);

	std::vector<action> result = req(K, startState);
	if (result.size() == 0) {
		printf_s("-1");
		return 0;
	}

	printf_s("%d\r\n", result.size()+1);
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

		int t = result[i].t;
		int l = answer.size();
		printf_s("%d %d", K-t, l);
		for (int j = 0; j < answer.size(); j++) {
			int n = answer[j];
			printf_s(" %d", n);
		}
		printf_s("\r\n");
	}
	printf_s("%d %d", K, 0);
}



