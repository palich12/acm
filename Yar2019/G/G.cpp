#include <iostream>
#include <vector>
#include <bitset>
#include <map>
#include <queue>

#define MAX_STEP_LENGHT 900
#define MAX_RECURSION_CALL 100000

//one action about reverse hourglasses
struct Action {
	int time;
	int glassMap;
};

//combination of actions that open posibility to count time of some lenght 
struct Step {
	int length;
	std::vector<Action> actions;
};

//specific order of selection reverse combinations of hourglasses for reverse simple combination first
int sort[4][16] = {
	{0b0001, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0011, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0100, 0b0011, 0b0110, 0b0101, 0b0111, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000, 0b0000},
	{0b0001, 0b0010, 0b0100, 0b1000, 0b0011, 0b0110, 0b1010, 0b0101, 0b1100, 0b1001, 0b1110, 0b1101, 0b1011, 0b0111, 0b1111, 0b0000}
};

int N = 0;
int K = 0;
int glasses[4] = { 0,0,0,0 };
std::bitset<MAX_STEP_LENGHT * 21 * 21 * 21 * 21> visitedPoints = { false };
std::vector<Step> timeLine (1450);
int recursionCount = 0;

//find new avalible time points by using new finding step
bool useNewStep(Step step) {
	for (int i = 0; i < K; i++) {
		int newIndex = i + step.length;
		if ((timeLine[i].length > 0 || i == 0)
			&& newIndex <= K
			&& timeLine[newIndex].length == 0) {
			timeLine[newIndex] = step;
			if (newIndex == K) {
				return true;
			}
		}
	}
	return false;
}

//find all avilable combination of actions while we have time for it ;)
bool findPosibleSteps(int time, std::vector<int> state, std::vector<Action> actions) {

	if (time >= MAX_STEP_LENGHT || time > K || recursionCount > MAX_RECURSION_CALL) {
		return false;
	}
	recursionCount++;

	int index = time;
	int m = MAX_STEP_LENGHT;
	for (int i = 0; i < N; i++) {
		index += state[i] * m;
		m *= 21;
	}

	if (visitedPoints[index]) {
		return false;
	}
	visitedPoints[index] = true;

	//generate all posible combination of action
	for (int i = 0; i < (1 << N)-1; i++) {
		std::vector<int> newState;
		int timeToNextAction = 0;
		//generate new state after action
		for (int j = 0; j < N; j++) {
			int newStateValue;
			if ((sort[N-1][i] & (1 << j)) > 0) {
				newStateValue = glasses[j] - state[j];
			}
			else {
				newStateValue = state[j];
			}
			newState.push_back(newStateValue);

			if (newStateValue > 0 && newStateValue < timeToNextAction || timeToNextAction == 0) {
				timeToNextAction = newStateValue;
			}
		}

		if (timeToNextAction == 0) {
			continue;
		}

		//create next action
		Action nextAction;
		nextAction.glassMap = sort[N - 1][i];
		nextAction.time = time;
		std::vector<Action> nextActions = actions;
		nextActions.push_back(nextAction);
		int nextTime = time + timeToNextAction;

		//define state after action
		bool stopFlag = true;
		for (int j = 0; j < N; j++) {
			if (newState[j] > 0) {
				newState[j] = newState[j] - timeToNextAction;
			}
			if (newState[j] > 0) {
				stopFlag = false;
			}
		}
		if (stopFlag) {
			Step step;
			step.actions = nextActions;
			step.length = nextTime;
			if (useNewStep(step)) {
				return true;
			}
			continue;
		}

		if (findPosibleSteps(nextTime, newState, nextActions)) {
			return true;
		}
	}

	return false;
}

int main()
{
	std::vector<int> startState;
	std::vector<Action> startActions;

	scanf_s("%d", &N);
	for (int i = 0; i < N; i++) {
		int g;
		scanf_s("%d", &g);
		glasses[i] = g;
		startState.push_back(0);
	}
	scanf_s("%d", &K);

	//start reqursion
	if (!findPosibleSteps(0, startState, startActions)) {
		printf_s("-1\n");
		return 0;
	}

	//generate result
	std::vector<Action> result;
	int time = K;
	while (time > 0) {
		int actionSize = timeLine[time].actions.size();
		int nextTime = time - timeLine[time].length;
		for (int i = actionSize-1; i >=0; i--) {
			timeLine[time].actions[i].time += nextTime;
			result.push_back(timeLine[time].actions[i]);
		}
		time = nextTime;
	}

	printf_s("%d\n", result.size() + 1);

	for (int i = result.size() - 1; i >= 0; i--) {
		int g = result[i].glassMap;
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

		printf_s("%d %d", result[i].time, answer.size());
		for (int j = 0; j < answer.size(); j++) {
			int n = answer[j];
			printf_s(" %d", n);
		}
		printf_s("\n");
	}
	printf_s("%d 0\n", K);
	return 0;
}